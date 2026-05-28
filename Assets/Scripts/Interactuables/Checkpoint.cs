using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Se activa cuando el jugador entra en el área del Checkpoint
    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si lo que tocó el checkpoint es el jugador
        if (other.CompareTag("Player"))
        {
            // Buscamos un script en el jugador que administre sus caídas y reapariciones
            PlayerSpawnManager spawnManager = other.GetComponent<PlayerSpawnManager>();

            if (spawnManager != null)
            {
                // Le enviamos la posición exacta de este objeto para que la guarde
                // Le sumamos un pequeño ajuste en el eje Y (vertical) para que el jugador no aparezca enterrado en el suelo
                Vector3 posicionDeAparicion = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);
                
                spawnManager.ActualizarPuntoDeControl(posicionDeAparicion);
            }
        }
    }
}