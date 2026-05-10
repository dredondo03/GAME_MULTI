using UnityEngine;

public class VidaMinus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto tiene el tag Player
        if (other.CompareTag("Player"))
        {
            // Busca el script PlayerHealth
            PlayerHealth player =
                other.GetComponent<PlayerHealth>();

            // Resta 1 vida
            player.CambiarVida(-1);

            // Destruye el objeto
            Destroy(gameObject);
        }
    }
}