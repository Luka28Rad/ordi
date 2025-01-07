using UnityEngine;
using Steamworks;

public class SteamStatsManager : MonoBehaviour
{
    public static SteamStatsManager Instance { get; private set; }

private void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}

    private void Start()
    {
        if (SteamManager.Initialized)
        {
            // Request the current stats from Steam
            SteamUserStats.RequestCurrentStats();
            Debug.Log("Requested stats");
        }
    }

    public void IncrementStat(string statName)
    {
        if (!SteamManager.Initialized) return;
        Debug.Log(statName +" pa daj vise jebote pas");
        int statValue;
        if (SteamUserStats.GetStat(statName, out statValue))
        {
            statValue++;
            SteamUserStats.SetStat(statName, statValue);
            SteamUserStats.StoreStats();
        }
    }

    public void UpdateStat(string statName, int value)
    {
        if (!SteamManager.Initialized) return;

        SteamUserStats.SetStat(statName, value);
        SteamUserStats.StoreStats();
    }

    public void UpdateStat(string statName, float value)
    {
        if (!SteamManager.Initialized) return;

        SteamUserStats.SetStat(statName, value);
        SteamUserStats.StoreStats();
    }

    public void CheckAndSetHighScore(int newScore)
    {
        if (!SteamManager.Initialized) return;

        int currentHighScore = 999999;
        if (SteamUserStats.GetStat("FastSR", out currentHighScore))
        {
            if (newScore < currentHighScore)
            {
                SteamUserStats.SetStat("FastSR", newScore);
                SteamUserStats.StoreStats();
            }
        }
    }

    public void CheckAndSetLowScore(int newScore)
    {
        if (!SteamManager.Initialized) return;

        int currentLowScore = 0;
        if (SteamUserStats.GetStat("SlowSR", out currentLowScore))
        {
            if (newScore > currentLowScore)
            {
                SteamUserStats.SetStat("SlowSR", newScore);
                SteamUserStats.StoreStats();
            }
        }
    }

        public void CheckAndSetHighScoreEndless(int newScore)
    {
        if (!SteamManager.Initialized) return;

        int currentHighScore = 0;
        if (SteamUserStats.GetStat("HighEN", out currentHighScore))
        {
            if (newScore > currentHighScore || currentHighScore == 0)
            {
                SteamUserStats.SetStat("HighEN", newScore);
                SteamUserStats.StoreStats();
            }
        }
    }

    public void CheckAndSetLowScoreEndless(int newScore)
    {
        if (!SteamManager.Initialized) return;

        int currentLowScore = 999999;
        if (SteamUserStats.GetStat("LowEN", out currentLowScore))
        {
            if (newScore < currentLowScore || currentLowScore == 999999)
            {
                SteamUserStats.SetStat("LowEN", newScore);
                SteamUserStats.StoreStats();
            }
        }
    }

    // GETTING STATS

    public int GetStatInt(string statName)
{
    if (!SteamManager.Initialized) return 0; // Ensure Steam is initialized

    int statValue = 0;
    if (SteamUserStats.GetStat(statName, out statValue))
    {
        return statValue;
    }
    else
    {
        Debug.LogWarning($"Failed to retrieve stat: {statName}");
        return 0;
    }
}

public float GetStatFloat(string statName)
{
    if (!SteamManager.Initialized) return 0f; // Ensure Steam is initialized

    float statValue = 0f;
    if (SteamUserStats.GetStat(statName, out statValue))
    {
        return statValue;
    }
    else
    {
        Debug.LogWarning($"Failed to retrieve stat: {statName}");
        return 0f;
    }
}

}
