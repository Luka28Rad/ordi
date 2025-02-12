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
    public static int demoCounter = 0;
    void Start()
    {
        totalCollectibles = transform.childCount;
        if(Variables.gameMode == "LoadGame") {
            collectibleCounter = 0;
            string collectiblesString = PlayerPrefs.GetString("collectibles");
        if (!string.IsNullOrEmpty(collectiblesString))
        {
            string[] collectiblesArray = collectiblesString.Split(',');

            foreach (string collectibleName in collectiblesArray)
            {
                Debug.Log(collectibleName + " aaa");
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
            collectibleCounter = 0;
            collectiblesUI.text = collectibleCounter + "/" + totalCollectibles;
        }
       
    }
    private bool demoSet = false;
    void Update()
    {
        if(Variables.gameMode == "Speedrun") {
            collectiblesUI.text = (collectibleCounter) + "/" + totalCollectibles +"\n1 coin = -2s at the end";
        } else {
            if(Variables.gameMode != "Demo") collectiblesUI.text = (collectibleCounter) + "/" + totalCollectibles;
        }
        if(Variables.gameMode == "Demo"){
            collectiblesUI.text = (demoCounter) + "/" + totalCollectibles;
            if(!demoSet && demoCounter == totalCollectibles){
                demoSet = true;
                Achievements.UnlockAllCoinsDemoAchievement();
            }
        }
    }
}
