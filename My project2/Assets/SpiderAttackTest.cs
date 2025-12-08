using UnityEngine;

public class SpiderAttackTest : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        Debug.Log("SpiderAttackTest Start. Animator = " + anim);

        // Try to play the attack at the very start
        anim.SetTrigger("isAttacking");
        Debug.Log("Trigger isAttacking sent.");
    }
}
