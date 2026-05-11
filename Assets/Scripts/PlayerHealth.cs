using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int vidaMaxima = 3;
    public int vidaActual;

    // Posición del checkpoint
    private Vector3 checkpointPosition;

    void Start()
    {
        vidaActual = vidaMaxima;

        // Guarda la posición inicial
        checkpointPosition = transform.position;

        Debug.Log("Sistema iniciado");
    }

    public void CambiarVida(int cantidad)
    {
        vidaActual += cantidad;

        if (vidaActual > vidaMaxima)
        {
            vidaActual = vidaMaxima;
        }

        if (vidaActual < 0)
        {
            vidaActual = 0;
        }

        Debug.Log("Vida actual: " + vidaActual);

        if (vidaActual <= 0)
        {
            Debug.Log("Jugador murió");
        }
    }

    // Guardar checkpoint
    public void SetCheckpoint(Vector3 nuevaPosicion)
    {
        checkpointPosition = nuevaPosicion;

        Debug.Log("Checkpoint guardado");
    }

    // Volver al checkpoint
    public void Respawn()
    {
        transform.position = checkpointPosition;

        Debug.Log("Jugador reapareció");
    }
}