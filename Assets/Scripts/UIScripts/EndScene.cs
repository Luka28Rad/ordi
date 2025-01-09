using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public Animator transition;
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
    public void GoToMenu()
    {
        Debug.Log("myb");
        StartCoroutine(LoadNewLevel("MainMenuScene"));
    }

    
    IEnumerator LoadNewLevel(string sceneName) {
        if(transition != null) {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene(sceneName);
    }
}
