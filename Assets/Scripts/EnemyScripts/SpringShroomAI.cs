using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringShroomAI : MonoBehaviour
{
    private float moveSpeed = 3f;
    Rigidbody2D rb;
    private Vector2 moveDir = Vector2.right;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerClose())
        {
            Debug.Log("Player is close to the enemy!");
        } 
        rb.velocity = moveDir * moveSpeed;
    }
        private bool IsPlayerClose()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance <= 5f) {
                Achievements.UnlockSpotGljivanAchievement();
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 3 && collision.gameObject.layer != 7)    //CHECK IF NOT PLAYER
        {
            moveDir *= -1;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
