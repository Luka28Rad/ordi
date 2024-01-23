using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Collectibles : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI collectiblesUI;
    private int totalCollectibles;
    public static int collectibleCounter = 0;
    void Start()
    {
        totalCollectibles = transform.childCount;
        if(Variables.gameMode == "LoadGame") {
            string collectiblesString = PlayerPrefs.GetString("collectibles");
        if (!string.IsNullOrEmpty(collectiblesString))
        {
            string[] collectiblesArray = collectiblesString.Split(',');

            foreach (string collectibleName in collectiblesArray)
            {
                if (!string.IsNullOrEmpty(collectibleName))
                {
                    GameObject collectibleObject = GameObject.Find(collectibleName);
                    if (collectibleObject != null)
                    {
                        collectibleCounter++;
                        Debug.Log("Destroyed " + collectibleObject.name);
                        Destroy(collectibleObject);
                    }
                }
            }
        }
         collectiblesUI.text = collectibleCounter + "/" + totalCollectibles;
        } else {
            Debug.Log("Bok");
            string collectiblesString = PlayerPrefs.GetString("collectiblesSpeedRun");
            if (!string.IsNullOrEmpty(collectiblesString))
            {
                string[] collectiblesArray = collectiblesString.Split(',');
                collectibleCounter = collectiblesArray.Length;
            } else {
                collectibleCounter = 0;
            }

            collectiblesUI.text = collectibleCounter + "/" + totalCollectibles;
        }
       
    }

    void Update()
    {
        collectiblesUI.text = (collectibleCounter) + "/" + totalCollectibles;
    }
}
