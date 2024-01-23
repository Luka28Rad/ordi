using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    readonly int maxHealth = 5;
    int currentHealth = 5;
    [SerializeField] ShowHealth showHealth;
    [SerializeField] Light2D playerLight;
    CheckpointManager checkpointManager;
    // Start is called before the first frame update
    private void Start()
    {
        checkpointManager = GameObject.Find("CheckpointManager").GetComponent<CheckpointManager>();
    }
    public void TakeDamage()
    { 
        if(Variables.gameMode == "Practice") return;
        currentHealth--;
        showHealth.UpdateHealth(currentHealth);
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(BackToNormalColor(gameObject.GetComponent<SpriteRenderer>(), Color.white, 0.4f));
        if (currentHealth <= 0)
        {
            if(Variables.gameMode == "Speedrun") {
                SpeedrunTimer.SaveTime();
            } else {
                Scene scene = SceneManager.GetActiveScene();
                if(checkpointManager.GetLastCheckpoint() != null)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Cleanse();
                    transform.position = checkpointManager.GetLastCheckpoint();
                    currentHealth = 5;
                    playerLight.shapeLightFalloffSize = 18;
                    showHealth.UpdateHealth(currentHealth);
                }
                else
                {
                    SceneManager.LoadScene(scene.name);
                }
            }

        }
        playerLight.shapeLightFalloffSize -= 3;
    }

    public void HealPlayer()
    {
        if(currentHealth < maxHealth)
        {
            currentHealth++;
            playerLight.shapeLightFalloffSize += 3;
        }
        showHealth.UpdateHealth(currentHealth);
    }

    

    IEnumerator BackToNormalColor(SpriteRenderer renderer, Color targetColor, float delay)
    {
        gameObject.layer = 7;
        yield return new WaitForSeconds(delay);
        renderer.color = targetColor;
        gameObject.layer = 3;
    }

}
