using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            menu.SetActive(false);
            isPaused = false;
            Time.timeScale = 1;
        }
        else
        {
            menu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }
    }

    public void Exit()
    {
        //tu ide kod za sejvanje il sta vec
    }

    public void Continue()
    {
        TogglePause();
    }
}
