using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompMatchstick : MonoBehaviour
{
    [SerializeField] BoxCollider2D colliderMatchstick;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] MatchstickAI matchstickAI;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 3)    //CHECK IF NOT PLAYER
        {
            return;
        }
        rb.constraints = RigidbodyConstraints2D.None;
        collision.GetComponent<Rigidbody2D>().velocity = Vector2.up * 10f;
        colliderMatchstick.enabled = false;
        rb.velocity = Vector2.right * 5f + Vector2.up * 4f;
        rb.angularVelocity = -200f;
        matchstickAI.Die();
        Destroy(gameObject);
    }
}
