using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class SpiderAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;            // assign in Inspector or auto-find by tag

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float detectionRange = 6f;   // chase when inside this range
    public float attackRange = 1.5f;    // attack when closer than this

    [Header("Attack")]
    public float attackCooldown = 1f;   // seconds between attacks
    public int damage = 1;

    Rigidbody2D rb;
    Animator anim;
    float lastAttackTime;
    float baseScaleX;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        baseScaleX = Mathf.Abs(transform.localScale.x);
    }

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
            {
                player = p.transform;
                Debug.Log("SpiderAI: Found player by tag.");
            }
            else
            {
                Debug.LogError("SpiderAI: No GameObject with tag 'Player' found.");
            }
        }
    }

    void Update()
    {
        if (player == null)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool("isWalking", false);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        // Visualize distance in Scene view
        Debug.DrawLine(transform.position, player.position, Color.red);

        // === FACE PLAYER ===
        if (player.position.x > transform.position.x)
            transform.localScale = new Vector3(baseScaleX, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(-baseScaleX, transform.localScale.y, transform.localScale.z);

        // === MOVE / IDLE ===
        if (distance <= detectionRange && distance > attackRange)
        {
            float dir = Mathf.Sign(player.position.x - transform.position.x);
            rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);

            anim.SetBool("isWalking", true);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool("isWalking", false);
        }

        // === ATTACK ===
        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("SpiderAI: ATTACK, distance = " + distance);

            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool("isWalking", false);

            // Fire the trigger – this will enter spiderAttack state
            anim.SetTrigger("isAttacking");

            lastAttackTime = Time.time;

            // Damage + knockback (if PlayerHealth exists)
            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null)
            {
                Vector2 hitDir = (player.position - transform.position).normalized;
                health.TakeDamage(damage, hitDir);
            }
        }
    }
}
