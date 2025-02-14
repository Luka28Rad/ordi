using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class NextLevel : MonoBehaviour
{

    private bool isScaling = false;
    public Animator transition;
    public AudioSource telSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isScaling)
        {
            isScaling = true;
            if(telSound != null) telSound.Play();
            StartCoroutine(ScalePlayerAndLoadNextLevel(other.gameObject));
        }
    }

    private IEnumerator ScalePlayerAndLoadNextLevel(GameObject player)
    {
        float i = 0;
        float rate = 1 / 2f;
    
        Vector3 fromScale = player.transform.localScale;
        Vector3 toScale = new Vector3(0.1f,0.1f,0.1f);
        Vector3 initialPosition = player.transform.position;

        while (i < 1)
        {
            i += Time.deltaTime * rate;
            
            player.transform.localScale = Vector3.Lerp(fromScale, toScale, i);
            player.transform.position = Vector3.Lerp(initialPosition, transform.position, i);

            yield return 0;
        }
        player.SetActive(false);
        GameObject.Find("dash indicator").gameObject.SetActive(false);
        LoadNextLevel();
    }
    bool statStarted = false;
    private void LoadNextLevel()
    {
        Achievements.UnlockUseTeleportAchievement();
        string levelName = SceneManager.GetActiveScene().name;
        if(Variables.gameMode == "LoadGame" || Variables.gameMode == "NewGame"){
            PlayerPrefs.SetString("Checkpoint", "StartCheckpoint");
        }
            switch (levelName)
            {
                case "Level 1":
                if(Variables.gameMode == "LoadGame" || Variables.gameMode == "NewGame"){
                    if(Variables.healthCount == 5) Achievements.UnlockFullLifeLvl1Achievement();
                    Achievements.UnlockFinishLvl1Achievement();
                    PlayerPrefs.SetString("Level", "Level 2");
                }
                    StartCoroutine(LoadNewLevel("Level 2"));
                    break;
                case "Level 2":
                if(Variables.gameMode == "LoadGame" || Variables.gameMode == "NewGame"){
                    if(Variables.healthCount == 5) Achievements.UnlockFullLifeLvl2Achievement();
                    Achievements.UnlockFinishLvl2Achievement();
                    PlayerPrefs.SetString("Level", "Level 3");
                }
                    StartCoroutine(LoadNewLevel("Level 3"));
                    break;
                case "Level 3":
                if(Variables.gameMode == "LoadGame" || Variables.gameMode == "NewGame"){
                    if(Variables.healthCount == 5) Achievements.UnlockFullLifeLvl3Achievement();
                    Achievements.UnlockFinishLvl3Achievement();
                    PlayerPrefs.SetString("Level", "Bossfight");
                }
                    StartCoroutine(LoadNewLevel("Bossfight"));
                    break;
                case "Bossfight": 
                if(Variables.gameMode == "LoadGame" || Variables.gameMode == "NewGame"){
                    if(Variables.healthCount == 5) Achievements.UnlockFullLifeBfAchievement();
                    if(!statStarted) {
                        Achievements.UnlockGameFinishAchievement();
                        statStarted = true;
                    }
                    PlayerPrefs.SetString("Checkpoint", "StartCheckpoint");
                    PlayerPrefs.SetString("collectibles", "");
                    PlayerPrefs.SetString("Level", "Level 1");
                    PlayerPrefs.SetString("StartedGame", "FALSE");
                } else if(Variables.gameMode == "Practice") {
                    Achievements.UnlockPracticeFinishAchievement();
                }
                if(Variables.gameMode == "Speedrun") {
                   // Achievements.UnlockFinishSpeedrunAchievement();
                    if(!statStarted) {
                        Achievements.UnlockFinishSpeedrunAchievement();
                        statStarted = true;
                    }
                    SpeedrunTimer.SaveTime();
                } else {
                    StartCoroutine(LoadNewLevel("EndScene"));
                }
                    break;
                case "DemoLevel":
                    Achievements.UnlockDemoAchievement();
                    //if(Variables.healthCount == 5) Achievements.UnlockFullLifeDemoAchievement();
                    StartCoroutine(LoadNewLevel("DemoEndScene"));
                    break;
                default:
                    Debug.LogWarning("Error");
                    Debug.Log("MOzda");
                    StartCoroutine(LoadNewLevel("MainMenuScene"));
                    break;
            }

    }

    IEnumerator LoadNewLevel(string sceneName) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        if(telSound != null) telSound.Stop();
        SceneManager.LoadScene(sceneName);
    }


}
