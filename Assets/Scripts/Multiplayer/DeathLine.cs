using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class DeathlineController : NetworkBehaviour
{
    [SerializeField] private float speed = 1f;
    [SerializeField] private WinnerPanelController winnerPanelController; // Reference to WinnerPanel

    // SyncList to maintain consistent death order across server and clients
    private readonly SyncList<PlayerDeathInfo> playerDeathOrder = new SyncList<PlayerDeathInfo>();
    private PlayerSpawnManager spawnManager;

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
       [Server]
    private void StartGame()
    {
        Debug.Log("All players are ready. Starting gameplay...");
        enabled = true; // Enable the Deathline movement
    }

    private void Start()
    {
        if (winnerPanelController == null)
        {
            winnerPanelController = FindObjectOfType<WinnerPanelController>();
        }
        spawnManager = FindObjectOfType<PlayerSpawnManager>();
        if (spawnManager != null)
        {
            if(isServer) spawnManager.OnAllPlayersReady += StartGame;
        }
    }

    private void Update()
    {
        if (isServer && NetworkedEndlessTiles.startDeathLine)
        {
            UpdateSpeedBasedOnY();
            MoveDeathline();
        }
    }

    private float lastYPosition = 0f; // Tracks the last Y position where speed was updated
private float speedIncrement = 1f; // Adjust the increment value as needed

private void UpdateSpeedBasedOnY()
{
    float currentYPosition = transform.position.y;

    // Check if the Y position has increased by 100 units
    if (currentYPosition - lastYPosition >= 100f)
    {
        speed += speedIncrement; // Increase speed
        lastYPosition = currentYPosition; // Update the last Y position

        Debug.Log($"Speed increased to: {speed}");
    }
}

    [Server]
    private float FindHighestPlayerY()
    {
        float highestY = float.MinValue;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player != null)
            {
                float playerHeight = player.transform.position.y;
                if (playerHeight > highestY)
                {
                    highestY = playerHeight;
                }
            }
        }
        return highestY;
    }

    [Server]
    private void MoveDeathline()
    {
        float highestPlayerY = FindHighestPlayerY();
        float dangerLineY = transform.position.y;

        if(Mathf.Abs(dangerLineY -highestPlayerY) > 30f) {
            if(dangerLineY < highestPlayerY -30f){
                transform.position = new Vector3(transform.position.x, highestPlayerY - 30f, transform.position.z);
            } else if(dangerLineY > highestPlayerY + 30f) {
                //transform.position = new Vector3(transform.position.x, highestPlayerY + 30f, transform.position.z);
            }
        } else {
                transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
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

    [Server]
public void HandlePlayerDisconnection(PlayerDeathInfo deathInfo) {
    Debug.Log("Bok2");
    playerDeathOrder.Add(deathInfo);
    if (winnerPanelController != null) {
        winnerPanelController.AddPlayerToDeathOrder(deathInfo.playerName, deathInfo.netId);
    }
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
            if (playerController != null) {
                playerController.setPlayerChar = false;
            playerController.enabled = false;
            }
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

        if (aliveCount < 1) // Changed to <= 1 to handle winner when one player remains
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
