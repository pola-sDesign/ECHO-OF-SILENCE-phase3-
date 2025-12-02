using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float cameraSpeed;
    public float minX, maxX, minY, maxY;

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 newPosition = Vector2.Lerp(transform.position, target.position, cameraSpeed * Time.deltaTime);
            float clampX = Mathf.Clamp(newPosition.x, minX, maxX);
            float clampY = Mathf.Clamp(newPosition.y, minY, maxY);
            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }
}