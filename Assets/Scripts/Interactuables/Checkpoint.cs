using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerSpawnManager spawnManager = other.GetComponent<PlayerSpawnManager>();

            if (spawnManager != null)
            {
                Vector3 posicionDeAparicion = new Vector3(
                    transform.position.x, 
                    transform.position.y + 1.0f, 
                    transform.position.z
                );
                spawnManager.ActualizarPuntoDeControl(posicionDeAparicion);

                // Mostrar mensaje en pantalla
                if (CheckpointUI.Instance != null)
                    CheckpointUI.Instance.ShowCheckpointMessage();
            }
        }
    }
}