using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemy : MonoBehaviour
{
    [SerializeField] BoxCollider2D collider;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] CandleBehaviour candle;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 3)    //CHECK IF NOT PLAYER
        {
            return;
        }
        rb.constraints = RigidbodyConstraints2D.None;
        collision.GetComponent<Rigidbody2D>().velocity = Vector2.up * 10f;
        collision.GetComponent<HealthManager>().HealPlayer();
        collider.enabled = false;
        rb.velocity = Vector2.right * 5f + Vector2.up * 15f;
        rb.angularVelocity = -120f;
        

        candle.Death();
    }

}
