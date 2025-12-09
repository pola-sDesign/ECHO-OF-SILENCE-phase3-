using UnityEngine;

public class DroneSearchLight : MonoBehaviour
{
    private DroneAI drone;

    private void Awake()
    {
        drone = GetComponentInParent<DroneAI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            drone.StartAttacking();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            drone.StopAttacking();
    }
}
