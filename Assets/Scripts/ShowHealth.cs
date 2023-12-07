using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHealth : MonoBehaviour
{
    readonly int maxHealth = 5;
    int healthToShow = 5;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    public Image[] hearts;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < healthToShow)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void UpdateHealth(int currentHealth)
    {
        healthToShow = currentHealth;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < healthToShow)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
