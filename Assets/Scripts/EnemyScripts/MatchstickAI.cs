using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchstickAI : MonoBehaviour
{
    [SerializeField] Sprite ugasen;
    SpriteRenderer renderer;
    [SerializeField] GameObject fire;
    [SerializeField] GameObject smoke;
    GameObject player;
    GameObject attack;
    Animator animator;
    bool canAttack = true;
    float attackRange = 12;
    float fireballSpeed = 12;

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
        if(canAttack && Vector2.Distance(player.transform.position, transform.position) < attackRange)
        {
            canAttack = false;
            Extinguish();
            Attack();
            StartCoroutine(LightUp());
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Extinguish();
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
}
