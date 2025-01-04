using UnityEngine;
using TMPro;
using Mirror;

public class TileCounterMP : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text playerYPositionText;
    [SerializeField] private TMP_Text distanceToDeathlineText;

    public static GameObject localPlayer;
    private GameObject deathline;

    private void Start()
    {
        // Find the deathline object
        deathline = GameObject.FindGameObjectWithTag("Deathline");
        
        // Ensure UI references are set
        if (playerYPositionText == null || distanceToDeathlineText == null)
        {
            Debug.LogWarning("TileCounterMP: Text references not set!");
        } else {
            playerYPositionText.text = "0";
            distanceToDeathlineText.text = "0";
        }
    }

    private void Update()
    {
        // Find local player if we haven't already
        if (localPlayer == null)
        {
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                if (player.transform.parent.transform.parent.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    localPlayer = player;
                    break;
                }
            }
        }
        if(deathline == null) {
            deathline = GameObject.FindGameObjectWithTag("Deathline");
        }

        // Update UI if we have all necessary references
        if (localPlayer != null && deathline != null)
        {
            // Get Y positions
            int playerY = Mathf.RoundToInt(localPlayer.transform.position.y / 3);
            int deathlineY = Mathf.RoundToInt(deathline.transform.position.y / 3);


            // Calculate distance
            float distance = playerY - deathlineY;

            // Update UI texts
            if (playerYPositionText != null)
            {
                playerYPositionText.text = $"{playerY}";
            }

            if (distanceToDeathlineText != null)
            {
                distanceToDeathlineText.text = $"{distance}";
            }
        } else{ Debug.Log((localPlayer != null));
         Debug.Log( deathline != null);}
    }
}