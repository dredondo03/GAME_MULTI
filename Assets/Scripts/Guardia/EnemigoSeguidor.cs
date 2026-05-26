using UnityEngine;

public class EnemigoSeguidor : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 3.5f;
    
    [Tooltip("Qué tan rápido gira el enemigo hacia el jugador")]
    public float velocidadRotacion = 5.0f;

    private Transform objetivoPlayer;

    void Start()
    {
        GameObject jugador = GameObject.FindWithTag("Player");

        if (jugador != null)
        {
            objetivoPlayer = jugador.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró ningún objeto con el tag 'Player' en la escena.");
        }
    }

    void Update()
    {
        if (objetivoPlayer != null)
        {
            SeguirYRotarJugador();
        }
    }

    void SeguirYRotarJugador()
    {
        // 1. MOVIMIENTO
        Vector3 direccion = (objetivoPlayer.position - transform.position).normalized;
        transform.position += direccion * velocidad * Time.deltaTime;

        // 2. ROTACIÓN SUAVE CON CUATERNIONES (3D)
        // Ignoramos la diferencia en el eje Y para que el enemigo no se incline hacia arriba/abajo si el jugador salta
        Vector3 direccionPlana = new Vector3(direccion.x, 0, direccion.z);
        
        if (direccionPlana != Vector3.zero)
        {
            // Calcula la rotación exacta hacia el objetivo
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionPlana);
            
            // Interpola suavemente desde la rotación actual a la rotación objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
        }
    }
}