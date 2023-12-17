using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.IO;

public class SpeedrunTimer : MonoBehaviour
{
    private static string fileName = "records.txt";
    float currentTime;

    public static TextMeshProUGUI currentTimeTextStatic;

    public TextMeshProUGUI currentTimeText;
    bool timerActive = false;

    // Start is called before the first frame update
    void Start()
    {
        if(Variables.gameMode != "Speedrun") {
            currentTimeText.gameObject.SetActive(false);
            enabled = false;
        } else {
            currentTimeText.gameObject.SetActive(true);
            StartTimer();
            currentTime = 0;
        }

    }

    void Awake() {

        currentTimeTextStatic = currentTimeText;
    }

    // Update is called once per frame
    void Update()
    {
        if(timerActive) {
            currentTime += Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        currentTimeTextStatic.text = time.ToString(@"hh\:mm\:ss\.fff");
    }

    public void StartTimer() {
        timerActive = true;
    }

    public void StopTimer(){
        timerActive = false;
    }

    public static void SaveTime()
    {
        string filePath = Path.Combine(Application.dataPath, "Resources/" + fileName);
        string text = "\nTestName " + currentTimeTextStatic.text;

        try
        {
            File.AppendAllText(filePath, text);
            Debug.Log("Text appended to file: " + filePath);
            Debug.Log("Record added: " + text);
        }
        catch (Exception e)
        {
            Debug.LogError("Error writing to file: " + e.Message);
        }
    }
}
