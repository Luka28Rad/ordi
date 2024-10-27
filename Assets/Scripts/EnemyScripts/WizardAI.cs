using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAI : MonoBehaviour
{
    [SerializeField] GameObject attackPoint;
    [SerializeField] GameObject laserAttack;
    [SerializeField] GameObject[] teleportPoints;
    [SerializeField] GameObject poof;
    [SerializeField] GameObject damageTaken;
    [SerializeField] GameObject teleportGate;
    GameObject player;
    GameObject attack;
    private bool m_FacingRight = true;
    LineRenderer lineRenderer;
    bool laser = false;
    float attackSpeed = 20f;
    float movementSpeed = 5;
    float movementRange = 0.2f;
    float initialY;
    float timeToWait = 9f;
    float elapsedTime = 7;
    int hp = 3;
    int currentPosition = 0;
    float immunityTime = 0.7f;
    float elapsedImmunityTime = 1f;
    int exPos = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lineRenderer = attackPoint.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        attackPoint.SetActive(false);
        initialY = transform.position.y;
        transform.position = teleportPoints[0].transform.position;
    }

        private bool IsPlayerClose()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance <= 5f) {
                Achievements.UnlockSpotWizardAchievement();
                return true;
            }
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (IsPlayerClose())
        {
            Debug.Log("Player is close to the boss!");
        } 
        elapsedTime += Time.deltaTime;
        elapsedImmunityTime += Time.deltaTime;
        if (elapsedTime >= timeToWait)
        {
            elapsedTime = 0;
            DecideAttack();
        }

        Bob();
        if (player.transform.position.x > transform.position.x && !m_FacingRight)
        {
            Flip();
        }
        else if (player.transform.position.x < transform.position.x && m_FacingRight)
        {
            Flip();
        }

        //visuals for laser attack
        if (laser)
        {
            attackPoint.SetActive(true);
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, attackPoint.transform.position);
            lineRenderer.SetPosition(1, player.transform.position);
        }
        else
        {
            attackPoint.SetActive(false);
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

    IEnumerator AttackLaser()
    {
        yield return new WaitForSeconds(0.3f);
        laser = true;
        yield return new WaitForSeconds(1f);
        Pew();
        yield return new WaitForSeconds(0.2f);
        Pew();
        yield return new WaitForSeconds(0.2f);
        Pew();
        yield return new WaitForSeconds(0.2f);
        Pew();
        yield return new WaitForSeconds(0.2f);
        Pew();
        yield return new WaitForSeconds(0.2f);
        Pew();
        yield return new WaitForSeconds(0.2f);
        Pew();
        yield return new WaitForSeconds(0.7f);
        laser = false;
        lineRenderer.enabled = false;

    }

    IEnumerator AttackLaserFast()
    {
        yield return new WaitForSeconds(0.3f);
        laser = true;
        yield return new WaitForSeconds(1f);
        PewBuff();
        yield return new WaitForSeconds(0.2f);
        PewBuff();
        yield return new WaitForSeconds(0.2f);
        PewBuff();
        yield return new WaitForSeconds(0.2f);
        PewBuff();
        yield return new WaitForSeconds(0.2f);
        PewBuff();
        yield return new WaitForSeconds(0.2f);
        PewBuff();
        yield return new WaitForSeconds(0.2f);
        PewBuff();
        yield return new WaitForSeconds(0.7f);
        laser = false;
        lineRenderer.enabled = false;

    }

    IEnumerator AttackLaserUltimate()
    {
        yield return new WaitForSeconds(0.3f);
        laser = true;
        yield return new WaitForSeconds(1f);
        Pew();
        yield return new WaitForSeconds(0.2f);
        Pew();
        StartCoroutine(DecideRandomPosition());
        yield return new WaitForSeconds(0.2f);
        Pew();
        yield return new WaitForSeconds(0.2f);
        Pew();
        StartCoroutine(DecideRandomPosition());
        yield return new WaitForSeconds(0.2f);
        Pew();
        yield return new WaitForSeconds(0.2f);
        Pew();
        StartCoroutine(DecideRandomPosition());
        yield return new WaitForSeconds(0.2f);
        Pew();
        yield return new WaitForSeconds(0.2f);
        Pew();
        StartCoroutine(DecideRandomPosition());
        yield return new WaitForSeconds(0.2f);
        Pew();
        yield return new WaitForSeconds(0.2f);
        Pew();
        StartCoroutine(DecideRandomPosition());
        yield return new WaitForSeconds(0.2f);
        Pew();
        yield return new WaitForSeconds(0.2f);
        Pew();
        laser = false;
        lineRenderer.enabled = false;

    }

    void Pew()
    {
        attack = Instantiate(laserAttack, attackPoint.transform);
        attack.SetActive(true);
        attack.transform.parent = null;
        attack.GetComponent<Rigidbody2D>().velocity = (player.transform.position - attackPoint.transform.position).normalized * attackSpeed;
        attack.transform.right = player.transform.position - attack.transform.position;
        Destroy(attack, 2f);
    }

    void PewBuff()
    {
        attack = Instantiate(laserAttack, attackPoint.transform);
        attack.SetActive(true);
        attack.transform.parent = null;
        attack.GetComponent<Rigidbody2D>().velocity = (player.transform.position - attackPoint.transform.position).normalized * attackSpeed;
        attack.transform.right = player.transform.position - attack.transform.position;
        Destroy(attack, 2f);
        attack = Instantiate(laserAttack, attackPoint.transform);
        attack.SetActive(true);
        attack.transform.parent = null;
        attack.GetComponent<Rigidbody2D>().velocity = (player.transform.position - attackPoint.transform.position + new Vector3(0, 3, 0)).normalized * attackSpeed;
        attack.transform.right = player.transform.position - attack.transform.position;
        Destroy(attack, 2f);
        attack = Instantiate(laserAttack, attackPoint.transform);
        attack.SetActive(true);
        attack.transform.parent = null;
        attack.GetComponent<Rigidbody2D>().velocity = (player.transform.position - attackPoint.transform.position + new Vector3(0, -3, 0)).normalized * attackSpeed;
        attack.transform.right = player.transform.position - attack.transform.position;
        Destroy(attack, 2f);
    }

    void Bob()
    {
        float verticalMovement = Mathf.Sin(Time.time * movementSpeed) * movementRange;
        transform.position = new Vector3(transform.position.x, initialY + verticalMovement, transform.position.z);
    }

    void DecideAttack()
    {
        if (hp == 3)
        {
            StartCoroutine(DecideRandomPosition());
            StartCoroutine(AttackLaser());
        }

        if (hp == 2)
        {
            timeToWait = 8;
            StartCoroutine(DecideRandomPosition());
            StartCoroutine(AttackLaserFast());
        }

        if (hp == 1)
        {
            timeToWait = 7;
            StartCoroutine(DecideRandomPosition());
            StartCoroutine(AttackLaserUltimate());
        }
    }

    IEnumerator DecideRandomPosition()
    {
        Instantiate(poof, transform.position + new Vector3(0, 0, -1), transform.rotation);
        int decidedPos = currentPosition;
        while (decidedPos == currentPosition || decidedPos == exPos)
        {
            decidedPos = Random.Range(0, 5);
        }
        exPos = currentPosition;
        currentPosition = decidedPos;
        yield return new WaitForSeconds(0.05f);
        transform.position = teleportPoints[decidedPos].transform.position;
        initialY = transform.position.y;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 3)    //CHECK IF NOT PLAYER
        {
            return;
        }
        collision.GetComponent<Rigidbody2D>().velocity = Vector2.up * 15;
        if (elapsedImmunityTime <= immunityTime)
        {
            return;
        }
        elapsedImmunityTime = 0;
        hp -= 1;
        Achievements.UnlockDamageWizardAchievement();
        Instantiate(damageTaken, transform.position + new Vector3(0, 0.5f, -1), transform.rotation);
        if (hp > 0)
        {
            elapsedTime = 5;
            StopAllCoroutines();
            StartCoroutine(GoToPosAfterHit());
        }
        
        if (hp <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        Debug.Log("POBJEDA");
        teleportGate.SetActive(true);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    IEnumerator GoToPosAfterHit()
    {
        Instantiate(poof, transform.position + new Vector3(0, 0, -1), transform.rotation);
        yield return new WaitForSeconds(0.05f);
        transform.position = teleportPoints[5].transform.position;
        initialY = transform.position.y;
    }
}
