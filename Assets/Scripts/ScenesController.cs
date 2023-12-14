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

    public void OptionsButtonClicked()
    {
        SceneManager.LoadScene("OptionsScene");
    }

    public void PlayButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
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
