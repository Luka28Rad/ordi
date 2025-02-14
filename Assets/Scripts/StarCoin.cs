using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StarCoin : MonoBehaviour
{
    public float rotationSpeed = 150f;  
    public float movementRange = 0.35f; 
    public float movementSpeed = 4f;
    private AudioSource audioSource;
    private float initialY;  

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        initialY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        float verticalMovement = Mathf.Sin(Time.time * movementSpeed) * movementRange;
        transform.position = new Vector3(transform.position.x, initialY + verticalMovement, transform.position.z);
    }

        private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Achievements.UnlockFirstCoinAchievement();
            string collectibleName = gameObject.name;
            if(collectibleName.ToLower().Contains("demo")) {
                if (!audioSource.isPlaying)
                    {
                        Collectibles.demoCounter++;
                        GetComponent<BoxCollider2D>().enabled = false;
                        audioSource.Play();
                        Color currentColor = GetComponent<Renderer>().material.color;
                        currentColor.a = 0f;
                        GetComponent<Renderer>().material.color =currentColor;
                        Destroy(gameObject, audioSource.clip.length);
                    }
                    return;
            }
            if(Variables.gameMode == "Speedrun" || Variables.gameMode == "Practice") {
                if(Variables.gameMode == "Speedrun") SteamStatsManager.Instance.IncrementStat("CoinsSR");
                if(Variables.gameMode == "Practice") SteamStatsManager.Instance.IncrementStat("CoinsPR");
                Variables.speedRunCollectiblesCounter++;
                Collectibles.collectibleCounter++;

            if (PlayerPrefs.HasKey("collectiblesSpeedRun"))
            {
                string currentCollectibles = PlayerPrefs.GetString("collectiblesSpeedRun");
                    if(currentCollectibles != "") currentCollectibles += "," + collectibleName;
                    else currentCollectibles = collectibleName;
                    PlayerPrefs.SetString("collectiblesSpeedRun", currentCollectibles);
            }
            else
            {
                PlayerPrefs.SetString("collectiblesSpeedRun", collectibleName);
            }
                    HasAllCoins("Speedrun");

                if (!audioSource.isPlaying)
                    {
                        GetComponent<BoxCollider2D>().enabled = false;
                        audioSource.Play();
                        Color currentColor = GetComponent<Renderer>().material.color;
                        currentColor.a = 0f;
                        GetComponent<Renderer>().material.color =currentColor;
                        Destroy(gameObject, audioSource.clip.length);
                    }
                Debug.Log("Collected " + gameObject.name);
            } else if(Variables.gameMode == "NewGame" || Variables.gameMode == "LoadGame") {
                SteamStatsManager.Instance.IncrementStat("CoinsSP");
            if (PlayerPrefs.HasKey("collectibles"))
            {
                string currentCollectibles = PlayerPrefs.GetString("collectibles");
                    currentCollectibles += "," + collectibleName;
                    PlayerPrefs.SetString("collectibles", currentCollectibles);
            }
            else
            {
                PlayerPrefs.SetString("collectibles", collectibleName);
            }
                    HasAllCoins("NewGame");
            Collectibles.collectibleCounter++;
                if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                        GetComponent<BoxCollider2D>().enabled = false;
                        Color currentColor = GetComponent<Renderer>().material.color;
                        currentColor.a = 0f;
                        GetComponent<Renderer>().material.color =currentColor;
                        Destroy(gameObject, audioSource.clip.length);
                    }
            Debug.Log("Collected " + gameObject.name);
            } else {
                                if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                        GetComponent<BoxCollider2D>().enabled = false;
                        Color currentColor = GetComponent<Renderer>().material.color;
                        currentColor.a = 0f;
                        GetComponent<Renderer>().material.color =currentColor;
                        Destroy(gameObject, audioSource.clip.length);
                    }
                Debug.Log("Just the sound.");
            }
        }
    }

    private void HasAllCoins(string mode){
        if(mode == "Speedrun" || mode == "Practice") {
            string currentCollectibles = PlayerPrefs.GetString("collectiblesSpeedRun");
            string[] collectiblesArray = currentCollectibles.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            string[] allItems = new string[] {"StarCoin1.1", "StarCoin1.2", "StarCoin1.3", "StarCoin1.4", "StarCoin1.5", "StarCoin2.1", "StarCoin2.2", "StarCoin2.3", "StarCoin2.4", "StarCoin2.5", "StarCoin2.6", "StarCoin3.1", "StarCoin3.2", "StarCoin3.3", "StarCoin3.4", "StarCoin3.5", "StarCoin3.6", "StarCoin3.7", "StarCoinB.1" };
            string[] allItemsLvl1 = new string[] {"StarCoin1.1", "StarCoin1.2", "StarCoin1.3", "StarCoin1.4", "StarCoin1.5"};
            string[] allItemsLvl2 = new string[] {"StarCoin2.1", "StarCoin2.2", "StarCoin2.3", "StarCoin2.4", "StarCoin2.5", "StarCoin2.6"};
            string[] allItemsLvl3 = new string[] {"StarCoin3.1", "StarCoin3.2", "StarCoin3.3", "StarCoin3.4", "StarCoin3.5", "StarCoin3.6", "StarCoin3.7"};
            string[] allItemsBf = new string[] {"StarCoinB.1"};
            bool allItemsContained = allItems.All(item => collectiblesArray.Contains(item));
            bool allItemsContainedLvl1 = allItemsLvl1.All(item => collectiblesArray.Contains(item));
            bool allItemsContainedLvl2 = allItemsLvl2.All(item => collectiblesArray.Contains(item));
            bool allItemsContainedLvl3 = allItemsLvl3.All(item => collectiblesArray.Contains(item));
            bool allItemsContainedBf = allItemsBf.All(item => collectiblesArray.Contains(item));
            if (allItemsContained)
            {
                Achievements.UnlockAllCoinsAchievement();
                if(mode == "Speedrun") SteamStatsManager.Instance.IncrementStat("100SR");
                Debug.Log("All items are collected: " + string.Join(", ", allItems));
            }
            else
            {
                Debug.Log("Not all items are collected.");
            }
            if (allItemsContainedLvl1)
            {
                Achievements.UnlockAllCoinsLvl1Achievement();
                Debug.Log("All items LVL1 are collected: " + string.Join(", ", allItemsLvl1));
            }
            else
            {
                Debug.Log("Not all items on LVL 1 are collected.");
            }
             if (allItemsContainedLvl2)
            {
                Achievements.UnlockAllCoinsLvl2Achievement();
                Debug.Log("All items LVL2 are collected: " + string.Join(", ", allItemsLvl2));
            }
            else
            {
                Debug.Log("Not all items on LVL 2 are collected.");
            }
             if (allItemsContainedLvl3)
            {
                Achievements.UnlockAllCoinsLvl3Achievement();
                Debug.Log("All items LVL3 are collected: " + string.Join(", ", allItemsLvl3));
            }
            else
            {
                Debug.Log("Not all items on LVL 3 are collected.");
            }
             if (allItemsContainedBf)
            {
                Achievements.UnlockAllCoinsBfAchievement();
                Debug.Log("All items BF are collected: " + string.Join(", ", allItemsBf));
            }
            else
            {
                Debug.Log("Not all items on BF are collected.");
            }
        } else if (mode == "NewGame" || mode == "LoadGame"){
            string currentCollectibles = PlayerPrefs.GetString("collectibles");
            string[] collectiblesArray = currentCollectibles.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            string[] allItems = new string[] {"StarCoin1.1", "StarCoin1.2", "StarCoin1.3", "StarCoin1.4", "StarCoin1.5", "StarCoin2.1", "StarCoin2.2", "StarCoin2.3", "StarCoin2.4", "StarCoin2.5", "StarCoin2.6", "StarCoin3.1", "StarCoin3.2", "StarCoin3.3", "StarCoin3.4", "StarCoin3.5", "StarCoin3.6", "StarCoin3.7", "StarCoinB.1" };
            string[] allItemsLvl1 = new string[] {"StarCoin1.1", "StarCoin1.2", "StarCoin1.3", "StarCoin1.4", "StarCoin1.5"};
            string[] allItemsLvl2 = new string[] {"StarCoin2.1", "StarCoin2.2", "StarCoin2.3", "StarCoin2.4", "StarCoin2.5", "StarCoin2.6"};
            string[] allItemsLvl3 = new string[] {"StarCoin3.1", "StarCoin3.2", "StarCoin3.3", "StarCoin3.4", "StarCoin3.5", "StarCoin3.6", "StarCoin3.7"};
            string[] allItemsBf = new string[] {"StarCoinB.1"};
            bool allItemsContained = allItems.All(item => collectiblesArray.Contains(item));
            bool allItemsContainedLvl1 = allItemsLvl1.All(item => collectiblesArray.Contains(item));
            bool allItemsContainedLvl2 = allItemsLvl2.All(item => collectiblesArray.Contains(item));
            bool allItemsContainedLvl3 = allItemsLvl3.All(item => collectiblesArray.Contains(item));
            bool allItemsContainedBf = allItemsBf.All(item => collectiblesArray.Contains(item));
            if (allItemsContained)
            {
                Achievements.UnlockAllCoinsAchievement();
                SteamStatsManager.Instance.IncrementStat("100SP");
                Debug.Log("All items are collected: " + string.Join(", ", allItems));
            }
            else
            {
                Debug.Log("Not all items are collected.");
            }
            if (allItemsContainedLvl1)
            {
                Achievements.UnlockAllCoinsLvl1Achievement();
                Debug.Log("All items LVL1 are collected: " + string.Join(", ", allItemsLvl1));
            }
            else
            {
                Debug.Log("Not all items on LVL 1 are collected.");
            }
             if (allItemsContainedLvl2)
            {
                Achievements.UnlockAllCoinsLvl2Achievement();
                Debug.Log("All items LVL2 are collected: " + string.Join(", ", allItemsLvl2));
            }
            else
            {
                Debug.Log("Not all items on LVL 2 are collected.");
            }
             if (allItemsContainedLvl3)
            {
                Achievements.UnlockAllCoinsLvl3Achievement();
                Debug.Log("All items LVL3 are collected: " + string.Join(", ", allItemsLvl3));
            }
            else
            {
                Debug.Log("Not all items on LVL 3 are collected.");
            }
             if (allItemsContainedBf)
            {
                Achievements.UnlockAllCoinsBfAchievement();
                Debug.Log("All items BF are collected: " + string.Join(", ", allItemsBf));
            }
            else
            {
                Debug.Log("Not all items on BF are collected.");
            }
        } 
    }
}
