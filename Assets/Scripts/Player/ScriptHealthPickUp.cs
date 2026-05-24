using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerHealth>(out var health))
            {
                health.AddLife(1); // agrega 1 vida (un corazón)
            }

            // prevenir triggers adicionales antes de destruir
            var col = GetComponent<Collider>();
            if (col != null) col.enabled = false;

            var rend = GetComponent<Renderer>();
            if (rend != null) rend.enabled = false;

            Destroy(gameObject);
        }
    }
}