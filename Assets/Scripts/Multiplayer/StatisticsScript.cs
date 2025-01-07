using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Steamworks;
using System;
public class StatisticsScript : MonoBehaviour
{
    private readonly string[] statApiNames = { "GamesSP", "WinsSP", "100SP", "KillsSP", "DeathsSP", "CoinsSP" };
    private readonly string[] statLabelsSP = { "GAMES PLAYED: ", "COMPLETIONS: ", "PERFECT COMPLETIONS: ", "KILLS: ", "DEATHS: ", "COINS: " };
    [SerializeField] Image[] storyImagesSP; // Array for UI images (e.g., win % and K/D ratio)
    [SerializeField] TMP_Text[] storyStatTextsSP; // Array to hold TMP_Text references
    private readonly string[] statApiNamesEN = { "GamesEN", "HighEN", "LowEN" };
    private readonly string[] statLabelsEN = { "GAMES PLAYED: ", "HIGHEST: ", "LOWEST: "};
    [SerializeField] Image[] last5EndlessImages; // Array for UI images (e.g., win % and K/D ratio)
    [SerializeField] TMP_Text[] last5EndlessTexts; // Array to hold TMP_Text references
    [SerializeField] TMP_Text[] storyStatTextsEN; // Array to hold TMP_Text references
    private readonly string[] statApiNamesSR = { "GamesSR", "WinsSR", "100SR", "FastSR", "SlowSR", "CoinsSR"};
    private readonly string[] statLabelsSR = { "GAMES PLAYED: ", "COMPLETIONS: ", "PERFECT COMPLETIONS: ", "FASTEST: ", "SLOWEST: " , "COINS: "};
    [SerializeField] Image[] storyImagesSR; // Array for UI images (e.g., win % and K/D ratio)
    [SerializeField] TMP_Text[] storyStatTextsSR; // Array to hold TMP_Text references
    [SerializeField] GameObject[] panelsUp; // Array for UI images (e.g., win % and K/D ratio)
    [SerializeField] GameObject[] panelsDown; // Array for UI images (e.g., win % and K/D ratio)
    [SerializeField] TMP_Dropdown dropdown;

    void Start()
    {
        SteamUserStats.RequestCurrentStats();
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }
        SetStoryStats();
    }
     void OnDropdownValueChanged(int value)
{
    // Get the name of the selected option
    string selectedOptionName = dropdown.options[value].text;

    // Hide all panels
    HideAllPanels();

    // Show the panels corresponding to the selected option
    ShowPanelsForMode(selectedOptionName);
}

// Method to hide all panels
void HideAllPanels()
{
    foreach (GameObject obj in panelsUp)
    {
        obj.SetActive(false);
    }
    foreach (GameObject obj in panelsDown)
    {
        obj.SetActive(false);
    }
}
    public string FormatTimeFromMilliseconds(float milliseconds)
    {
        TimeSpan time = TimeSpan.FromMilliseconds(milliseconds);

        if (time.Hours > 0)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}", time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
        }
        else
        {
            return string.Format("{0:D2}:{1:D2}.{2:D3}", time.Minutes, time.Seconds, time.Milliseconds);
        }
    }

// Method to show panels based on the selected mode
void ShowPanelsForMode(string selectedOptionName)
{
    switch (selectedOptionName)
    {
        case "Story mode":
            panelsUp[0].SetActive(true);
            panelsDown[0].SetActive(true);
            SetStoryStats();
            break;
        case "Speedrun mode":
            panelsUp[1].SetActive(true);
            panelsDown[1].SetActive(true);
            SetSpeedrunStats();
            break;
        case "Endless mode":
            panelsUp[2].SetActive(true);
            panelsDown[2].SetActive(true);
            SetEndlessStats();
            break;
        default:
            // Handle any default case if needed
            break;
    }
}

