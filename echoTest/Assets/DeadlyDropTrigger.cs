using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyDropTrigger : MonoBehaviour
{
    // لو عايز تلعب صوت أو VFX عند الموت حطه هنا
    // public AudioClip deathSfx;
    // public GameObject deathVFX;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph != null)
        {
            // kill instantly by dealing remaining HP
            ph.TakeDamage(ph.currentHealth);
        }

        // لو عايز تعمل صوت أو مؤثر:
        // if (deathSfx) AudioSource.PlayClipAtPoint(deathSfx, transform.position);
        // if (deathVFX) Instantiate(deathVFX, other.transform.position, Quaternion.identity);
    }
}
