using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 600f;							// Amount of force added when the player jumps.
    [SerializeField] private float m_DashForce = 600f;	
    [SerializeField] private float moveSpeed = 10f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private ParticleSystem dashTrail;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
    private bool canDash;
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
    private float input;
    private bool toJump = false;
    private bool toDash = false;
    private float timeSinceLastDash = 0f;
	private float coyoteTimer = 0.2f;
	private float coyoteTimerTemp = 0.2f;
	private bool canMove = true;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

    private void Update() {
        input = Input.GetAxisRaw("Horizontal");
		
        if(Input.GetKeyDown(KeyCode.Space)){
            toJump = true;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            toDash = true;
        }

        if(Input.GetKeyDown(KeyCode.P)){
            transform.position = new Vector3(-7f, -3.5f, 0);
        }
    }

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
                canDash = true;
			}
		}
		if (canMove)
		{
			Move(input * Time.fixedDeltaTime * moveSpeed, toJump, toDash);
		}
        toJump = false;
        toDash = false;
	}


	public void Move(float move, bool jump, bool dash)
	{
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
		if(!m_Grounded){
			coyoteTimerTemp -= Time.fixedDeltaTime;
		}
		if(m_Grounded){
			coyoteTimerTemp = coyoteTimer;
		}
		// If the player should jump...
		if ((m_Grounded || coyoteTimerTemp > 0) && jump)
		{
			coyoteTimerTemp = 0;
            // Add a vertical force to the player.
            m_Grounded = false;
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}

        if(canDash && dash && Time.time - timeSinceLastDash > 0.8f){
            canDash = false;
            timeSinceLastDash = Time.time;
			dashTrail.Play();
			StartCoroutine(Dash());

            if(m_FacingRight){
                m_Rigidbody2D.AddForce(new Vector2(m_DashForce, 0f));
            }
            else if(!m_FacingRight){
                m_Rigidbody2D.AddForce(new Vector2(-1f * m_DashForce, 0f));
            }
			
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

	public void NoMove()
	{
		canMove = false;
		StartCoroutine(MakeUnableToMove());
	}

    IEnumerator MakeUnableToMove()
    {
        yield return new WaitForSeconds(0.4f);
        canMove = true;
    }

    IEnumerator Dash()
	{
		m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
		m_Rigidbody2D.gravityScale = 0;
		yield return new WaitForSeconds(.16f);
		m_Rigidbody2D.gravityScale = 4;
	}
}