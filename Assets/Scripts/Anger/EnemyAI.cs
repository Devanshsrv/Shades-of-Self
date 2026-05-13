using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Combat Settings")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    private float attackTimer = 0f;

    [Header("Teleport Settings")]
    public Transform[] teleportPoints;
    public int teleportHealthThreshold = 70;
    public float teleportCooldown = 10f;
    private float teleportTimer = 0f;
    private bool teleportPhase = false;

    [Header("Visual Prefab Variants")]
    public GameObject[] enemyVariants;
    private GameObject currentVisual;

    private Animator anim;
    private Transform player;
    private EnemyHealth health;

    // NEW: Overflow freeze state
    private bool frozen = false;    // NEW
    private EnemySFX sfx;


    public GameObject levelExitTrigger;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = GetComponent<EnemyHealth>();
        sfx = GetComponentInChildren<EnemySFX>();

        ChangeForm(); // spawn first form
    }


    void Update()
    {
        // NEW: If frozen by rage overflow, do nothing
        if (frozen) return;   // NEW

        if (health == null || health.currentHealth <= 0)
            return;

        FacePlayer();

        attackTimer -= Time.deltaTime;
        teleportTimer -= Time.deltaTime;

        HandleAttack();
        HandleTeleportPhase();
    }


    // =====================
    //     ATTACK LOGIC
    // =====================
    void HandleAttack()
    {
        if (player == null || anim == null) return;

        float dist = Vector2.Distance(player.position, transform.position);

        if (dist <= attackRange && attackTimer <= 0f)
        {
            anim.SetTrigger("Attack");

            // 🔊 Play appropriate attack sound based on current variant
            if (sfx != null)
                sfx.PlayAttackSound();

            attackTimer = attackCooldown;
        }

    }


    // =====================
    //     TELEPORT LOGIC
    // =====================
    void HandleTeleportPhase()
    {
        if (!teleportPhase &&
            health.currentHealth > 0 &&
            health.currentHealth <= teleportHealthThreshold)
        {
            teleportPhase = true;
            teleportTimer = 0f;
        }

        if (!teleportPhase)
            return;

        if (teleportTimer > 0f)
            return;

        teleportTimer = teleportCooldown;

        int rand = Random.Range(0, teleportPoints.Length);
        transform.position = teleportPoints[rand].position;

        ChangeForm();
    }


    // =====================
    //   CHANGE VISUAL FORM
    // =====================
    void ChangeForm()
    {
        if (currentVisual != null)
            Destroy(currentVisual);

        int rand = Random.Range(0, enemyVariants.Length);

        currentVisual = Instantiate(enemyVariants[rand], transform);
        currentVisual.transform.localPosition = Vector3.zero;

        anim = currentVisual.GetComponentInChildren<Animator>();
        sfx = currentVisual.GetComponentInChildren<EnemySFX>(); // 🔊 grab the new SFX script
    }


    // =====================
    //   HIT / DEATH ANIMS
    // =====================
    public void PlayHit()
    {
        if (!frozen && anim != null)  // NEW: prevent hit anim while frozen
            anim.SetTrigger("Hit");
    }

    public void Die()
    {
        if (anim != null && levelExitTrigger != null)
        {
            levelExitTrigger.SetActive(true);
            anim.SetTrigger("Die");
        }

        Destroy(gameObject, 1.2f);
    }


    // =====================
    //    FACE THE PLAYER
    // =====================
    void FacePlayer()
    {
        if (currentVisual == null || player == null) return;

        Vector3 scale = currentVisual.transform.localScale;

        if (player.position.x < transform.position.x)
            scale.x = Mathf.Abs(scale.x) * -1;
        else
            scale.x = Mathf.Abs(scale.x);

        currentVisual.transform.localScale = scale;
    }


    // =====================
    // DAMAGE VIA ANIM EVENT
    // =====================
    public void DealDamageEvent()
    {
        if (player == null) return;

        float dist = Vector2.Distance(player.position, transform.position);

        if (dist <= attackRange)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(4);
        }
    }


    // =====================
    // NEW — FREEZE AI
    // Called when player hits RAGE OVERFLOW
    // =====================
    public void FreezeEnemy()       // NEW
    {
        frozen = true;

        // stop animations too
        if (anim != null)
        {
            anim.speed = 0f; // freeze animator
        }
    }
}
