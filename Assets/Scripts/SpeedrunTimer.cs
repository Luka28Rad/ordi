using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpeedrunTimer : MonoBehaviour
{
    public TextMeshProUGUI resultsText;
    public Button saveButton;
    public Button mainMenuButton;
    public Button tryAgainButton;
    private static string fileName = "records.txt";
    float currentTime;
    public GameObject nameInputPanel;

    public static TextMeshProUGUI recordTimeStatic;
    public TextMeshProUGUI recordTime;
    public static TextMeshProUGUI currentTimeTextStatic;

    public TextMeshProUGUI currentTimeText;
    static bool timerActive = false;

    // Start is called before the first frame update
    void Start()
    {
        if(Variables.gameMode != "Speedrun") {
            currentTimeText.gameObject.SetActive(false);
            enabled = false;
        } else {
            if(SceneManager.GetActiveScene().name == "Level 1") PlayerPrefs.SetFloat("SpeedTime", 0);
            currentTimeText.gameObject.SetActive(true);
            StartTimer();
            currentTime = PlayerPrefs.GetFloat("SpeedTime", 0);
        }

    }

public static long ConvertToMilliseconds(string timeString)
{
    if (TimeSpan.TryParseExact(timeString, @"hh\:mm\:ss\.fff", null, out TimeSpan time))
    {
        return (long)time.TotalMilliseconds;
    }
    else
    {
        Debug.LogError("Invalid time format. Ensure it is in 'hh:mm:ss.fff' format.");
        return -1; // Indicate an error with a negative value
    }
}

    void Awake() {
        recordTimeStatic = recordTime;
        currentTimeTextStatic = currentTimeText;
        prevRecordStatic = prevRecord;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Variables.paused) {
        if(timerActive) {
            currentTime += Time.deltaTime;
            PlayerPrefs.SetFloat("SpeedTime", currentTime);
        } else {
            nameInputPanel.SetActive(true);
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeTextStatic.text = time.ToString(@"hh\:mm\:ss\.fff");
        } 
    }

    public void StartTimer() {
        timerActive = true;
    }

    public static void StopTimer(){
        timerActive = false;
    }

    public TextMeshPro errText;
    public void SaveRecordClicked()
    {
        string filePath = Path.Combine(Application.dataPath,fileName);
        TMP_InputField nameInputField = nameInputPanel.GetComponentInChildren<TMP_InputField>();
        string name = nameInputField.text;
        if(name == "") name = "Unknown";
        int collectC = 0;
        string collectiblesString = PlayerPrefs.GetString("collectiblesSpeedRun", "0");
        if (!string.IsNullOrEmpty(collectiblesString))
        {
        string[] collectiblesArray = collectiblesString.Split(','); 
        collectC = collectiblesArray.Length;
        }
        long rec = ConvertToMilliseconds(currentTimeText.text);
        resultsText.gameObject.SetActive(true);
        try{
            if(rec<=0) {
                 resultsText.text = "Why are you cheating? :(";
            } else if(rec<=180000){
                resultsText.text = "Why are you cheating? :(";
            } else if(rec<=360000){
                resultsText.text = "A little suspicious but i will allow it!!! :(";
                SteamLeaderboardManager.UpdateScore((int)rec);
            } else {
            SteamLeaderboardManager.UpdateScore((int)rec);
            }
        } catch (System.Exception e) {
            resultsText.text = "Saving score failed, please try again. " + e.Message;
            return;
        }
        compareRec = rec;
    
        var achievements = new List<System.Action> {
                    Achievements.UnlockBeatZirAchievement,
                    Achievements.UnlockBeatZvjezdanAchievement,
                    Achievements.UnlockBeatGljivanAchievement,
                    Achievements.UnlockBeatDuskoAchievement,
                    Achievements.UnlockBeatSvjetlanaAchievement,
                    Achievements.UnlockBeatBrunoAchievement,
                    Achievements.UnlockBeatNestaskoAchievement,
                    Achievements.UnlockBeatDarkoAchievement
                };

                int threshold = rec switch {
                    <= 480000 => 8,
                    <= 600000 => 7,
                    <= 720000 => 6,
                    <= 900000 => 5,
                    <= 1080000 => 4,
                    <= 1200000 => 3,
                    <= 1500000 => 2,
                    <= 1800000 => 1,
                    _ => 0
                };

                for (int i = 0; i < threshold; i++) {
                    achievements[i].Invoke();
                }
        //resultsText.text = "Press continue to save results. (*Only faster time will be saved.)";
        string text = "\n" +name+ " "+ currentTimeText.text + " " + collectC;
        try
        {        
            saveButton.gameObject.SetActive(false);
            tryAgainButton.gameObject.SetActive(true);
            mainMenuButton.gameObject.SetActive(true);
            nameInputField.gameObject.SetActive(false);
            resultsText.gameObject.SetActive(true);
            //File.AppendAllText(filePath, text);
            Debug.Log("Text appended to file: " + filePath);
            Debug.Log("Record added: " + text);
        }
        catch (Exception e)
        {
            resultsText.text = "Error storing results...";
            Debug.LogError("Error writing to file: " + e.Message);
        }
    }
    private static long compareRec = 999999999999999999;
    public TextMeshProUGUI prevRecord;
    public static TextMeshProUGUI prevRecordStatic;

    public static void SaveTime()
    {
        long checkRecord = SteamLeaderboardDisplay.userPreviousRecord;
        if(checkRecord == -404) {
            prevRecordStatic.text = "No previous time!";
        } else if(checkRecord < 0){
            prevRecordStatic.text = "Error loading previous time..";
        } else {
            if(compareRec < checkRecord) Achievements.UnlockBeatYourselfAchievement();
            prevRecordStatic.text = "Your last best time:\n"+SteamLeaderboardDisplay.FormatTimeFromMilliseconds(checkRecord);
        }
        GameObject playerObject = GameObject.FindWithTag("Player");
        Destroy(playerObject);
        recordTimeStatic.text = "Current time:\n" +currentTimeTextStatic.text;
        StopTimer();
    }
}
