using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringShroomAI : MonoBehaviour
{
    private float moveSpeed = 3f;
    Rigidbody2D rb;
    private Vector2 moveDir = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = moveDir * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 3)    //CHECK IF NOT PLAYER
        {
            moveDir *= -1;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
