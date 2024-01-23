using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    private string checkpointName;
    private int numberOfLives;
    private int numberOfCollectibles;
    private int lightIntensity;
    void Start()
    {
        if(Variables.gameMode == "LoadGame") 
        {
        checkpointName = PlayerPrefs.GetString("Checkpoint", "Start");
        numberOfCollectibles = PlayerPrefs.GetInt("Collectibles", 0);

        Debug.Log("Checkpoint: " + checkpointName);
        Debug.Log("Collectibles: " + numberOfCollectibles);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(checkpointName != "Start") player.transform.position = GameObject.Find(checkpointName).transform.position;
        } else {

        }
    }

    // Update is called once per frame
    void Update()
    {
        // Your update logic here
    }
}
