using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class SpiderAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;            // will auto-find Player tag if left empty

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float detectionRange = 6f;   // start chasing when inside this range
    public float attackRange = 1.2f;    // stop & attack when closer than this

    [Header("Attack")]
    public float attackCooldown = 1f;   // seconds between attacks

    Rigidbody2D rb;
    Animator anim;
    float lastAttackTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        // Auto-find player by tag if not assigned
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
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

        // Move / idle
        if (distance <= detectionRange && distance > attackRange)
        {
            // Chase player
            float dir = Mathf.Sign(player.position.x - transform.position.x);
            rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);
            anim.SetBool("isWalking", true);

            // Flip sprite to face player
            if ((dir > 0 && transform.localScale.x < 0) ||
                (dir < 0 && transform.localScale.x > 0))
            {
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
        else
        {
            // Idle (too far or inside attack range)
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool("isWalking", false);
        }

        // Attack when close enough
        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool("isWalking", false);
            anim.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
    }
}
