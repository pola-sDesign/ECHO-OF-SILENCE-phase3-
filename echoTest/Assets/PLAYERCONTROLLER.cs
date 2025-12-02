using System.Collections;
using UnityEngine;

public class PLAYERCONTROLLER : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 10f;
    public KeyCode Spacebar;
    public KeyCode L;
    public KeyCode R;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    private bool isGrounded;
    private Animator anim;

    [Header("Laser System")]
    public Transform firePoint;
    public LineRenderer lineRenderer;
    public float testBeamLength = 5f;
    public float testBeamTime = 0.05f;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        
        // Jump
        if (Input.GetKeyDown(Spacebar) && isGrounded)
            Jump();

        // Move left
        if (Input.GetKey(L))
        {
            GetComponent<Rigidbody2D>().velocity =
                new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y);

            GetComponent<SpriteRenderer>().flipX = true;
        }
        // Move right
        else if (Input.GetKey(R))
        {
            GetComponent<Rigidbody2D>().velocity =
                new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);

            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            // no movement
            GetComponent<Rigidbody2D>().velocity =
                new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        }

        // Shoot laser - right mouse click
        if (Input.GetMouseButtonDown(1))
        {
            anim.SetTrigger("Shoot");
            StartCoroutine(FireLaser());
        }

        // Animator values
        anim.SetFloat("Speed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        anim.SetFloat("Height", GetComponent<Rigidbody2D>().velocity.y);
        anim.SetBool("Grounded", isGrounded);
    }

    void Jump()
    {
        GetComponent<Rigidbody2D>().velocity =
            new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpHeight);
    }

    IEnumerator FireLaser()
    {
        lineRenderer.enabled = true;

        // Start point of laser
        Vector3 startPos = firePoint.position;

        // Direction depends on character facing
        Vector3 dir = transform.right;

        // If flipped, invert direction
        if (GetComponent<SpriteRenderer>().flipX)
            dir = -transform.right;

        // End point
        Vector3 endPos = startPos + dir * testBeamLength;

        // Assign positions
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

        yield return new WaitForSeconds(testBeamTime);

        lineRenderer.enabled = false;
    }
}
