using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using TMPro;
public class ResourcesFile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "records.txt");
                if (!File.Exists(filePath))
        {
            CreateRecordsFile(filePath);
        }
    }

            private void CreateRecordsFile(string filePath)
    {
        try
        {
            // Create a new file at the specified path
            File.Create(filePath);

            Debug.Log("records.txt created successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error creating records.txt: {e.Message}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
