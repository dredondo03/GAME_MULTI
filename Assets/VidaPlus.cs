using UnityEngine;

public class VidaPlus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto tiene el tag Player
        if (other.CompareTag("Player"))
        {
            // Busca el script PlayerHealth
            PlayerHealth player =
                other.GetComponent<PlayerHealth>();

            // Suma 1 vida
            player.CambiarVida(1);

            // Destruye el objeto de vida
            Destroy(gameObject);
        }
    }
}