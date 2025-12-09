using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public float CameraSpeed = 5f;

    [Header("Camera Area Bounds")]
    public BoxCollider2D boundsCollider;

    private float minX, maxX, minY, maxY;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        UpdateBounds();
    }

    public void SetBoundsCollider(BoxCollider2D newBounds)
    {
        boundsCollider = newBounds;
        UpdateBounds();
    }

    void UpdateBounds()
    {
        if (boundsCollider == null)
        {
            Debug.LogWarning("CameraFollow: No boundsCollider assigned!");
            return;
        }

        Bounds b = boundsCollider.bounds;

        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        minX = b.min.x + camHalfWidth;
        maxX = b.max.x - camHalfWidth;
        minY = b.min.y + camHalfHeight;
        maxY = b.max.y - camHalfHeight;
    }

    void FixedUpdate()
    {
        if (Target != null)
        {
            Vector2 newCamPosition = Vector2.Lerp(transform.position, Target.position, Time.deltaTime * CameraSpeed);

            float ClampX = Mathf.Clamp(newCamPosition.x, minX, maxX);
            float ClampY = Mathf.Clamp(newCamPosition.y, minY, maxY);

            transform.position = new Vector3(ClampX, ClampY, -10f);
        }
    }
}
