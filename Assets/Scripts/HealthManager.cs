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
    float immunityTime = 0.7f;
    float elapsedTime = 1f;
    // Start is called before the first frame update
    private void Start()
    {
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
    }
    public void TakeDamage()
    {
        if (elapsedTime < immunityTime)
        {
            return;
        }
        if(Variables.gameMode == "Practice") return;
        currentHealth--;
        showHealth.UpdateHealth(currentHealth);
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(BackToNormalColor(gameObject.GetComponent<SpriteRenderer>(), Color.white, 0.4f));
        if (currentHealth <= 0)
        {
            if(Variables.gameMode == "Speedrun") {
                PlayerPrefs.SetString("collectiblesSpeedRun","");
                SceneManager.LoadScene("DeathScene");
            } else {
                Scene scene = SceneManager.GetActiveScene();
                string checkpointName = PlayerPrefs.GetString("Checkpoint", "StartCheckpoint");
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Cleanse();
                    transform.position = GameObject.Find(checkpointName).transform.position;
                    currentHealth = 5;
                    playerLight.shapeLightFalloffSize = 20;
                    showHealth.UpdateHealth(currentHealth);
                    SceneManager.LoadScene("DeathScene");
            }

        }
        playerLight.shapeLightFalloffSize -= 3;
        elapsedTime = 0f;
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
