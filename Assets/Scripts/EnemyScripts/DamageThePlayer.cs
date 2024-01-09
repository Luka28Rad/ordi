using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageThePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)    //CHECK IF PLAYER
        { 
            collision.GetComponent<HealthManager>().TakeDamage();
            //Napravi da player bude imun na damge neko kratko vrijeme dodaj coroutine u damage the player
            collision.GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(BackToNormalColor(collision.GetComponent<SpriteRenderer>(), Color.white, 0.25f, collision));
        }
    }

    IEnumerator BackToNormalColor(SpriteRenderer renderer, Color targetColor, float delay, Collider2D collision)
    {
        collision.gameObject.layer = 7;
        yield return new WaitForSeconds(delay);
        renderer.color = targetColor;
        collision.gameObject.layer = 3;
    }
}
