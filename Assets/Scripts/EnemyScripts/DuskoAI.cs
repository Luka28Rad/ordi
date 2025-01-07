using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuskoAI : MonoBehaviour
{
    GameObject player;
    private bool m_FacingRight = true;
    readonly float speed = 6f;
    readonly float attackRange = 6f;
    readonly float stalkRange = 40f;
    private bool hasAttacked = false;
    SpriteRenderer spriteRenderer;
    [SerializeField] GameObject deathEffect;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (IsPlayerClose())
        {
            Debug.Log("Player is close to the enemy!");
        }     
    }

    private bool IsPlayerClose()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance <= attackRange) {
                Achievements.UnlockSpotDuskoAchievement();
                return true;
            }
        }
        return false;
    }

    private void LateUpdate()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < attackRange)
        {
            hasAttacked = true;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(player.transform.position, transform.position) < stalkRange)
        {
            if (hasAttacked)
            {
                StartCoroutine(Disappear(1f));
            }
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
            Achievements.UnlockDuskoDamageAchievement();
            //Napravi da player bude imun na damge neko kratko vrijeme dodaj coroutine u damage the player
            Instantiate(deathEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    bool started = false;
    private IEnumerator Disappear(float seconds)
    {
        if(!started) {
            Achievements.UnlockDuskoKillAchievement();
            started = true;
        }
        float elapsedTime = 0f;
        Color targetColor = Color.clear;

        while (elapsedTime < seconds)
        {
            spriteRenderer.color = Color.Lerp(Color.white, targetColor, elapsedTime / seconds);

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        spriteRenderer.color = targetColor;

        spriteRenderer.enabled = false;
        Instantiate(deathEffect, transform.position, transform.rotation);
        Destroy(gameObject);
        yield return new WaitForSeconds(1f);
        
    }
}
