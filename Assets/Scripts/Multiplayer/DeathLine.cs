using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class DeathlineController : NetworkBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private WinnerPanelController winnerPanelController; // Reference to WinnerPanel

    // SyncList to maintain consistent death order across server and clients
    private readonly SyncList<PlayerDeathInfo> playerDeathOrder = new SyncList<PlayerDeathInfo>();

    // Structure to hold death information
    public struct PlayerDeathInfo
    {
        public NetworkIdentity identity;
        public string playerName;
        public uint netId;

        public PlayerDeathInfo(NetworkIdentity identity, string name, uint id)
        {
            this.identity = identity;
            this.playerName = name;
            this.netId = id;
        }
    }

    private void Start()
    {
        if (winnerPanelController == null)
        {
            winnerPanelController = FindObjectOfType<WinnerPanelController>();
        }
    }

    private void Update()
    {
        if (isServer)
        {
            MoveDeathline();
        }
    }

    [Server]
    private void MoveDeathline()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer) return;

        if (collision.CompareTag("Player"))
        {
            var playerNetId = collision.GetComponentInParent<NetworkIdentity>();
            if (playerNetId != null)
            {
                HandlePlayerDeath(playerNetId);
            }
        }
    }

    [Server]
    private void HandlePlayerDeath(NetworkIdentity playerNetId)
    {
        // Check if player is already in death order
        foreach (var deathInfo in playerDeathOrder)
        {
            if (deathInfo.netId == playerNetId.netId)
                return;
        }

        // Get player's name
        var playerController = playerNetId.GetComponent<PlayerObjectController>();
        string playerName = playerController != null ? playerController.PlayerName : "Unknown Player";

        // Add to death order
        PlayerDeathInfo deathInfo2 = new PlayerDeathInfo(playerNetId, playerName, playerNetId.netId);
        playerDeathOrder.Add(deathInfo2);

        // Update winner panel
        if (winnerPanelController != null)
        {
            winnerPanelController.AddPlayerToDeathOrder(playerName, playerNetId.netId);
        }

        // Notify clients
        RpcPlayerDeath(playerNetId.gameObject, playerName);

        // Check for game end
        StartCoroutine(DelayedCheckForGameEnd());
    }

    [ClientRpc]
    private void RpcPlayerDeath(GameObject player, string playerName)
    {
        Debug.Log($"{playerName} has died!");

        if (player == null) return;

        // Disable player visuals and controller
        var playerVisual = player.transform.GetChild(0).GetChild(1).gameObject;
        if (playerVisual != null)
        {
            var spriteRenderer = playerVisual.GetComponent<SpriteRenderer>();
            var playerController = playerVisual.GetComponent<PlayerControllerMP>();
            
            if (spriteRenderer != null) spriteRenderer.enabled = false;
            if (playerController != null) playerController.enabled = false;
        }

        // Handle spectator camera for local player
        if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            var cameraFollow = FindObjectOfType<CameraFollowMP>();
            if (cameraFollow != null)
            {
                cameraFollow.FindOtherAlivePlayer();
            }
        }
    }

    [Server]
    private System.Collections.IEnumerator DelayedCheckForGameEnd()
    {
        yield return new WaitForEndOfFrame();

        var allPlayers = FindObjectsOfType<PlayerControllerMP>();
        int aliveCount = 0;

        foreach (var player in allPlayers)
        {
            if (player.isActiveAndEnabled)
            {
                aliveCount++;
            }
        }

        Debug.Log($"Alive players: {aliveCount}");

        if (aliveCount <= 1) // Changed to <= 1 to handle winner when one player remains
        {
            RpcEndGame();
        }
    }

    [ClientRpc]
    private void RpcEndGame()
    {
        Debug.Log("Game Over!");
        
        // Ensure winner panel is shown
        if (winnerPanelController != null)
        {
            winnerPanelController.ShowFinalResults();
        }
    }
}
