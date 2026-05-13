using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;     // normal full speed
    public float jumpForce = 14f;

    private float originalMoveSpeed; // ⭐ stores original normal speed
    private float guiltSpeedMultiplier = 0.25f; // ⭐ start at 25% speed in Guilt level
    private int helpCount = 0;       // ⭐ increases as player gets help

    [Header("Double Jump")]
    public int maxExtraJumps = 1;
    private int extraJumpsLeft;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    [Header("Combat (Anger Level Only)")]
    public bool combatEnabled = false;
    public int lightPunchDamage = 1;
    public int heavyPunchDamage = 3;

    private float horizontal;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    public RageManager rage;
    public Animator fxAnimator;

    private bool isGrounded = false;

    // ============================================================
    // 🌋 OVERFLOW VARIABLES
    // ============================================================
    public bool overflowMode = false;
    public float overflowAttackInterval = 0.25f;
    private float overflowTimer = 0f;

    // ============================================================
    // UNITY FUNCTIONS
    // ============================================================
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();

        extraJumpsLeft = maxExtraJumps;

        // ⭐ Store original speed for later restoring
        originalMoveSpeed = moveSpeed;

        // ⭐ Apply guilt slow-down only if inside Guilt level
        if (SceneManager.GetActiveScene().name == "Guilt")
        {
            moveSpeed = originalMoveSpeed * guiltSpeedMultiplier;
        }
    }

    void Update()
    {
        if (overflowMode)
        {
            HandleOverflowBehavior();
            return;
        }

        UpdateGroundCheck();
        HandleMovement();
        HandleJump();
        HandleCombat();
    }

    // ============================================================
    // 🌋 OVERFLOW BEHAVIOR
    // ============================================================
    void HandleOverflowBehavior()
    {
        overflowTimer -= Time.deltaTime;

        GameObject enemyObj = GameObject.FindGameObjectWithTag("Enemy");
        if (!enemyObj) return;

        transform.position = enemyObj.transform.position + new Vector3(-1.2f, 0, 0);

        sr.flipX = enemyObj.transform.position.x < transform.position.x;

        if (overflowTimer <= 0f)
        {
            anim.SetTrigger("LightPunch");

            EnemyHealth e = enemyObj.GetComponent<EnemyHealth>();
            if (e != null)
            {
                int dmg = Mathf.RoundToInt(4 * (1f + rage.rage / 100f));
                e.TakeDamage(dmg);
            }

            overflowTimer = overflowAttackInterval;
        }
    }

    // ============================================================
    // MOVEMENT
    // ============================================================
    void UpdateGroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        isGrounded = hit.collider != null;

        if (isGrounded)
            extraJumpsLeft = maxExtraJumps;

        anim.SetBool("Grounded", isGrounded);
    }

    void HandleMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if (horizontal > 0.1f) sr.flipX = false;
        if (horizontal < -0.1f) sr.flipX = true;

        anim.SetFloat("Speed", Mathf.Abs(horizontal));
    }

    // ============================================================
    // JUMP SYSTEM
    // ============================================================
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (extraJumpsLeft > 0)
            {
                Jump();
                extraJumpsLeft--;

                if (fxAnimator != null)
                    fxAnimator.SetTrigger("PlayFX");
            }
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    // ============================================================
    // COMBAT
    // ============================================================
    void HandleCombat()
    {
        if (!combatEnabled) return;

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("LightPunch");
            EnemyAttack(lightPunchDamage);
            SFXManager.Instance.Play(SFXManager.Instance.punch1);
        }

        if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("HeavyPunch");
            EnemyAttack(heavyPunchDamage);
            SFXManager.Instance.Play(SFXManager.Instance.punch2);
        }
    }

    void EnemyAttack(int dmg)
    {
        float dir = sr.flipX ? -1 : 1;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position + Vector3.up * 0.5f,
            Vector2.right * dir,
            1f,
            LayerMask.GetMask("Enemy")
        );

        if (hit.collider != null)
        {
            EnemyHealth e = hit.collider.GetComponent<EnemyHealth>();
            if (e != null)
            {
                int finalDamage = Mathf.RoundToInt(dmg * (1f + rage.rage / 100f));
                e.TakeDamage(finalDamage);
            }
        }
        else
        {
            rage?.AddRageFromMiss();
        }
    }

    // ============================================================
    // ⭐ GUILT LEVEL: RECEIVE HELP
    // ============================================================
    public void ReceiveHelp()
    {
        if (SceneManager.GetActiveScene().name != "Guilt") return;

        // Cap at max 3 helps
        helpCount++;
        if (helpCount > 3) helpCount = 3;

        // Convert helpCount (0–3) into a multiplier:
        // 0 → 0.25 (25%)
        // 1 → ~0.50
        // 2 → ~0.75
        // 3 → 1.0 (full normal speed)
        float multiplier = Mathf.Lerp(0.25f, 1f, helpCount / 3f);

        moveSpeed = originalMoveSpeed * multiplier;
    }


    // ============================================================
    // ENABLE OVERFLOW MODE
    // ============================================================
    public void EnableOverflowMode()
    {
        overflowMode = true;
        anim.SetBool("OverflowMode", true);

        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        combatEnabled = false;
        anim.SetFloat("Speed", 0f);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position,
                groundCheck.position + Vector3.down * groundCheckDistance);
        }
    }
}
