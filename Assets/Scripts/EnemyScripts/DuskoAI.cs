using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuskoAI : MonoBehaviour
{
    GameObject player;
    private bool m_FacingRight = true;
    readonly float speed = 7f;
    readonly float attackRange = 4f;
    readonly float stalkRange = 40f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x > transform.position.x && !m_FacingRight)
        {
            Flip();
        }
        else if (player.transform.position.x < transform.position.x && m_FacingRight)
        {
            Flip();
        }     
    }

    private void LateUpdate()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < attackRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(player.transform.position, transform.position) < stalkRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 0.5f * Time.deltaTime);
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)    //CHECK IF PLAYER
        {
            collision.GetComponent<HealthManager>().TakeDamage();
            //Napravi da player bude imun na damge neko kratko vrijeme dodaj coroutine u damage the player
            Destroy(gameObject);
        }
    }
}
