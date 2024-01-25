using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    CheckpointManager checkpointManager;
    // Start is called before the first frame update
    void Start()
    {
                if((Variables.gameMode == "Speedrun" || Variables.gameMode == "Practice")) {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Variables.gameMode != "Speedrun" && Variables.gameMode != "Practice") {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.white;
        Debug.Log("Level " +  SceneManager.GetActiveScene().name);
        PlayerPrefs.SetString("Level", SceneManager.GetActiveScene().name);
        Debug.Log("Aktiviran chekpoint: " + gameObject.name);
        PlayerPrefs.SetString("Checkpoint", gameObject.name);
        GetComponent<Collider2D>().enabled = false;
        }
    }
}
