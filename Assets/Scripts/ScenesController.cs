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

    [SerializeField] GameObject levelButtons;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject title;
    [SerializeField] GameObject settingsButtons;
    [SerializeField] Sprite panelImageNormal;
    [SerializeField] Sprite panelImageLevels;
    
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "CreditsScene")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Mozda2");
                SceneManager.LoadScene("MainMenuScene");
            }
        }
    }
    public void LeaderboardButtonClicked()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void EndlessScene(){
        Variables.gameMode = "Endless";
        SceneManager.LoadScene("EndlessLevel");
    }

    public void CreditsButtonClicked()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void SpeedRunButtonClicked()
    {
        PlayerPrefs.SetFloat("SpeedTime", 0);
        PlayerPrefs.SetString("collectiblesSpeedRun", "");
        Variables.gameMode = "Speedrun";
        SceneManager.LoadScene("Level 1");
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
            default:
            Debug.Log("Mozda3");
            SceneManager.LoadScene("MainMenuScene");
            break;
        }
    }
    public void NewGameButtonClicked()
    {
        Variables.gameMode = "NewGame";
        PlayerPrefs.SetString("Checkpoint", "StartCheckpoint");
        PlayerPrefs.SetString("collectibles", "");
        PlayerPrefs.SetString("Level", "Level 1");
        SceneManager.LoadScene("Level 1");
    }

    public void LoadGameButtonClicked()
    {
        Variables.gameMode = "LoadGame";
        string level = PlayerPrefs.GetString("Level", "Level 1");
        SceneManager.LoadScene(level);
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
        SceneManager.LoadScene("Level " + level);
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
        SceneManager.LoadScene("HostLobbyScene");
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
        SceneManager.LoadScene("MainMenuScene");
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
}
