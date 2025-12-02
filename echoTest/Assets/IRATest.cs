using UnityEngine;

public class IRATest : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float stopDistance = 3f;

    private Animator anim;
    private SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        // Send distance to Animator
        anim.SetFloat("Distance", distance);

        // Flip
        if (player.position.x < transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;

        // Move towards player if far enough
        if (distance > stopDistance)
        {
            anim.Play("IRA-RUNNING");
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            anim.Play("IRA-IDEL");
        }
    }
}
