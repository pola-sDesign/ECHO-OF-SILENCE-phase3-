using UnityEngine;
using System.Collections;

public class IRATest : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float stopDistance = 3f;

    private Animator anim;
    private SpriteRenderer sr;

    private bool canAttack = true;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Flip to face player
        sr.flipX = (player.position.x < transform.position.x);

        // ============================================
        // 1) If she is on cooldown → STAY STILL unless 
        //    the player MOVES AWAY past stopDistance
        // ============================================
        if (!canAttack)
        {
            if (distance > stopDistance)
            {
                anim.SetFloat("Speed", 1f);
                MoveTowardsPlayer();
            }
            else
            {
                anim.SetFloat("Speed", 0f); // STOP beside player
            }
            return;
        }

        // ============================================
        // 2) If she can attack
        // ============================================
        if (distance > stopDistance)
        {
            anim.SetFloat("Speed", 1f);
            MoveTowardsPlayer();
        }
        else
        {
            anim.SetFloat("Speed", 0f);
            canAttack = false;
            anim.SetTrigger("Attack");
        }
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            new Vector3(player.position.x, transform.position.y, 0),
            speed * Time.deltaTime
        );
    }

    public void EnableAttackAfterDelay(float delay)
    {
        StartCoroutine(AttackDelayCoroutine(delay));
    }

    IEnumerator AttackDelayCoroutine(float d)
    {
        yield return new WaitForSeconds(d);
        canAttack = true;
    }
}
