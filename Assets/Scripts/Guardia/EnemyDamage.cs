using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [Header("Configuración de Daño")]
    public int damageAmount = 1;

    // Al usar "Is Trigger" en el Collider, se activa esta función tridimensional
    private void OnTriggerEnter(Collider other)
    {
        // ¿Lo que entró en el área del enemigo es el jugador?
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}