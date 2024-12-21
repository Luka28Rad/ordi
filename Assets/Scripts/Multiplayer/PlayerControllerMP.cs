using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerControllerMP : NetworkBehaviour
{
    [SerializeField] private float m_JumpForce = 600f;                            
    [SerializeField] private float m_DashForce = 600f;    
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float starDustBoostSpeed = 150f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;    
    [SerializeField] private LayerMask m_WhatIsGround;                            
    [SerializeField] private Transform m_GroundCheck;                            
    [SerializeField] private Transform m_CeilingCheck;                          
    [SerializeField] private ParticleSystem dashTrail;
    [SerializeField] private GameObject dashIndicator;

    const float k_GroundedRadius = .3f;
    private bool m_Grounded;            
    private bool canDash;
    const float k_CeilingRadius = .2f;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  
    private Vector3 m_Velocity = Vector3.zero;
    private float input;
    private bool toJump = false;
    private bool toDash = false;
    private float timeSinceLastDash = 0f;
    readonly private float coyoteTimer = 0.2f;
    private float coyoteTimerTemp = 0.2f;
    readonly private float jumpBuffer = 0.1f;
    private float jumpBufferTemp = 0.1f;
    private bool canMove = true;
    private Animator m_animator;

    [SyncVar(hook = nameof(OnDashIndicatorStateChanged))]
    private bool isDashIndicatorVisible = true;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (dashIndicator != null)
        {
            UpdateDashIndicatorVisibility(isDashIndicatorVisible);
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "MultiplayerLevel" && !isOwned) return;
        
        input = Input.GetAxisRaw("Horizontal");
        
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            toJump = true;
            jumpBufferTemp = jumpBuffer;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)){
            toDash = true;
        }
    }

    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().name == "MultiplayerLevel" && !isOwned) return;
        
        bool wasGrounded = m_Grounded;
        m_Grounded = false;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (Time.time - timeSinceLastDash > 0.4f)
                {
                    if(SceneManager.GetActiveScene().name == "MultiplayerLevel") 
                    {
                        CmdSetDashIndicatorState(true);
                        canDash = true;
                    }
                    else 
                    {
                        UpdateDashIndicatorVisibility(true);
                        canDash = true;
                    }
                }
                m_animator.SetBool("isJumping", false);
            }
        }

        if (canMove)
        {
            Move(input * Time.fixedDeltaTime * moveSpeed, toJump, toDash);
        }
        else
        {
            m_animator.SetBool("isWalking", false);
        }
        
        toJump = false;
        toDash = false;
    }

    private void UpdateDashIndicatorVisibility(bool visible)
    {
        if (dashIndicator != null)
        {
            dashIndicator.GetComponent<Renderer>().enabled = visible;
            dashIndicator.GetComponent<Light2D>().enabled = visible;
        }
    }

    private void OnDashIndicatorStateChanged(bool oldValue, bool newValue)
    {
        UpdateDashIndicatorVisibility(newValue);
    }

    [Command]
    private void CmdSetDashIndicatorState(bool visible)
    {
        isDashIndicatorVisible = visible;
    }

    public void Move(float move, bool jump, bool dash)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        if (move > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if (move < 0 && m_FacingRight)
        {
            Flip();
        }

        if(!m_Grounded){
            coyoteTimerTemp -= Time.fixedDeltaTime;
            jumpBufferTemp -= Time.fixedDeltaTime;
        }
        if(m_Grounded){
            coyoteTimerTemp = coyoteTimer;
            if (jumpBufferTemp > 0)
            {
                jump = true;
            }
        }

        if (Mathf.Abs(targetVelocity.x) > 0 && m_Grounded)
        {
            m_animator.SetBool("isWalking", true);
        }
        else
        {
            m_animator.SetBool("isWalking", false);
        }

        if ((m_Grounded || coyoteTimerTemp > 0) && jump)
        {
            coyoteTimerTemp = 0;
            m_Grounded = false;
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            m_animator.SetBool("isJumping", true);
        }

        if(canDash && dash)
        {
            if(SceneManager.GetActiveScene().name == "MultiplayerLevel")
            {
                CmdSetDashIndicatorState(false);
            }
            else 
            {
                UpdateDashIndicatorVisibility(false);
            }
            
            canDash = false;
            timeSinceLastDash = Time.time;
            dashTrail.Play();
            StartCoroutine(Dash());

            if(m_FacingRight)
            {
                m_Rigidbody2D.AddForce(new Vector2(m_DashForce, 0f));
            }
            else if(!m_FacingRight)
            {
                m_Rigidbody2D.AddForce(new Vector2(-1f * m_DashForce, 0f));
            }
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Cleanse()
    {
        m_Rigidbody2D.velocity = Vector2.zero;
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
        Debug.Log("Dashed");
        Achievements.UnlockDashAchievement();
        m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
        m_Rigidbody2D.gravityScale = 0;
        yield return new WaitForSeconds(.16f);
        m_Rigidbody2D.gravityScale = 4;
    }

    private float originalSpeed = 40f;    
    private Coroutine speedChangeCoroutine;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Box entered");
        if (other.CompareTag("StarDust"))
        {
            Achievements.UnlockEnterStardustAchievement();
            Debug.Log("Stardust entered");
            if (speedChangeCoroutine != null)
            {
                StopCoroutine(speedChangeCoroutine);
            }
            moveSpeed = starDustBoostSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("StarDust"))
        {
            Debug.Log("Stardust exited");
            if (speedChangeCoroutine != null)
            {
                StopCoroutine(speedChangeCoroutine);
            }
            speedChangeCoroutine = StartCoroutine(SmoothSpeedChange(originalSpeed));
        }
    }

    private System.Collections.IEnumerator SmoothSpeedChange(float targetSpeed)
    {
        float duration = 1f;
        float elapsedTime = 0f;
        float initialSpeed = moveSpeed;

        while (elapsedTime < duration)
        {
            moveSpeed = Mathf.Lerp(initialSpeed, targetSpeed, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        moveSpeed = targetSpeed;
    }
}