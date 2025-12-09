using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    public float knockbackForce = 7f;
    public float knockbackDuration = 0.2f;

    private Rigidbody2D rb;
    private bool isKnocked = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyKnockback(Transform attacker)
    {
        if (isKnocked) return;

        isKnocked = true;

        Vector2 dir = (transform.position - attacker.position).normalized;

        rb.velocity = Vector2.zero;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);

        Invoke(nameof(EndKnockback), knockbackDuration);
    }

    void EndKnockback()
    {
        isKnocked = false;
        rb.velocity = Vector2.zero;
    }
}

