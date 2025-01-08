using UnityEngine;
using TMPro;
using Mirror;

public class TileCounterMP : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text playerYPositionText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text[] playerOrderText;
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
    private int high = 0;
    private bool set = false;
    private bool set2 = false;
    private void Update()
    {
        if(winPanel.activeSelf && ProgressiveFillWithContinuousAnimations.done && !set2) {
            var players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                if (player.transform.parent.transform.parent.GetComponent<NetworkIdentity>().isLocalPlayer)
                {
                    localPlayer = player;
                    break;
                }
            }
            string name = localPlayer.transform.parent.transform.parent.GetComponent<PlayerObjectController>().PlayerName;
            if(name == playerOrderText[0].text) {
                SteamStatsManager.Instance.IncrementStat("WinsMP");
                Debug.Log("Dodan win");
            } else if(name == playerOrderText[1].text) {
                SteamStatsManager.Instance.IncrementStat("2MP");
                Debug.Log("Dodan 2");
            } else if(name == playerOrderText[2].text) {
                    SteamStatsManager.Instance.IncrementStat("3MP");
                    Debug.Log("Dodan 3");
            } else if(name == playerOrderText[3].text) {
                    SteamStatsManager.Instance.IncrementStat("4MP");
                    Debug.Log("Dodan 4");
            }
            set2 = true;
        }
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
            if(playerY > high) high = playerY;


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

         if(localPlayer != null && !set && !localPlayer.GetComponent<PlayerControllerMP>().enabled) {
            SteamStatsManager.Instance.CheckAndSetHighScoreMP(high);
            SteamStatsManager.Instance.CheckAndSetLowScoreMP(high);
            set = true;
         }
    }
}