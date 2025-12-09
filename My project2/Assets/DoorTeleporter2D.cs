using System.Collections;
using UnityEngine;

public class DoorTeleporter2D : MonoBehaviour
{
    [Header("Teleport Settings")]
    public DoorTeleporter2D connectedDoor;   // Assign the OTHER door here
    public Transform exitPoint;             // Child transform on THIS door
    public KeyCode useKey = KeyCode.W;      // Key to enter (W or UpArrow)

    [Header("Camera")]
    public BoxCollider2D cameraBoundsAfter; // Bounds of the room you teleport INTO

    [Header("Animation")]
    private Animator animator;
    private bool playerInRange = false;
    private Transform player;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.transform;
            SetOpen(true);          // open when player comes close
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            SetOpen(false);         // close when player goes away
        }
    }

    void Update()
    {
        // Player presses key while standing in the door
        if (playerInRange && Input.GetKeyDown(useKey))
        {
            StartCoroutine(TeleportPlayer());
        }
    }

    IEnumerator TeleportPlayer()
    {
        if (connectedDoor == null || player == null) yield break;

        // Optional: disable player movement script while teleporting
        // Replace "PlayerMovement" with the actual movement script name if you want
        // var controller = player.GetComponent<PlayerMovement>();
        // if (controller != null) controller.enabled = false;

        // Small delay to let open animation play
        yield return new WaitForSeconds(0.1f);

        // Move player to the other door's exit point
        player.position = connectedDoor.exitPoint.position;

        // Open the target door briefly
        connectedDoor.SetOpen(true);

        // --- NEW: update camera bounds to new room ---
        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow != null && cameraBoundsAfter != null)
        {
            camFollow.SetBoundsCollider(cameraBoundsAfter);
        }

        // if (controller != null) controller.enabled = true;
    }

    public void SetOpen(bool open)
    {
        if (animator != null)
        {
            animator.SetBool("IsOpen", open);
        }
    }
}
