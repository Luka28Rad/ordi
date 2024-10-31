using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class SteamLeaderboardDisplay : MonoBehaviour
{
    public GameObject resultPrefab; // Reference to the ResultPrefab
    public Transform scoreContainer; // Parent object to organize instantiated prefabs
    public Transform scoreContainerEndless; // Parent object to organize instantiated prefabs
    public TMP_Text info;

    [HideInInspector]
    public SteamLeaderboardEntries_t m_SteamLeaderboardEntries;
    [HideInInspector]
    public SteamLeaderboardEntries_t m_SteamLeaderboardEntries_Endless;

    private static readonly CallResult<LeaderboardScoresDownloaded_t> m_scoresDownloadedResult = new CallResult<LeaderboardScoresDownloaded_t>();
    private static readonly CallResult<LeaderboardScoresDownloaded_t> m_userPlacementDownloadedResult = new CallResult<LeaderboardScoresDownloaded_t>();
    private static readonly CallResult<LeaderboardScoresDownloaded_t> m_previousRecordDownloadedResult = new CallResult<LeaderboardScoresDownloaded_t>();

    private static readonly CallResult<LeaderboardScoresDownloaded_t> m_scoresDownloadedResultEndless = new CallResult<LeaderboardScoresDownloaded_t>();
    private static readonly CallResult<LeaderboardScoresDownloaded_t> m_userPlacementDownloadedResultEndless = new CallResult<LeaderboardScoresDownloaded_t>();
    private static readonly CallResult<LeaderboardScoresDownloaded_t> m_previousRecordDownloadedResultEndless = new CallResult<LeaderboardScoresDownloaded_t>();

    public static long userPreviousRecord = -25;
    public static long userPreviousRecordEndless = -25;

    public TMP_Text rankUserText;
    public TMP_Text scoreUserText;

    public TMP_Text rankUserTextEndless;
    public TMP_Text scoreUserTextEndless;
    private static TMP_Text rankUserTextStatic;
    private static TMP_Text scoreUserTextStatic;
    private static TMP_Text rankUserTextEndlessStatic;
    private static TMP_Text scoreUserTextEndlessStatic;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "LeaderboardScene")
        {
            Achievements.UnlockCheckLeaderboardsAchievement();
            scoreUserTextStatic = scoreUserText;
            rankUserTextStatic = rankUserText;
            rankUserTextEndlessStatic = rankUserTextEndless;
            scoreUserTextEndlessStatic = scoreUserTextEndless;
        }
    }

    private void OnEnable()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogWarning("Steam is not initialized.");
            return;
        }

        if (SceneManager.GetActiveScene().name == "Bossfight" && Variables.gameMode == "Speedrun")
        {
            GetUserPreviousRecord();
        }
        else if (SceneManager.GetActiveScene().name == "LeaderboardScene")
        {
            if (info)
            {
                info.gameObject.SetActive(true);
                info.text = "LOADING SCORES...";
            }
            GetScores();
            GetUserPlacement();
            GetUserPlacementEndless();
            GetScoresEndless();
        } else if(SceneManager.GetActiveScene().name == "EndlessLevel") {
            GetUserPreviousRecordEndless();
        }
    }

    private static void GetUserPlacement()
    {
        if (!SteamLeaderboardManager.s_initialized)
        {
            Debug.LogWarning("Leaderboard not initialized. Initializing now...");
            SteamLeaderboardManager.Init();
            return;
        }

        // Download the leaderboard entries around the user
        SteamAPICall_t hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(SteamLeaderboardManager.s_currentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -1, -1);
        m_userPlacementDownloadedResult.Set(hSteamAPICall, OnUserPlacementDownloaded);
    }
        private static void GetUserPlacementEndless()
    {
        if (!SteamLeaderboardManager.s_initialized_Endless)
        {
            Debug.LogWarning("Leaderboard endless not initialized. Initializing now...");
            SteamLeaderboardManager.InitEndless();
            return;
        }

        // Download the leaderboard entries around the user
        SteamAPICall_t hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(SteamLeaderboardManager.s_currentLeaderboard_Endless, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -1, -1);
        m_userPlacementDownloadedResultEndless.Set(hSteamAPICall, OnUserPlacementDownloadedEndless);
    }

    private static void OnUserPlacementDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
    {
        if (bIOFailure)
        {
            Debug.LogError("Failed to download user placement scores.");
            return;
        }

        if (pCallback.m_cEntryCount > 0)
        {
            LeaderboardEntry_t entry;
            SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, 0, out entry, null, 0);

            // Get the user's rank and score
            int rankUser = entry.m_nGlobalRank;
            long scoreUser = entry.m_nScore;

            rankUserTextStatic.text = rankUser > 0 ? rankUser.ToString() : "-";
            scoreUserTextStatic.text = scoreUser > 0 ? FormatTimeFromMilliseconds(scoreUser) : "No time yet.";
            Debug.Log($"User's Rank: {rankUser}, Score: {scoreUser}");
        }
        else
        {
            Debug.Log("No entries found for the user.");
            rankUserTextStatic.text = "-";
            scoreUserTextStatic.text = "No time yet.";
        }
    }

       private static void OnUserPlacementDownloadedEndless(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
    {
        if (bIOFailure)
        {
            Debug.LogError("Failed to download user placement scores.");
            return;
        }

        if (pCallback.m_cEntryCount > 0)
        {
            LeaderboardEntry_t entry;
            SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, 0, out entry, null, 0);

            // Get the user's rank and score
            int rankUser = entry.m_nGlobalRank;
            long scoreUser = entry.m_nScore;

            rankUserTextEndlessStatic.text = rankUser > 0 ? rankUser.ToString() : "-";
            scoreUserTextEndlessStatic.text = scoreUser > 0 ? (scoreUser.ToString()) : "No time yet.";

            Debug.Log($"User's Rank: {rankUser}, Score: {scoreUser}");
        }
        else
        {
            Debug.Log("No entries found for the user.");
            rankUserTextEndlessStatic.text = "-";
            scoreUserTextEndlessStatic.text = "No time yet.";
        }
    }

    private static void GetUserPreviousRecord()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogWarning("Steam is not initialized.");
            return;
        }

        if (!SteamLeaderboardManager.s_initialized)
        {
            Debug.LogWarning("Leaderboard not initialized. Initializing now...");
            SteamLeaderboardManager.Init();
            return;
        }

        // Request leaderboard entries for the current user
        SteamAPICall_t hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(SteamLeaderboardManager.s_currentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -1, -1);
        m_previousRecordDownloadedResult.Set(hSteamAPICall, OnUserPreviousRecordDownloaded);
    }
        private static void GetUserPreviousRecordEndless()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogWarning("Steam is not initialized.");
            return;
        }

        if (!SteamLeaderboardManager.s_initialized_Endless)
        {
            Debug.LogWarning("Leaderboard endless not initialized. Initializing now...");
            SteamLeaderboardManager.InitEndless();
            return;
        }

        // Request leaderboard entries for the current user
        SteamAPICall_t hSteamAPICall = SteamUserStats.DownloadLeaderboardEntries(SteamLeaderboardManager.s_currentLeaderboard_Endless, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -1, -1);
        m_previousRecordDownloadedResultEndless.Set(hSteamAPICall, OnUserPreviousRecordDownloadedEndless);
    }


    public static void GetScores()
    {
        if (!SteamLeaderboardManager.s_initialized)
        {
            Debug.LogWarning("Can't fetch leaderboard because it isn't loaded yet");
            return;
        }

        SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(SteamLeaderboardManager.s_currentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 100);
        m_scoresDownloadedResult.Set(handle, OnLeaderboardScoresDownloaded);
    }
    public static void GetScoresEndless()
    {
        if (!SteamLeaderboardManager.s_initialized_Endless)
        {
            Debug.LogWarning("Can't fetch leaderboard because it isn't loaded yet");
            return;
        }

        SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(SteamLeaderboardManager.s_currentLeaderboard_Endless, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 100);
        m_scoresDownloadedResultEndless.Set(handle, OnLeaderboardScoresDownloadedEndless);
    }

    private static void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
    {
        if (bIOFailure)
        {
            Debug.LogError("Failed to download leaderboard scores.");
            return;
        }

        SteamLeaderboardDisplay instance = FindObjectOfType<SteamLeaderboardDisplay>();
        instance.ProcessDownloadedScores(pCallback);
    }
    private static void OnLeaderboardScoresDownloadedEndless(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
    {
        if (bIOFailure)
        {
            Debug.LogError("Failed to download endless leaderboard scores.");
            return;
        }

        SteamLeaderboardDisplay instance = FindObjectOfType<SteamLeaderboardDisplay>();
        instance.ProcessDownloadedScoresEndless(pCallback);
    }
    private void ProcessDownloadedScoresEndless(LeaderboardScoresDownloaded_t pCallback) {
        info.gameObject.SetActive(false);

        // Clear previous instantiated prefabs
        foreach (Transform child in scoreContainerEndless)
        {
            Destroy(child.gameObject);
        }

        int numEntries = pCallback.m_cEntryCount;
        m_SteamLeaderboardEntries_Endless = pCallback.m_hSteamLeaderboardEntries;

        int rank = 1;

        for (int index = 0; index < numEntries; index++)
        {
            SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, index, out LeaderboardEntry_t leaderboardEntry, null, 0);
            string username = SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser);

            // Handle rare unknown username issue and reset leaderboard
            if (username.ToUpper() == "[UNKNOWN]")
            {
                Debug.LogWarning("Encountered unknown user. Resetting leaderboard...");
                info.gameObject.SetActive(true);
                info.text = "LOADING SCORES...";
                SteamLeaderboardManager.Init();
                return;
            }

            // Instantiate a new ResultPrefab for each entry
            GameObject newResult = Instantiate(resultPrefab, scoreContainerEndless);
            newResult.transform.localScale = Vector3.one;

            // Update the prefab's text fields
            TMP_Text rankText = newResult.transform.Find("PlaceHolder").GetComponent<TMP_Text>();
            TMP_Text playerNameText = newResult.transform.Find("PlayerName").GetComponent<TMP_Text>();
            TMP_Text scoreText = newResult.transform.Find("TimeHolder").GetComponent<TMP_Text>();
            Image avatarImage = newResult.transform.Find("IconHolder").GetComponent<Image>();

            rankText.text = "#" + rank.ToString();
            playerNameText.text = username;
            scoreText.text = leaderboardEntry.m_nScore.ToString();

            // Retrieve and set the player's Steam avatar
            int avatarInt = SteamFriends.GetMediumFriendAvatar(leaderboardEntry.m_steamIDUser);
            if (avatarInt != -1)
            {
                LoadSteamAvatar(avatarInt, avatarImage);
            }

            rank++;
        }

        // Update the "info" text field with additional information
        info.text += "\n\nPRESS ANY KEY TO RETURN";
    }
    private void ProcessDownloadedScores(LeaderboardScoresDownloaded_t pCallback)
    {
        info.gameObject.SetActive(false);

        // Clear previous instantiated prefabs
        foreach (Transform child in scoreContainer)
        {
            Destroy(child.gameObject);
        }

        int numEntries = pCallback.m_cEntryCount;
        m_SteamLeaderboardEntries = pCallback.m_hSteamLeaderboardEntries;

        int rank = 1;

        for (int index = 0; index < numEntries; index++)
        {
            SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, index, out LeaderboardEntry_t leaderboardEntry, null, 0);
            string username = SteamFriends.GetFriendPersonaName(leaderboardEntry.m_steamIDUser);

            // Handle rare unknown username issue and reset leaderboard
            if (username.ToUpper() == "[UNKNOWN]")
            {
                Debug.LogWarning("Encountered unknown user. Resetting leaderboard...");
                info.gameObject.SetActive(true);
                info.text = "LOADING SCORES...";
                SteamLeaderboardManager.Init();
                return;
            }

            // Instantiate a new ResultPrefab for each entry
            GameObject newResult = Instantiate(resultPrefab, scoreContainer);
            newResult.transform.localScale = Vector3.one;

            // Update the prefab's text fields
            TMP_Text rankText = newResult.transform.Find("PlaceHolder").GetComponent<TMP_Text>();
            TMP_Text playerNameText = newResult.transform.Find("PlayerName").GetComponent<TMP_Text>();
            TMP_Text scoreText = newResult.transform.Find("TimeHolder").GetComponent<TMP_Text>();
            Image avatarImage = newResult.transform.Find("IconHolder").GetComponent<Image>();

            rankText.text = "#" + rank.ToString();
            playerNameText.text = username;
            scoreText.text = FormatTimeFromMilliseconds(leaderboardEntry.m_nScore);

            // Retrieve and set the player's Steam avatar
            int avatarInt = SteamFriends.GetMediumFriendAvatar(leaderboardEntry.m_steamIDUser);
            if (avatarInt != -1)
            {
                LoadSteamAvatar(avatarInt, avatarImage);
            }

            rank++;
        }

        // Update the "info" text field with additional information
        info.text += "\n\nPRESS ANY KEY TO RETURN";
    }

    private static void OnUserPreviousRecordDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
    {
        if (bIOFailure)
        {
            Debug.LogError("Failed to download previous record scores.");
            userPreviousRecord = -50;
            return;
        }

        if (pCallback.m_cEntryCount > 0)
        {
            LeaderboardEntry_t entry;
            SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, 0, out entry, null, 0);
            userPreviousRecord = entry.m_nScore;
        }
        else
        {
            userPreviousRecord = -404; // no previous score
        }

        SteamLeaderboardDisplay instance = FindObjectOfType<SteamLeaderboardDisplay>();
        instance.DisplayUserPreviousRecord();
    }
        private static void OnUserPreviousRecordDownloadedEndless(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
    {
        if (bIOFailure)
        {
            Debug.LogError("Failed to download previous record scores.");
            userPreviousRecordEndless = -50;
            return;
        }

        if (pCallback.m_cEntryCount > 0)
        {
            LeaderboardEntry_t entry;
            SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, 0, out entry, null, 0);
            userPreviousRecordEndless = entry.m_nScore;
        }
        else
        {
            userPreviousRecordEndless = -404; // no previous score
        }

        SteamLeaderboardDisplay instance = FindObjectOfType<SteamLeaderboardDisplay>();
        instance.DisplayUserPreviousRecordEndless();
    }

    private void DisplayUserPreviousRecord()
    {
        if (info)
        {
            info.text += "\n" + userPreviousRecord;
        }
    }
     private void DisplayUserPreviousRecordEndless()
    {
    }

    public static string FormatTimeFromMilliseconds(long milliseconds)
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

    private void LoadSteamAvatar(int avatarInt, Image avatarImage)
    {
        uint width, height;
        if (SteamUtils.GetImageSize(avatarInt, out width, out height))
        {
            byte[] imageData = new byte[width * height * 4]; // RGBA format
            SteamUtils.GetImageRGBA(avatarInt, imageData, (int)(width * height * 4));

            // Create a new Texture2D to hold the image
            Texture2D avatarTexture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
            avatarTexture.LoadRawTextureData(imageData);
            avatarTexture.Apply();

            // Convert the Texture2D to a Sprite and assign it to the avatar Image component
            Sprite avatarSprite = Sprite.Create(avatarTexture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
            avatarImage.sprite = avatarSprite;
        }
    }

    public GameObject endlessLeaderboard;
    public GameObject speedRunLeaderboard;
    public void ViewEndless(){
        endlessLeaderboard.SetActive(true);
        speedRunLeaderboard.SetActive(false);
    }
    public void ViewSpeedrun(){
        endlessLeaderboard.SetActive(false);
        speedRunLeaderboard.SetActive(true);
    }

    private void OnDisable()
    {
        m_scoresDownloadedResult.Dispose();
        m_userPlacementDownloadedResult.Dispose();
        m_previousRecordDownloadedResult.Dispose();
         m_scoresDownloadedResultEndless.Dispose();
        m_userPlacementDownloadedResultEndless.Dispose();
        m_previousRecordDownloadedResultEndless.Dispose();
    }
}
