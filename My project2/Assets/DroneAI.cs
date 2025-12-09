using UnityEngine;

public class DroneAI : MonoBehaviour
{
    [Header("References")]
    public Animator anim;

    [Header("Attack")]
    public float attackRate = 1.2f;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private bool playerDetected = false;
    private bool isDead = false;

    private void Update()
    {
        if (isDead) return;

        anim.SetBool("isAttacking", playerDetected);
    }

    public void StartAttacking()
    {
        if (isDead) return;
        playerDetected = true;
        InvokeRepeating(nameof(Fire), 0f, attackRate);
    }

    public void StopAttacking()
    {
        playerDetected = false;
        CancelInvoke(nameof(Fire));
    }

    private void Fire()
    {
        if (isDead || bulletPrefab == null || firePoint == null)
            return;

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    public void Die()
    {
        isDead = true;
        StopAttacking();
        anim.SetTrigger("Die");
        Destroy(gameObject, 1.2f); // match death animation length
    }
}
