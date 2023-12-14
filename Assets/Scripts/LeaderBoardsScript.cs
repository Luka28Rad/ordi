using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using TMPro;

public class LeaderBoardsScript : MonoBehaviour
{
    public GameObject recordPrefab;
    public Transform contentTransform;
    public List<Sprite> spriteList = new List<Sprite>();
    void Start()
    {
        string filePath = "Assets/Resources/records.txt";
        string[] lines = File.ReadAllLines(filePath);
        List<Record> records = new List<Record>();

        foreach (string line in lines)
        {
            string[] parts = line.Split(' ');
            if (parts.Length == 2)
            {
                string name = parts[0];
                string timeStr = parts[1];

                if (TimeSpan.TryParseExact(timeStr, "hh\\:mm\\:ss\\.fff", null, out TimeSpan time))
                {
                    records.Add(new Record(name, time));
                }
                else
                {
                    Debug.LogError("Error parsing time: " + timeStr);
                }
            }
            else
            {
                Debug.LogError("Invalid line format: " + line);
            }
        }

        records.Sort((record1, record2) => record1.Time.CompareTo(record2.Time));

        for(int i = 1; i <= records.Count; i++) {
            Record record = records[i-1];
           // Debug.Log(i + ". " + record.Name + " " + record.Time.ToString("hh\\:mm\\:ss\\.fff"));

            GameObject instance = Instantiate(recordPrefab,contentTransform);

            Transform iconHolderTransform = instance.transform.Find("IconHolder");
            Transform placeHolderTransform = instance.transform.Find("PlaceHolder");
            Transform timeHolderTransform = instance.transform.Find("TimeHolder");
            Transform playerNameHolderTransform = instance.transform.Find("PlayerName");

            TextMeshProUGUI placeHolderText = placeHolderTransform.GetComponent<TextMeshProUGUI>();
            placeHolderText.text = i + "";

            TextMeshProUGUI timeHolderText = timeHolderTransform.GetComponent<TextMeshProUGUI>();
            timeHolderText.text = record.Time.ToString("hh\\:mm\\:ss\\.fff");

            TextMeshProUGUI playerNameText = playerNameHolderTransform.GetComponent<TextMeshProUGUI>();
            playerNameText.text = record.Name;

            Image imageComponent = iconHolderTransform.GetComponent<Image>();
            Sprite randomSprite = spriteList[UnityEngine.Random.Range(0, spriteList.Count)];
            imageComponent.sprite = randomSprite;
        }
    }
    public class Record
    {
        public string Name { get; }
        public TimeSpan Time { get; }

        public Record(string name, TimeSpan time)
        {
            Name = name;
            Time = time;
        }
    }
}

