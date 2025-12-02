using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 4f;
    public float minZoom = 3f;
    public float maxZoom = 10f;
    private float targetZoom;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollData * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
    }
}