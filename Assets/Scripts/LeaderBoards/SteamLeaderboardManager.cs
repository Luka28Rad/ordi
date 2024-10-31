using Steamworks;
using UnityEngine;
using System.Collections;

public class SteamLeaderboardManager : MonoBehaviour
{
    private const string s_leaderboardName = "SPEEDRUN_LEADERBOARD";
    private const string s_leaderboardNameEndless = "ENDLESS_LEADERBOARD";

    public static SteamLeaderboardManager instance;
    public static bool s_initialized = false;
    public static bool s_initialized_Endless = false;
    public static SteamLeaderboard_t s_currentLeaderboard;
    public static SteamLeaderboard_t s_currentLeaderboard_Endless;
    
    private static readonly CallResult<LeaderboardFindResult_t> m_findResult = new CallResult<LeaderboardFindResult_t>();
    private static readonly CallResult<LeaderboardScoreUploaded_t> m_uploadResult = new CallResult<LeaderboardScoreUploaded_t>();
    private static readonly CallResult<LeaderboardFindResult_t> m_findResult_Endless = new CallResult<LeaderboardFindResult_t>();
    private static readonly CallResult<LeaderboardScoreUploaded_t> m_uploadResult_Endless = new CallResult<LeaderboardScoreUploaded_t>();

    //private const ELeaderboardUploadScoreMethod s_leaderboardMethod = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate;
    private const ELeaderboardUploadScoreMethod s_leaderboardMethod = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest;

    public bool getScores = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForSteamInitialization());
    }

    private IEnumerator WaitForSteamInitialization()
    {
        // Wait until SteamManager is fully initialized
        yield return new WaitUntil(() => SteamManager.Initialized);

        // Now we can initialize the leaderboard
        Init();
        InitEndless();
    }

public static void UpdateScore(int score)
{
    if (!SteamManager.Initialized)
        return;

    if (!s_initialized)
    {
        Init();
        Debug.Log("Can't upload to the leaderboard because it isn't loaded yet");
        return;
    }

    Debug.Log($"Uploading score: {score}, Leaderboard: {s_currentLeaderboard}");

    SteamAPICall_t hSteamAPICall = SteamUserStats.UploadLeaderboardScore(s_currentLeaderboard, s_leaderboardMethod, score, null, 0);
    m_uploadResult.Set(hSteamAPICall, OnLeaderboardUploadResult);
}

public static void UpdateScoreEndless(int score)
{
    if (!SteamManager.Initialized)
        return;

    if (!s_initialized_Endless)
    {
        InitEndless();
        Debug.Log("Can't upload to the leaderboard because it isn't loaded yet");
        return;
    }

    Debug.Log($"Uploading score: {score}, Leaderboard: {s_currentLeaderboard_Endless}");

    SteamAPICall_t hSteamAPICall = SteamUserStats.UploadLeaderboardScore(s_currentLeaderboard_Endless, s_leaderboardMethod, score, null, 0);
    m_uploadResult.Set(hSteamAPICall, OnLeaderboardUploadResultEndless);
}

    public static void Init()
    {
        if (!SteamManager.Initialized)
            return;

        SteamAPICall_t hSteamAPICall = SteamUserStats.FindLeaderboard(s_leaderboardName);
        m_findResult.Set(hSteamAPICall, OnLeaderboardFindResult);
    }

    public static void InitEndless()
    {
        if (!SteamManager.Initialized)
            return;

        SteamAPICall_t hSteamAPICall_Endless = SteamUserStats.FindLeaderboard(s_leaderboardNameEndless);
        m_findResult_Endless.Set(hSteamAPICall_Endless, OnLeaderboardFindResultEndless);
    }

    private static void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool failure)
    {
        s_currentLeaderboard = pCallback.m_hSteamLeaderboard;
        s_initialized = true;
    }
    private static void OnLeaderboardFindResultEndless(LeaderboardFindResult_t pCallback, bool failure)
    {
        s_currentLeaderboard_Endless = pCallback.m_hSteamLeaderboard;
        s_initialized_Endless = true;
    }

    private static void OnLeaderboardUploadResult(LeaderboardScoreUploaded_t pCallback, bool failure)
    {
        Debug.Log("STEAM LEADERBOARDS: failure - " + failure + " Completed - " + pCallback.m_bSuccess + " NewScore: " + pCallback.m_nGlobalRankNew + " Score " + pCallback.m_nScore + " HasChanged - " + pCallback.m_bScoreChanged);
    }
    private static void OnLeaderboardUploadResultEndless(LeaderboardScoreUploaded_t pCallback, bool failure)
    {
        Debug.Log("STEAM ENDLESS LEADERBOARDS: failure - " + failure + " Completed - " + pCallback.m_bSuccess + " NewScore: " + pCallback.m_nGlobalRankNew + " Score " + pCallback.m_nScore + " HasChanged - " + pCallback.m_bScoreChanged);
    }
}
