using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NextLevel : MonoBehaviour
{

    private bool isScaling = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isScaling)
        {
            isScaling = true;
            StartCoroutine(ScalePlayerAndLoadNextLevel(other.gameObject));
        }
    }

    private IEnumerator ScalePlayerAndLoadNextLevel(GameObject player)
    {
        float i = 0;
        float rate = 1 / 2f;
    
        Vector3 fromScale = player.transform.localScale;
        Vector3 toScale = Vector3.zero;
        Vector3 initialPosition = player.transform.position;

        while (i < 1)
        {
            i += Time.deltaTime * rate;
            
            player.transform.localScale = Vector3.Lerp(fromScale, toScale, i);
            player.transform.position = Vector3.Lerp(initialPosition, transform.position, i);

            yield return 0;
        }
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        int levelNumber;
        if (int.TryParse(currentSceneName.Replace("Level ", ""), out levelNumber))
        {
            switch (levelNumber)
            {
                case 1:
                PlayerPrefs.SetString("Level", "Level 2");
                    SceneManager.LoadScene("Level 2");
                    break;
                case 2:
                SpeedrunTimer.SaveTime();
                /*PlayerPrefs.SetString("Level", "Level 3");
                    SceneManager.LoadScene("Level 3");*/
                    break;
                case 3: 
                if(Variables.gameMode == "Speedrun") {
                    SpeedrunTimer.SaveTime();
                } else {
                    SceneManager.LoadScene("MainMenuScene");
                }
                    break;
                default:
                    Debug.LogWarning("Level 1");
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Error.");
        }
    }


}
