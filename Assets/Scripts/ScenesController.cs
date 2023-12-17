using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    public void LeaderboardButtonClicked()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void SpeedRunButtonClicked()
    {
        Variables.gameMode = "Speedrun";
        SceneManager.LoadScene("GameScene");
    }

    public void NewGameButtonClicked()
    {
        Variables.gameMode = "NewGame";
        SceneManager.LoadScene("GameScene");
    }

    public void LoadGameButtonClicked()
    {
        Variables.gameMode = "LoadGame";
        SceneManager.LoadScene("GameScene");
    }

    public void PracticeButtonClicked()
    {
        Variables.gameMode = "Practice";
        SceneManager.LoadScene("GameScene");
    }

    public void OptionsButtonClicked()
    {
        SceneManager.LoadScene("OptionsScene");
    }

    public void PlayButtonClicked(GameObject buttonsPanel)
    {
        if(!buttonsPanel.activeSelf)
            buttonsPanel.SetActive(true);
        else 
             buttonsPanel.SetActive(false);
    }

    public void MainMenuClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ExitButtonClicked()
    {
        UnityEditor.EditorApplication.isPlaying = false; // ovu liniju treba maknuti prije buildanja jer ce inace stvarat error - omogucuje da se izade iz editora na klik
        Application.Quit(); // ova logicno izlazi iz aplikacije
    }
}
