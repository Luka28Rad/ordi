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
    [SerializeField] private float flyingForce = 600f;  // Force for Dusko's flying ability
    [SerializeField] private float flyDuration = 0.5f;  // How long Dusko can fly
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;    
    [SerializeField] private LayerMask m_WhatIsGround;                            
    [SerializeField] private Transform m_GroundCheck;                            
    [SerializeField] private Transform m_CeilingCheck;                          
    [SerializeField] private ParticleSystem dashTrail;
    [SerializeField] private GameObject dashIndicator;

    const float k_GroundedRadius = .3f;
    private bool m_Grounded;            
    private bool canDash;
    private bool canDoubleJump;
    private bool canFly;  // Added for Dusko's flying ability
    const float k_CeilingRadius = .2f;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  
    private Vector3 m_Velocity = Vector3.zero;
    private float input;
    private bool toJump = false;
    private bool toDash = false;
    private bool toFly = false;  // Added for Dusko's flying ability
    private float timeSinceLastDash = 0f;
    private float timeSinceLastDoubleJump = 0f;
    private float timeSinceLastFly = 0f;  // Added for Dusko's flying ability
    readonly private float coyoteTimer = 0.2f;
    private float coyoteTimerTemp = 0.2f;
    readonly private float jumpBuffer = 0.1f;
    private float jumpBufferTemp = 0.1f;
    private bool canMove = true;
    private Animator m_animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private int maxJumps = 2;
    private int remainingJumps;
    private bool springshroomDoubleJump = false;
    private bool isDusko = false;

    [SyncVar(hook = nameof(OnIndicatorStateChanged))]
    private bool isIndicatorVisible = true;
    [SerializeField] private float jumpHoldThreshold = 0.4f;  // Time needed to hold jump for flying
    
    private float jumpHoldTimer = 0f;
    private bool isHoldingJump = false;
    [SerializeField] private float teleportDistance = 5f;
    [SerializeField] private ParticleSystem poofParticle;
    [SerializeField] private float teleportDelay = 0.5f;
    private bool isTeleporting = false;
    // Add to existing private fields
    private bool isWizzy = false;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        CheckCharacterType();
        remainingJumps = maxJumps;
        canDoubleJump = true;
        canFly = isDusko;  // Initialize flying ability if Dusko
    }

    private void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        CheckCharacterType();
        remainingJumps = maxJumps;
        canDoubleJump = true;
        canFly = isDusko;  // Initialize flying ability if Dusko
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (dashIndicator != null)
        {
            UpdateIndicatorVisibility(isIndicatorVisible);
        }
    }

        private void CheckCharacterType()
    {
        if(spriteRenderer)
        {
            string spriteName = spriteRenderer.sprite.name.ToLower();
            if(spriteName.Contains("springshroom"))
            {
                springshroomDoubleJump = true;
                isDusko = false;
                isWizzy = false;
            }
            else if(spriteName.Contains("dusko"))
            {
                isDusko = true;
                springshroomDoubleJump = false;
                isWizzy = false;
            }
            else if(spriteName.Contains("wizzy"))
            {
                isWizzy = true;
                isDusko = false;
                springshroomDoubleJump = false;
            }
        }
    }

    private bool set = false;
    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "MultiplayerLevel" && !isOwned) return;
        if(spriteRenderer && spriteRenderer.sprite.name.ToLower().Contains("springshroom") && !set) 
        {
            springshroomDoubleJump = true;
            set = true;
        }
        
        input = Input.GetAxisRaw("Horizontal");
        
        // Handle jump input and flying for Dusko
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            toJump = true;
            jumpBufferTemp = jumpBuffer;
            if(isDusko)
            {
                isHoldingJump = true;
                jumpHoldTimer = 0f;
            }
        }

        // Track jump hold duration for Dusko's flying ability
        if(isDusko && isHoldingJump)
        {
            if(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W))
            {
                jumpHoldTimer += Time.deltaTime;
                if(jumpHoldTimer >= jumpHoldThreshold && canFly)
                {
                    toFly = true;
                    isHoldingJump = false;
                }
            }
            else
            {
                isHoldingJump = false;
            }
        }

        // Remove the previous Dusko flying input check
        if(!isDusko && Input.GetKeyDown(KeyCode.LeftShift))
        {
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
                if (springshroomDoubleJump)
                {
                    if (Time.time - timeSinceLastDoubleJump > 0.4f)
                    {
                        if(SceneManager.GetActiveScene().name == "MultiplayerLevel") 
                        {
                            CmdSetIndicatorState(true);
                            canDoubleJump = true;
                        }
                        else 
                        {
                            UpdateIndicatorVisibility(true);
                            canDoubleJump = true;
                        }
                    }
                }
                else if (isDusko)
                {
                    if (Time.time - timeSinceLastFly > 0.4f)
                    {
                        if(SceneManager.GetActiveScene().name == "MultiplayerLevel") 
                        {
                            CmdSetIndicatorState(true);
                            canFly = true;
                        }
                        else 
                        {
                            UpdateIndicatorVisibility(true);
                            canFly = true;
                        }
                    }
                }
                else
                {
                    if (Time.time - timeSinceLastDash > 0.4f)
                    {
                        if(SceneManager.GetActiveScene().name == "MultiplayerLevel") 
                        {
                            CmdSetIndicatorState(true);
                            canDash = true;
                        }
                        else 
                        {
                            UpdateIndicatorVisibility(true);
                            canDash = true;
                        }
                    }
                }
                m_animator.SetBool("isJumping", false);
            }
        }

        if (canMove)
        {
            Move(input * Time.fixedDeltaTime * moveSpeed, toJump, toDash, toFly);
        }
        else
        {
            m_animator.SetBool("isWalking", false);
        }
        
        toJump = false;
        toDash = false;
        toFly = false;
    }

    private void UpdateIndicatorVisibility(bool visible)
    {
        if (dashIndicator != null)
        {
            dashIndicator.GetComponent<Renderer>().enabled = visible;
            dashIndicator.GetComponent<Light2D>().enabled = visible;
        }
    }

    private void OnIndicatorStateChanged(bool oldValue, bool newValue)
    {
        UpdateIndicatorVisibility(newValue);
    }

    [Command]
    private void CmdSetIndicatorState(bool visible)
    {
        isIndicatorVisible = visible;
    }

    [Command]
    private void CmdUpdateWalkingState(bool walking)
    {
        isWalking = walking;
    }

    [Command]
    private void CmdUpdateJumpingState(bool jumping)
    {
        isJumping = jumping;
    }

    [SyncVar(hook = nameof(OnWalkingStateChanged))]
    private bool isWalking;

    [SyncVar(hook = nameof(OnJumpingStateChanged))]
    private bool isJumping;

    private void OnWalkingStateChanged(bool oldValue, bool newValue)
    {
        m_animator.SetBool("isWalking", newValue);
    }

    private void OnJumpingStateChanged(bool oldValue, bool newValue)
    {
        m_animator.SetBool("isJumping", newValue);
    }

    public void Move(float move, bool jump, bool dash, bool fly)
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
            remainingJumps = maxJumps;
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

        // Normal jump logic for all characters
        bool canJump = (m_Grounded || coyoteTimerTemp > 0 || 
                      (springshroomDoubleJump && remainingJumps > 0 && canDoubleJump));
        
        if (canJump && jump)
        {
            if (!m_Grounded && coyoteTimerTemp <= 0 && springshroomDoubleJump)
            {
                if(SceneManager.GetActiveScene().name == "MultiplayerLevel")
                {
                    CmdSetIndicatorState(false);
                }
                else 
                {
                    UpdateIndicatorVisibility(false);
                }
                canDoubleJump = false;
                timeSinceLastDoubleJump = Time.time;
            }
            
            coyoteTimerTemp = 0;
            m_Grounded = false;
            remainingJumps--;
            
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            m_animator.SetBool("isJumping", true);
        }

        if(isWizzy && dash && canDash)
        {
            if(SceneManager.GetActiveScene().name == "MultiplayerLevel")
            {
                CmdSetIndicatorState(false);
                CmdTeleport();
            }
            else 
            {
                UpdateIndicatorVisibility(false);
                Teleport();
            }
            
            canDash = false;
            timeSinceLastDash = Time.time;
        }

        // Dusko's flying ability
        if(isDusko && canFly && fly)
        {
            if(SceneManager.GetActiveScene().name == "MultiplayerLevel")
            {
                CmdSetIndicatorState(false);
            }
            else 
            {
                UpdateIndicatorVisibility(false);
            }
            
            canFly = false;
            timeSinceLastFly = Time.time;
            StartCoroutine(Fly());
        }

        // Normal dash ability for non-Dusko characters
        if(!springshroomDoubleJump && !isDusko && canDash && dash)
        {
            if(SceneManager.GetActiveScene().name == "MultiplayerLevel")
            {
                CmdSetIndicatorState(false);
            }
            else 
            {
                UpdateIndicatorVisibility(false);
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
    IEnumerator Fly()
    {
        float flyTimer = 0f;
        float originalGravity = m_Rigidbody2D.gravityScale;
        m_Rigidbody2D.gravityScale = 0;
        
        while(flyTimer < flyDuration)
        {
            m_Rigidbody2D.AddForce(new Vector2(0f, flyingForce * Time.deltaTime));
            flyTimer += Time.deltaTime;
            m_animator.SetBool("isJumping", true);
            yield return null;
        }
        
        m_Rigidbody2D.gravityScale = originalGravity;
    }

    private void Teleport()
    {
        if (!isTeleporting)
        {
            StartCoroutine(TeleportSequence());
        }
    }
private IEnumerator TeleportSequence()
    {
        isTeleporting = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, teleportDistance, 0);

        // Initial poof and disappear
        if (poofParticle != null)
        {
            Instantiate(poofParticle, startPos + new Vector3(0, 0, -1), transform.rotation);
        }
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(teleportDelay);

        // Spawn end poof and appear
        if (poofParticle != null)
        {
            Instantiate(poofParticle, endPos + new Vector3(0, 0, -1), transform.rotation);
        }
        transform.position = endPos;
        spriteRenderer.enabled = true;
        isTeleporting = false;
    }

    [Command]
    private void CmdTeleport()
    {
        if (!isTeleporting)
        {
            StartCoroutine(NetworkTeleportSequence());
        }
    }

    private IEnumerator NetworkTeleportSequence()
    {
        isTeleporting = true;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, teleportDistance, 0);
        int checkNum = Mathf.FloorToInt(endPos.y);
        checkNum = (checkNum + 1)%3;
        if(checkNum != 0) {
            endPos = endPos + new Vector3(0, 0.5f, 0);
        }

        // Initial disappear and poof on all clients
        RpcStartTeleport(startPos);

        yield return new WaitForSeconds(teleportDelay);

        // Final poof and appear on all clients
        RpcEndTeleport(endPos);
        transform.position = endPos;
        isTeleporting = false;
    }

    [ClientRpc]
    private void RpcStartTeleport(Vector3 startPos)
    {
        if (poofParticle != null)
        {
            Instantiate(poofParticle, startPos + new Vector3(0, 0, -1), transform.rotation);
        }
        spriteRenderer.enabled = false;
    }

    [ClientRpc]
    private void RpcEndTeleport(Vector3 endPos)
    {
        if (poofParticle != null)
        {
            Instantiate(poofParticle, endPos + new Vector3(0, 0, -1), transform.rotation);
        }
        transform.position = endPos;
        spriteRenderer.enabled = true;
    }
}