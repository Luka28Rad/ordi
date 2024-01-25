using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
