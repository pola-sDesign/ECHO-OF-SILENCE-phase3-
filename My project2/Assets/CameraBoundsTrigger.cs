using UnityEngine;

public class CameraBoundsTrigger : MonoBehaviour
{
    public BoxCollider2D newBounds;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraFollow cam = Camera.main.GetComponent<CameraFollow>();

            if (cam != null && newBounds != null)
            {
                cam.SetBoundsCollider(newBounds);
            }
        }
    }
}
