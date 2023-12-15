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
        if (collision.gameObject.layer == 3)    //CHECK IF PLAYER
        {
            collision.GetComponent<HealthManager>().TakeDamage();
            collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(-collision.GetComponent<Rigidbody2D>().velocity.normalized.x, 0.2f) * 30f, ForceMode2D.Impulse);
        }
    }
}
