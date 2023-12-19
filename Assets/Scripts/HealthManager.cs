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
    // Start is called before the first frame update

    public void TakeDamage()
    { 
        if(Variables.gameMode == "Practice") return;
        currentHealth--;
        showHealth.UpdateHealth(currentHealth);
        Debug.Log("OUCH");
        if(currentHealth <= 0)
        {
            if(Variables.gameMode == "Speedrun") {
                SpeedrunTimer.SaveTime();
            } else {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
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

}
