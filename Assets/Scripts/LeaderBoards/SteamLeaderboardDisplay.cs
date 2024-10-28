using Steamworks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SteamLeaderboardDisplay : MonoBehaviour
{
    public GameObject resultPrefab; // Reference to the ResultPrefab
    public Transform scoreContainer; // Parent object to organize instantiated prefabs
    public TMP_Text info;

    [HideInInspector]
    public SteamLeaderboardEntries_t m_SteamLeaderboardEntries;
    private static readonly CallResult<LeaderboardScoresDownloaded_t> m_scoresDownloadedResult = new CallResult<LeaderboardScoresDownloaded_t>();

    void Start(){
        // SteamLeaderboardManager.UpdateScore(5000); // This will upload the score for the currently logged-in user.
    }

    private void OnEnable()
    {
        if (info)
        {
            info.gameObject.SetActive(true);
            info.text = "LOADING SCORES...";
        }
        GetScores();
    }

    public static void GetScores()
    {
        if (!SteamLeaderboardManager.s_initialized)
        {
            Debug.Log("Can't fetch leaderboard because it isn't loaded yet");
        }
        else
        {
            SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(SteamLeaderboardManager.s_currentLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 10);
            m_scoresDownloadedResult.Set(handle, OnLeaderboardScoresDownloaded);
        }
    }

    private static void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
    {
        SteamLeaderboardDisplay instance = FindObjectOfType<SteamLeaderboardDisplay>();
        instance.ProcessDownloadedScores(pCallback);
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
                if (info)
                {
                    info.gameObject.SetActive(true);
                    info.text = "LOADING SCORES...";
                }

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

public static string FormatTimeFromMilliseconds(long milliseconds)
{
    TimeSpan time = TimeSpan.FromMilliseconds(milliseconds);

    if (time.Hours > 0)
    {
        // If there are hours, include hours in the format
        return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
                             time.Hours, 
                             time.Minutes, 
                             time.Seconds, 
                             time.Milliseconds);
    }
    else
    {
        // If there are no hours, exclude hours from the format
        return string.Format("{0:D2}:{1:D2}.{2:D3}",
                             time.Minutes, 
                             time.Seconds, 
                             time.Milliseconds);
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
            avatarImage.sprite = Sprite.Create(avatarTexture, new Rect(0, 0, avatarTexture.width, avatarTexture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
