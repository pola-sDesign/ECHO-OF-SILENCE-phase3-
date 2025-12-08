using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyLaser : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;
    private int currentHealth;
    private bool isDead = false;

    [Header("Animator")]
    private Animator anim;

    [Header("Laser")]
    public GameObject laser;   // Child GameObject (Laser)
    
    [Header("Teleport")]
    public Transform[] teleportPoints;

    [Header("Attack Timing")]
    public float attackCooldown = 1.5f;

    // =========================
    // Ground Check
    // =========================
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    // =========================

    void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (laser != null)
            laser.SetActive(false);

        StartCoroutine(EnemyLoop());
    }

    void Update()
    {
        // Ground check
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position,
                groundRadius,
                groundLayer
            );
        }
    }

    // 🔁 اللوب الأساسي للعدو
    IEnumerator EnemyLoop()
    {
        while (!isDead)
        {
            // العدو ما يهاجمش غير وهو على الأرض
            if (!isGrounded)
            {
                yield return null;
                continue;
            }

            // 1️⃣ Attack
            anim.SetBool("isAttacking", true);
            yield return new WaitForSeconds(1.0f);
            anim.SetBool("isAttacking", false);

            // 2️⃣ Shield + Teleport
            anim.SetBool("isShielded", true);
            yield return new WaitForSeconds(0.3f);

            Teleport();

            anim.SetBool("isShielded", false);

            // 3️⃣ Cooldown
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    // ======================================
    // Animation Events (من Animation_Attack)
    // ======================================

    public void StartLaser()
    {
        if (laser != null && !isDead)
            laser.SetActive(true);
    }

    public void StopLaser()
    {
        if (laser != null)
            laser.SetActive(false);
    }

    // ======================================
    // Teleport
    // ======================================

    void Teleport()
    {
        if (teleportPoints.Length == 0) return;

        int index = Random.Range(0, teleportPoints.Length);
        transform.position = teleportPoints[index].position;
    }

    // ======================================
    // Damage
    // ======================================

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        if (anim.GetBool("isShielded")) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ======================================
    // Death
    // ======================================

    void Die()
    {
        isDead = true;
        StopAllCoroutines();

        anim.SetBool("isAttacking", false);
        anim.SetBool("isShielded", false);
        anim.SetTrigger("Die");

        if (laser != null)
            laser.SetActive(false);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        Destroy(gameObject, 1.2f); // مدة Animation الموت
    }

    // ======================================
    // Gizmos (علشان تشوفي GroundCheck)
    // ======================================

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
