using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class DeathlineController : NetworkBehaviour
{
    [SerializeField] private float speed = 2f; // Speed of the Deathline

    // List to store the order of player deaths
    private readonly List<NetworkIdentity> playerDeathOrder = new List<NetworkIdentity>();

    private void Update()
    {
        if (isServer) // Only the server controls movement
        {
            MoveDeathline();
        }
    }

    [Server] // Ensure this runs only on the server
    private void MoveDeathline()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime); // Move upwards
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer) return; // Only the server handles collisions

        if (collision.CompareTag("Player")) // Check for player collisions
        {
            var playerNetId = collision.GetComponentInParent<NetworkIdentity>();
            if (playerNetId != null)
            {
                HandlePlayerDeath(playerNetId);
            }
        }
    }

    [Server] // Handle player death on the server
    private void HandlePlayerDeath(NetworkIdentity playerNetId)
    {
        // Add the player to the death order list if not already present
        if (!playerDeathOrder.Contains(playerNetId))
        {
            playerDeathOrder.Add(playerNetId);
        }

        // Notify all clients of the player's death
        RpcPlayerDeath(playerNetId.gameObject);

        // Delay the game-end check to ensure player state is fully updated
        StartCoroutine(DelayedCheckForGameEnd());
    }

    [ClientRpc] // Updates clients about the player's death
    private void RpcPlayerDeath(GameObject player)
    {
        var playerVisual = player.transform.GetChild(0).GetChild(1).gameObject;

        // Disable player visuals and input
        playerVisual.GetComponent<SpriteRenderer>().enabled = false;
        playerVisual.GetComponent<PlayerControllerMP>().enabled = false;

        // Disable additional components
        player.transform.GetChild(0).gameObject.SetActive(false); // UI or effects

        // Move the camera to another player
        var cameraFollow = FindObjectOfType<CameraFollowMP>();
        if (cameraFollow != null)
        {
            cameraFollow.FindOtherAlivePlayer();
        }
    }

    [Server] // Delayed check for game-end
    private System.Collections.IEnumerator DelayedCheckForGameEnd()
    {
        yield return new WaitForEndOfFrame(); // Wait for the current frame to complete

        var allPlayers = FindObjectsOfType<PlayerControllerMP>();
        int aliveCount = 0;

        foreach (var player in allPlayers)
        {
            if (player.isActiveAndEnabled) // Check if player is still alive
            {
                aliveCount++;
            }
        }

        Debug.Log($"Alive players: {aliveCount}");

        if (aliveCount == 0)
        {
            Debug.Log("Game Over");
            RpcEndGame();
        }
    }

    [ClientRpc] // Notify clients that the game is over
    private void RpcEndGame()
    {
        Debug.Log("Game Over! Everyone is dead!");

        // Log death order for debugging
        foreach (var playerNetId in playerDeathOrder)
        {
            Debug.Log($"Player {playerNetId.netId} died");
        }

        // Implement game-over UI or logic here
    }
}
