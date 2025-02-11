using UnityEngine;
using System;
using System.Linq;

public class PlayDaysTracker : MonoBehaviour
{
    private const string PLAY_DAYS_KEY = "PlayDays";
    private const char SEPARATOR = ',';

    private void Start()
    {
        // Record today's play session
        RecordToday();
        
        // Check if player has played every day of the week
        CheckAllDaysPlayed();
    }

    private void RecordToday()
    {
        // Get current day
        string today = DateTime.Now.DayOfWeek.ToString();
        
        // Get existing play days
        string existingDays = PlayerPrefs.GetString(PLAY_DAYS_KEY, "");
        string[] playDays = existingDays.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
        
        // Add today if not already recorded
        if (!playDays.Contains(today))
        {
            string updatedDays = string.IsNullOrEmpty(existingDays) 
                ? today 
                : existingDays + SEPARATOR + today;
                
            PlayerPrefs.SetString(PLAY_DAYS_KEY, updatedDays);
            PlayerPrefs.Save();
            
            Debug.Log($"Recorded play session for {today}");
        }
    }

    private void CheckAllDaysPlayed()
    {
        string savedDays = PlayerPrefs.GetString(PLAY_DAYS_KEY, "");
        string[] playDays = savedDays.Split(new[] { SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
        
        // Create arrays of all days of the week and played days
        var allDays = Enum.GetNames(typeof(DayOfWeek));
        var playedDays = playDays.Distinct().ToArray();
        
        // Check if all days are present
        bool hasPlayedAllDays = allDays.All(day => playedDays.Contains(day));
        
        if (hasPlayedAllDays)
        {
            Debug.Log("BRAVO! You have played the game on every day of the week!");
            Achievements.UnlockPlayEveryDayAchievement();
        }
        
        // Log current progress
        Debug.Log($"Days played so far: {string.Join(", ", playedDays)}");
    }

    // Method to reset tracking (for testing)
    public void ResetTracking()
    {
        PlayerPrefs.DeleteKey(PLAY_DAYS_KEY);
        Debug.Log("Play days tracking has been reset");
    }

    // Method to manually check current progress
    public void CheckProgress()
    {
        CheckAllDaysPlayed();
    }
}