using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class MegaKnightAI : MonoBehaviour
{
    [Header("References (if empty script will try to find by tag \"Player\")")]
    public Transform player;                 // assign in inspector or tag player "Player"
    public float walkSpeed = 2f;
    public float attackRange = 2f;
    public float defenseRange = 1f;
    public int attackDamage = 20;

    private Animator anim;
    private SpriteRenderer sr;

    private bool isAttacking = false;
    private bool isDefending = false;
    private bool facingRight = true;         // true = localScale.x > 0

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        // initial facing sign from localScale
        facingRight = transform.localScale.x > 0f;
    }

    void Start()
    {
        // if player not set, try to find one by tag "Player"
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return; // nothing to do

        float distance = Vector2.Distance(transform.position, player.position);

        // Face player using localScale (recommended over sr.flipX if animations or inspector flip cause issues)
        FacePlayer();

        // If currently attacking or defending, prevent movement (animation events will call EndAttack/EndDefense)
        if (isAttacking || isDefending)
        {
            // ensure animator Speed parameter = 0 so blend tree / locomotion knows we're stopped
            SetAnimatorSpeed(0f);
            return;
        }

        // Defense priority (closest)
        if (distance <= defenseRange)
        {
            StartDefense();
            return;
        }

        // Attack if in range
        if (distance <= attackRange)
        {
            StartAttack();
            return;
        }

        // Otherwise walk toward player's x (keep same y)
        MoveTowardPlayer();
    }

    // --- Movement / facing ---

    void FacePlayer()
    {
        if (player == null) return;
        bool shouldFaceRight = player.position.x > transform.position.x;
        if (shouldFaceRight != facingRight)
        {
            facingRight = shouldFaceRight;
            Vector3 s = transform.localScale;
            s.x = Mathf.Abs(s.x) * (facingRight ? 1f : -1f);
            transform.localScale = s;
            // make sure SpriteRenderer.flipX is OFF in inspector and not animated
            // sr.flipX = !facingRight; // don't use this if animation or inspector flip used
        }
    }

    void MoveTowardPlayer()
    {
        // walk animation param
        SetAnimatorSpeed(1f);

        Vector3 target = new Vector3(player.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, walkSpeed * Time.deltaTime);
    }

    void SetAnimatorSpeed(float val)
    {
        if (anim != null)
            anim.SetFloat("Speed", val);
    }

    // --- Attack / Defense control ---

    void StartAttack()
    {
        if (isAttacking || isDefending) return;
        isAttacking = true;
        // Use trigger (matches your earlier setup). Make sure Animator has a Trigger named "Attack".
        anim.SetTrigger("Attack");
        // Do NOT call EndAttack here — we rely on animation event EndAttack() or fallback below
    }

    void StartDefense()
    {
        if (isDefending || isAttacking) return;
        isDefending = true;
        anim.SetTrigger("Defense"); // Animator must have Trigger "Defense"
        // Prefer to end defense with animation event EndDefense() placed in Defense clip.
        // As a safety fallback, uncomment the line below (but prefer animation event):
        // Invoke(nameof(EndDefense), 1f);
    }

    // --- Called by Animation Events (add these events inside animation clips) ---

    // place an Animation Event at the hit frame -> function name: DealDamage
    public void DealDamage()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance > attackRange + 0.1f) return; // small tolerance

        // Try to find PlayerHealth and PlayerKnockback components on player
        var pHealth = player.GetComponent<PlayerHealth>();
        if (pHealth != null)
        {
            pHealth.TakeDamage(attackDamage);
        }

        var pKB = player.GetComponent<PlayerKnockback>();
        if (pKB != null)
        {
            pKB.ApplyKnockback(transform);
        }
    }

    // place an Animation Event at the last frame of attack -> function name: EndAttack
    public void EndAttack()
    {
        isAttacking = false;
        // reset animator flags if you used booleans - if using triggers, not necessary
        // anim.ResetTrigger("Attack");
    }

    // place an Animation Event at the last frame of defense -> function name: EndDefense
    public void EndDefense()
    {
        isDefending = false;
        // anim.ResetTrigger("Defense");
    }

    // Safety: if animation events are missing, you can also call EndAttack/EndDefense via code timer,
    // but animation events produce perfect sync with frames.
}
