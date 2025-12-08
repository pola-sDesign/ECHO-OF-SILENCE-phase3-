using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public float invincibilityTime = 0.5f;   // time after getting hit where you can't be hit again

    [Header("Knockback")]
    public float knockbackForceX = 8f;       // horizontal push
    public float knockbackForceY = 5f;       // vertical push
    public float knockbackDuration = 0.2f;   // how long movement is disabled

    int currentHealth;
    float lastHitTime;

    Rigidbody2D rb;
    Animator anim;

    bool isKnockedback;
    float knockbackEndTime;

    public int CurrentHealth => currentHealth;
    public bool IsKnockedback => isKnockedback;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isKnockedback && Time.time >= knockbackEndTime)
        {
            isKnockedback = false;
        }
    }

    // Old signature if you need it somewhere else
    public void TakeDamage(int amount)
    {
        TakeDamage(amount, Vector2.left);
    }

    // New signature with knockback direction
    public void TakeDamage(int amount, Vector2 hitDirection)
    {
        // Prevent taking damage too fast
        if (Time.time < lastHitTime + invincibilityTime)
            return;

        lastHitTime = Time.time;
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        // Trigger hit animation
        if (anim != null)
        {
            anim.SetTrigger("Hit");
        }

        // Apply knockback
        if (rb != null)
        {
            isKnockedback = true;
            knockbackEndTime = Time.time + knockbackDuration;

            // Clear current velocity so knockback is clean
            rb.velocity = Vector2.zero;

            if (hitDirection == Vector2.zero)
                hitDirection = Vector2.left;

            hitDirection.Normalize();
            Vector2 force = new Vector2(hitDirection.x * knockbackForceX, knockbackForceY);

            rb.AddForce(force, ForceMode2D.Impulse);
        }

        Debug.Log("Player took damage. Current health = " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died, reloading scene...");
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }
}
