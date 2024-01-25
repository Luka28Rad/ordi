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

    void Awake() {
        recordTimeStatic = recordTime;
        currentTimeTextStatic = currentTimeText;
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
        } else {
            
        }
    }

    public void StartTimer() {
        timerActive = true;
    }

    public static void StopTimer(){
        timerActive = false;
    }


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
        
        string text = "\n" +name+ " "+ currentTimeText.text + " " + collectC;
        try
        {        
            saveButton.gameObject.SetActive(false);
            tryAgainButton.gameObject.SetActive(true);
            mainMenuButton.gameObject.SetActive(true);
            nameInputField.gameObject.SetActive(false);
            resultsText.gameObject.SetActive(true);
            File.AppendAllText(filePath, text);
            Debug.Log("Text appended to file: " + filePath);
            Debug.Log("Record added: " + text);
        }
        catch (Exception e)
        {
            resultsText.text = "Error storing results...";
            Debug.LogError("Error writing to file: " + e.Message);
        }
    }

    public static void SaveTime()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        Destroy(playerObject);
        recordTimeStatic.text = "Time: " +currentTimeTextStatic.text;
        StopTimer();
    }
}