private void SetEndlessStats(){
        int highest = SteamStatsManager.Instance.GetStatInt("HighEN");

        // Update stat texts
        for (int i = 0; i < statApiNamesEN.Length; i++)
        {
            if(statApiNamesEN[i] == "HighEN" || statApiNamesEN[i] == "LowEN") {
                int statValue = SteamStatsManager.Instance.GetStatInt(statApiNamesEN[i]);
                if(statValue == 0 ||statValue == 999999) storyStatTextsEN[i].text = statLabelsEN[i]+ "-";
                else storyStatTextsEN[i].text = statLabelsEN[i] + (statValue);
                Debug.Log("NO sta sad" + statValue);
            } else{
            int statValue = SteamStatsManager.Instance.GetStatInt(statApiNamesEN[i]);
            storyStatTextsEN[i].text = statLabelsEN[i] + statValue;
            }
        }

        float sum = 0;
        string all = PlayerPrefs.GetString("ALLENDLESS", "");
        if(all != ""){
        string[] allScores = all.Split(',');
        foreach(string s in allScores) {
            Debug.Log(s);
            sum += int.Parse(s);
        }
        sum = sum/allScores.Length;
        }
        // Update percentage and ratio texts
        storyStatTextsEN[statApiNamesEN.Length].text = "AVERAGE: " + sum.ToString("F2"); 
        storyStatTextsEN[statApiNamesEN.Length+1].text = "RECORD\n" + highest;

                // Get the scores stored in PlayerPrefs, defaulting to "0,0,0,0,0" if not found
        string savedScores = PlayerPrefs.GetString("LAST5ENDLESS", "0,0,0,0,0");

        // Split the scores string into an array (assuming comma-separated values)
        string[] scoreStrings = savedScores.Split(',');
        float[] scores = new float[scoreStrings.Length];

        for (int i = 0; i < scoreStrings.Length; i++)
                {
                    if (float.TryParse(scoreStrings[i], out float score))
                    {
                        scores[i] = score;
                    }
                    else
                    {
                        scores[i] = 0f; // Default to 0 if parsing fails
                    }
                }
        
                // Find the maximum score to scale against
        float maxScore = Mathf.Max(scores);

        // Iterate over the Image array and set the fill amount
        for (int i = 0; i < last5EndlessImages.Length; i++)
        {
            // Get the score for the current image
            float currentScore = i < scores.Length ? scores[i] : 0f;

            // Scale the score based on the max score, and clamp it between 0 and 1
            last5EndlessImages[i].fillAmount = maxScore > 0 ? Mathf.Clamp01(currentScore / maxScore) : 0f;
            if(currentScore < sum) last5EndlessImages[i].color = Color.red;
            else last5EndlessImages[i].color = Color.green;
            last5EndlessTexts[i].text = currentScore + "";
        }

}
private void SetSpeedrunStats(){
    int gamesPlayed = SteamStatsManager.Instance.GetStatInt("GamesSR");
        int gamesWon = SteamStatsManager.Instance.GetStatInt("WinsSR");
        int kills = SteamStatsManager.Instance.GetStatInt("KillsSR");
        int deaths = SteamStatsManager.Instance.GetStatInt("DeathsSR");

        // Update stat texts
        for (int i = 0; i < statApiNamesSR.Length; i++)
        {
            if(statApiNamesSR[i] == "FastSR" || statApiNamesSR[i] == "SlowSR") {
                int statValue = SteamStatsManager.Instance.GetStatInt(statApiNamesSR[i]);
                if(statValue == 999999 ||statValue == 0) storyStatTextsSR[i].text = statLabelsSR[i]+ "-";
                else storyStatTextsSR[i].text = statLabelsSR[i] + FormatTimeFromMilliseconds(statValue);
            } else{
            int statValue = SteamStatsManager.Instance.GetStatInt(statApiNamesSR[i]);
            storyStatTextsSR[i].text = statLabelsSR[i] + statValue;
            }
        }

        // Calculate win percentage and kill/death ratio
        float winPercentage = (gamesPlayed > 0) ? ((float)gamesWon / gamesPlayed * 100f) : 0f;
        float kdRatio = (kills > 0) ? ((float)kills / Mathf.Max(deaths, 1)) : 0f; // Prevent division by zero

        // Update percentage and ratio texts
        storyStatTextsSR[statApiNamesSR.Length].text = "WIN %\n" + winPercentage.ToString("F2");
        storyStatTextsSR[statApiNamesSR.Length + 1].text = "K/D\n" + kdRatio.ToString("F2");

        // Update UI images (if assigned)
        if (storyImagesSR.Length > 0)
        {
            // Assuming storyImagesSP[0] is win percentage and storyImagesSP[1] is K/D ratio
            if (storyImagesSR[0] != null)
                storyImagesSR[0].fillAmount = winPercentage / 100f; // Normalized fill amount (0 to 1)

            if (storyImagesSR[1] != null)
                storyImagesSR[1].fillAmount = Mathf.Clamp01(kdRatio / 10f); // Normalized fill amount (scale K/D ratio)
        }
}

    public void ResetStats(bool resetAchievements)
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam is not initialized. Cannot reset stats.");
            return;
        }

        // Reset all stats and optionally achievements
        bool success = SteamUserStats.ResetAllStats(resetAchievements);
        if (success)
        {
            Debug.Log("Stats reset successfully. Requesting stats update...");
            // Request the stats again to sync the reset with Steam
            SteamUserStats.RequestCurrentStats();
        }
        else
        {
            Debug.LogError("Failed to reset stats.");
        }
    }

    private void SetStoryStats()
    {
        int gamesPlayed = SteamStatsManager.Instance.GetStatInt("GamesSP");
        int gamesWon = SteamStatsManager.Instance.GetStatInt("WinsSP");
        int kills = SteamStatsManager.Instance.GetStatInt("KillsSP");
        int deaths = SteamStatsManager.Instance.GetStatInt("DeathsSP");

        // Update stat texts
        for (int i = 0; i < statApiNames.Length; i++)
        {
            int statValue = SteamStatsManager.Instance.GetStatInt(statApiNames[i]);
            storyStatTextsSP[i].text = statLabelsSP[i] + statValue;
        }

        // Calculate win percentage and kill/death ratio
        float winPercentage = (gamesPlayed > 0) ? ((float)gamesWon / gamesPlayed * 100f) : 0f;
        float kdRatio = (kills > 0) ? ((float)kills / Mathf.Max(deaths, 1)) : 0f; // Prevent division by zero

        // Update percentage and ratio texts
        storyStatTextsSP[statApiNames.Length].text = "WIN %\n" + winPercentage.ToString("F2");
        storyStatTextsSP[statApiNames.Length + 1].text = "K/D\n" + kdRatio.ToString("F2");

        // Update UI images (if assigned)
        if (storyImagesSP.Length > 0)
        {
            // Assuming storyImagesSP[0] is win percentage and storyImagesSP[1] is K/D ratio
            if (storyImagesSP[0] != null)
                storyImagesSP[0].fillAmount = winPercentage / 100f; // Normalized fill amount (0 to 1)

            if (storyImagesSP[1] != null)
                storyImagesSP[1].fillAmount = Mathf.Clamp01(kdRatio / 10f); // Normalized fill amount (scale K/D ratio)
        }
    }
}
