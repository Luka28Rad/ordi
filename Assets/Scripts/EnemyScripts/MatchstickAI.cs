using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchstickAI : MonoBehaviour
{
    [SerializeField] Sprite ugasen;
    SpriteRenderer renderer;
    [SerializeField] GameObject fire;
    [SerializeField] GameObject smoke;
    [SerializeField] GameObject damagePoint;
    GameObject player;
    GameObject attack;
    Animator animator;
    bool canAttack = true;
    readonly float attackRange = 9;
    readonly float fireballSpeed = 12;
    private bool m_FacingRight = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        smoke.SetActive(false);
        renderer = GetComponent<SpriteRenderer>();
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

        if (player != null && canAttack && Vector2.Distance(player.transform.position, transform.position) < attackRange) // player != null dodao jer je tokom speedruna aktivna idalje skripta a nema playera, mislim da ne utjece na nista drugo
        {
            canAttack = false;
            Extinguish();
            Attack();
            StartCoroutine(LightUp());
        }
    }

    void Extinguish()
    {
        animator.enabled = false;
        renderer.sprite = ugasen;
        fire.SetActive(false);
        smoke.SetActive(true);
    }

    void Attack()
    {
        attack = Instantiate(fire, transform);
        attack.SetActive(true);
        attack.transform.parent = null;
        attack.GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * fireballSpeed;
        attack.GetComponent<FireballScript>().HasBeenFired();
    }

    public void Die()
    {
        damagePoint.SetActive(false);
        canAttack = false;
        StopAllCoroutines();
        Destroy(gameObject, 2);
    }

    IEnumerator LightUp()
    {
        yield return new WaitForSeconds(4);
        animator.enabled = true;
        fire.SetActive(true);
        smoke.SetActive(false);
        yield return new WaitForSeconds(1.5f); //cooldown
        canAttack = true;    
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
}
