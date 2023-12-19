using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageThePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 velTemp;
        if (collision.gameObject.layer == 3)    //CHECK IF PLAYER
        { 
            collision.GetComponent<HealthManager>().TakeDamage();
            collision.GetComponent<PlayerController>().NoMove();
            velTemp = collision.GetComponent<Rigidbody2D>().velocity;
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (Mathf.Abs(velTemp.normalized.x) < 0.5f)
            {
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(-collision.transform.localScale.x * 0.5f, 0.8f) * 8f, ForceMode2D.Impulse);
            }
            else
            {
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(-velTemp.normalized.x, 0.8f) * 8f, ForceMode2D.Impulse);
            }    
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
