using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [Header("Configuración de Curación")]
    public int healAmount = 1;

    // Se activa cuando el jugador entra en el espacio del objeto (Trigger 3D)
    private void OnTriggerEnter(Collider other)
    {
        // Comprobamos si lo que entró en el Trigger es el jugador
        if (other.CompareTag("Player"))
        {
            // Buscamos el script de vida en el jugador
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Le sumamos la vida
                playerHealth.Heal(healAmount);

                // Destruimos el objeto del mapa para que desaparezca
                Destroy(gameObject);
            }
        }
    }
}