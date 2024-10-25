using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
public class Achievements : MonoBehaviour
{
private void Start()
    {
        // Initialize Steamworks (ensure you have Steam running)
        if (!SteamManager.Initialized)
        {
            Debug.LogError("SteamAPI failed to initialize.");
        } else {
            UnlockWelcomeAchievement();
        }
    }

    public void UnlockWelcomeAchievement()
    {
        
        // Check if the Steamworks API is initialized
        if (SteamManager.Initialized)
        {
            // Set the achievement
            SteamUserStats.SetAchievement("WELCOME_ACHIEVEMENT");

            // Optionally, call to store the achievement data
            SteamUserStats.StoreStats();
            bool achieved;
            Debug.Log("Achievement WELCOME_ACHIEVEMENT unlocked!");
SteamUserStats.GetAchievement("ACHIEVEMENT_ID", out achieved);
Debug.Log("Achievement unlocked: " + achieved);

        }
        else
        {
            Debug.LogError("Steamworks API is not initialized.");
        }
    }

    private void OnDestroy()
    {
        // Shutdown Steamworks when done
        SteamAPI.Shutdown();
    }
}
