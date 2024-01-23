using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    bool isPaused = false;
    [SerializeField] GameObject menu;

    private void Start()
    {
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        if (isPaused)
        {
            Variables.paused = false;
            menu.SetActive(false);
            isPaused = false;
            Time.timeScale = 1;
            
        }
        else
        {
            Variables.paused = true;
            menu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Continue()
    {
        TogglePause();
    }
}
