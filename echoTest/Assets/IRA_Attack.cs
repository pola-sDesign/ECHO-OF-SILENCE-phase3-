using System.Collections;
using UnityEngine;

public class IRA_Attack : MonoBehaviour
{
    public Transform firePoint;
    public LineRenderer laser;
    public float laserDuration = 5f;
    public float laserLength = 50f;
    public int damage = 10;

    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        laser.enabled = false;
    }

    public void StartLaserAttack()
    {
        StartCoroutine(LaserAttack());
    }

    IEnumerator LaserAttack()
    {
        laser.enabled = true;

        float timer = 0f;

        while (timer < laserDuration)
        {
            Vector3 endPoint;

            if (GetComponent<SpriteRenderer>().flipX)
                endPoint = firePoint.position - transform.right * laserLength;
            else
                endPoint = firePoint.position + transform.right * laserLength;

            laser.SetPosition(0, firePoint.position);
            laser.SetPosition(1, endPoint);

            timer += Time.deltaTime;
            yield return null;
        }

        laser.enabled = false;

        // EXACTLY 2 seconds until next attack
        FindObjectOfType<IRATest>().EnableAttackAfterDelay(2f);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!laser.enabled) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
                hp.TakeDamage(damage);
        }
    }
}
