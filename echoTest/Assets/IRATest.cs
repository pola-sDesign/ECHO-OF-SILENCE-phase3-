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

        // Flip direction
        sr.flipX = (player.position.x < transform.position.x);

        if (!canAttack)
        {
            anim.SetFloat("Speed", 0f);
            return;
        }

        // Move until close
        if (distance > stopDistance)
        {
            anim.SetFloat("Speed", 1f);
            transform.position = Vector2.MoveTowards(
                transform.position,
                new Vector3(player.position.x, transform.position.y, 0),
                speed * Time.deltaTime
            );
        }
        else
        {
            // Stop walking
            anim.SetFloat("Speed", 0f);

            // Attack immediately
            canAttack = false;
            anim.SetTrigger("Attack");
        }
    }

    public void EnableAttackAfterDelay(float delay)
    {
        StartCoroutine(AttackDelayCoroutine(delay));
    }

    IEnumerator AttackDelayCoroutine(float d)
    {
        yield return new WaitForSeconds(d);
        canAttack = true;  // ready for new attack
    }
}
