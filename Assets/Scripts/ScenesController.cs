using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    [SerializeField] GameObject buttonsToShow;
    [SerializeField] GameObject buttonsToHide;
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "CreditsScene")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenuScene");
            }
        }
    }
    public void LeaderboardButtonClicked()
    {
        SceneManager.LoadScene("LeaderboardScene");
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
        PlayerPrefs.SetString("collectiblesSpeedRun", "");
        Variables.gameMode = "Practice";
        SceneManager.LoadScene("Level 1");
    }

    public void OptionsButtonClicked()
    {
        SceneManager.LoadScene("OptionsScene");
    }

    public void PlayButtonClicked()
    {
        if (!buttonsToShow.activeSelf)
        {
            buttonsToShow.SetActive(true);
            buttonsToHide.SetActive(false);
        }    
        else
        {
            buttonsToShow.SetActive(false);
            buttonsToHide.SetActive(true);
        }
            
    }

    public void MainMenuClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ExitButtonClicked()
    {
        //UnityEditor.EditorApplication.isPlaying = false; // ovu liniju treba maknuti prije buildanja jer ce inace stvarat error - omogucuje da se izade iz editora na klik
        Application.Quit(); // ova logicno izlazi iz aplikacije
    }
}
