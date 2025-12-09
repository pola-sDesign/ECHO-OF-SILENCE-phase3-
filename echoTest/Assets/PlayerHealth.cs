using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    // Respawn point
    public Vector3 respawnPoint;

    void Start()
    {
        currentHealth = maxHealth;

        // Default spawn point = player's starting position
        respawnPoint = transform.position;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("PLAYER HP = " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("PLAYER DIED!");
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        // (optional) add death animation delay
        yield return new WaitForSeconds(0.5f);

        // Respawn player
        transform.position = respawnPoint;

        // Restore HP
        currentHealth = maxHealth;

        Debug.Log("PLAYER RESPAWNED!");
    }

    public void SetCheckpoint(Vector3 newPoint)
    {
        respawnPoint = newPoint;
        Debug.Log("Checkpoint Saved: " + respawnPoint);
    }
}
