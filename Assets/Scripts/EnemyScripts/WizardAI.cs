using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAI : MonoBehaviour
{
    [SerializeField] GameObject attackPoint;
    [SerializeField] GameObject laserAttack;
    GameObject player;
    GameObject attack;
    private bool m_FacingRight = false;
    LineRenderer lineRenderer;
    bool laser = false;
    float attackSpeed = 20f;
    float movementSpeed = 5;
    float movementRange = 0.2f;
    float initialY;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lineRenderer = attackPoint.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        attackPoint.SetActive(false);
        initialY = transform.position.y;
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
            Bob();
        }
        if (Input.GetKeyDown(KeyCode.N) && !laser)
        {
            StartCoroutine(AttackLaser());
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
        laser = true;
        yield return new WaitForSeconds(0.7f);
        Pew();
        yield return new WaitForSeconds(0.7f);
        Pew();
        yield return new WaitForSeconds(0.7f);
        Pew();
        yield return new WaitForSeconds(0.8f);
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

    void Bob()
    {
        float verticalMovement = Mathf.Sin(Time.time * movementSpeed) * movementRange;
        transform.position = new Vector3(transform.position.x, initialY + verticalMovement, transform.position.z);
    }
}
