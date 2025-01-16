using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesController : MonoBehaviour
{
    [SerializeField] GameObject buttonsToShow;
    [SerializeField] GameObject buttonsToHide;
    [SerializeField] GameObject buttonsToShowSingleplayer;
    public Animator transition;
    [SerializeField] GameObject levelButtons;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject title;
    [SerializeField] GameObject settingsButtons;
    [SerializeField] Sprite panelImageNormal;
    [SerializeField] Sprite panelImageLevels;

    void Start() {
        GameObject levelLoader = GameObject.Find("LevelLoader");

    if (levelLoader != null && levelLoader.transform.childCount > 0)
    {
        // Get the first child of the LevelLoader object
        Transform firstChild = levelLoader.transform.GetChild(0);

        // Get the Animator component of the first child
        transition = firstChild.GetComponent<Animator>();

        // Log if the animator is found
        if (transition != null)
        {
            Debug.Log("Animator successfully assigned to 'transition'.");
        }
        else
        {
            Debug.LogWarning("First child of LevelLoader does not have an Animator component.");
        }
    }
    else
    {
        Debug.LogWarning("LevelLoader object not found or has no children.");
    } 
    }
    
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "CreditsScene")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Mozda2");
                StartCoroutine(LoadNewLevel("MainMenuScene"));
            }
        }
    }
    public void LeaderboardButtonClicked()
    {
        StartCoroutine(LoadNewLevel("LeaderboardScene"));
    }

    public void EndlessScene(){
        Variables.gameMode = "Endless";
        SteamStatsManager.Instance.IncrementStat("GamesEN");
        StartCoroutine(LoadNewLevel("EndlessLevel"));
    }

    public void CreditsButtonClicked()
    {
        StartCoroutine(LoadNewLevel("CreditsScene"));
    }

    public void SpeedRunButtonClicked()
    {
        PlayerPrefs.SetFloat("SpeedTime", 0);
        SteamStatsManager.Instance.IncrementStat("GamesSR");
        PlayerPrefs.SetString("collectiblesSpeedRun", "");
        Variables.gameMode = "Speedrun";
        StartCoroutine(LoadNewLevel("Level 1"));
    }

    public void ReplayButtonClicked() {
        switch(Variables.gameMode) {
            case "NewGame":
                Variables.gameMode = "LoadGame";
                LoadGameButtonClicked();
                break;
            case "Speedrun":
            Variables.gameMode = "Speedrun";
                SpeedRunButtonClicked();
                break;
            case "LoadGame":
            Variables.gameMode = "LoadGame";
                LoadGameButtonClicked();
                break;
            case "Practice":
                PracticeLevelClicked(PlayerPrefs.GetInt("PracticeLevel", 1));
                break;
            case "Demo":
                ToDemo();
                break;
            default:
            Debug.Log("Mozda3");
            StartCoroutine(LoadNewLevel("MainMenuScene"));
            break;
        }
    }
    public void NewGameButtonClicked()
    {
        Variables.gameMode = "NewGame";
        SteamStatsManager.Instance.IncrementStat("GamesSP");
        PlayerPrefs.SetString("Checkpoint", "StartCheckpoint");
        PlayerPrefs.SetString("collectibles", "");
        PlayerPrefs.SetString("Level", "Level 1");
        StartCoroutine(LoadNewLevel("Level 1"));
    }

    public void LoadGameButtonClicked()
    {
        Variables.gameMode = "LoadGame";
        string level = PlayerPrefs.GetString("Level", "Level 1");
        StartCoroutine(LoadNewLevel(level));
    }

    public void PracticeButtonClicked()
    {
        if(levelButtons.activeSelf) {
            panel.GetComponent<Image>().sprite = panelImageNormal;
        levelButtons.SetActive(false);
        buttonsToShowSingleplayer.SetActive(true);
        } else {
            panel.GetComponent<Image>().sprite = panelImageLevels;
        levelButtons.SetActive(true);
        buttonsToShow.SetActive(false);
        buttonsToShowSingleplayer.SetActive(false);
        }
    }

    public void PracticeLevelClicked(int level) {
        PlayerPrefs.SetString("collectiblesSpeedRun", "");
        Variables.gameMode = "Practice";
        PlayerPrefs.SetInt("PracticeLevel", level);
        StartCoroutine(LoadNewLevel("Level " + level));
    }

    public void OptionsButtonClicked()
    {
        if(settingsButtons.activeSelf) {
            settingsButtons.SetActive(false);
            buttonsToHide.SetActive(true);
        } else {
            settingsButtons.SetActive(true);
            buttonsToHide.SetActive(false);
        }
    }
    public void MultiplayerButtonClicked(){
        Debug.Log("MultiplayerButtonClicked");
        StartCoroutine(LoadNewLevel("HostLobbyScene"));
    }

    public void PlayButtonClicked()
    {
        if (buttonsToShow.activeSelf)
        {
            title.SetActive(true);
            buttonsToShow.SetActive(false);
            buttonsToHide.SetActive(true);
        }    
        else if(buttonsToShowSingleplayer.activeSelf)
        {
            title.SetActive(false);
            buttonsToShow.SetActive(true);
            buttonsToShowSingleplayer.SetActive(false);
        } else if(buttonsToHide.activeSelf){
            title.SetActive(false);
            buttonsToShow.SetActive(true);
            buttonsToHide.SetActive(false);
        }
            
    }

    public void SinglePlayerButtonClicked()
    {
        title.SetActive(false);
        buttonsToShowSingleplayer.SetActive(true);
        buttonsToShow.SetActive(false);
    }

    public void MainMenuClicked()
    {
        Debug.Log("Mozda3");
        if(SceneManager.GetActiveScene().name == "LeaderboardScene")Achievements.UnlockWatchCreditsAchievement();
        StartCoroutine(LoadNewLevel("MainMenuScene"));
    }

    public void ExitButtonClicked()
    {
        //UnityEditor.EditorApplication.isPlaying = false; // ovu liniju treba maknuti prije buildanja jer ce inace stvarat error - omogucuje da se izade iz editora na klik
        Application.Quit(); // ova logicno izlazi iz aplikacije
    }

    //potrebna funkcija za multiplayer
        public void CallPersistentObjectFunction()
    {
        // Find the persistent object in the scene
        SteamLobby persistentObj = FindObjectOfType<SteamLobby>();

        if (persistentObj != null)
        {
            persistentObj.HostLobby();
        }
        else
        {
            Debug.LogWarning("PersistentObject not found in the scene.");
        }
    }

    public void ToStatistics(){
        Achievements.UnlockStatsAchievement();
        StartCoroutine(LoadNewLevel("StatisticsScene"));
    }

    public void ToDemo(){
        Variables.gameMode = "Demo";
        StartCoroutine(LoadNewLevel("DemoLevel"));
    }
    IEnumerator LoadNewLevel(string sceneName) {
        if(transition != null) {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene(sceneName);
    }
}
