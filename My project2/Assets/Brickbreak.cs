using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brickbreak : MonoBehaviour
{
    private SpriteRenderer sr;
    private Collider2D col;

    public Sprite explodedBlock;      // ONE broken sprite
    public float destroyDelay = 0.2f; // time before it disappears

    void Awake()
    {
        sr  = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // only react to player
        if (!other.gameObject.CompareTag("Player")) return;

        // contact point must be ABOVE the platform center = player is standing on it
        ContactPoint2D contact = other.GetContact(0);
        if (contact.point.y > transform.position.y)
        {
            Break();
        }
    }

    void Break()
    {
        // change to broken sprite
        if (explodedBlock != null)
            sr.sprite = explodedBlock;

        // turn off collider so player falls through
        if (col != null)
            col.enabled = false;

        // destroy the object after a short delay
        Destroy(gameObject, destroyDelay);
    }
}
