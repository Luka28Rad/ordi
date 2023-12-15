using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringShroomBoost : MonoBehaviour
{
    private float jumpStrength = 22f;

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
        if (collision.gameObject.layer != 3)    //CHECK IF NOT PLAYER
        {
            return;
        }

        if (collision.GetComponent<Transform>().transform.position.y > transform.position.y)
        {
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpStrength;
        }
    }
}
