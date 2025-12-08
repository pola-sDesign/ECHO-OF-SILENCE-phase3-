using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BridgeFall : MonoBehaviour
{
    public float rotateSpeed = 200f;
    private bool fall = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            fall = true;
        }
    }

    void Update()
    {
        if (fall)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.Euler(0, 0, -90),
                rotateSpeed * Time.deltaTime
            );
        }
    }
}

