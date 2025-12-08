using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }


    public GameObject laser;

 public void StartLaser()
{
    laser.SetActive(true);
}

 public void StopLaser()
{
    laser.SetActive(false);
}

}

