using UnityEngine;

public class VoidDeath : MonoBehaviour
{
    [Header("Configuración de Penalización")]
    public int dañoPorCaida = 1;

    // Se activa cuando el jugador cae y atraviesa este plano
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Le quitamos una vida usando el script que ya tenías creado
            PlayerHealth vidaJugador = other.GetComponent<PlayerHealth>();
            if (vidaJugador != null)
            {
                vidaJugador.TakeDamage(dañoPorCaida);
            }

            // 2. Le ordenamos al jugador que regrese al último punto seguro
            PlayerSpawnManager spawnManager = other.GetComponent<PlayerSpawnManager>();
            if (spawnManager != null)
            {
                spawnManager.TeletransportarAlPuntoSeguro();
            }
        }
    }
}