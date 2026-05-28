using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{
    private Transform objetivo; // El jugador con el tag "Player"

    [Header("Configuración de Distancia")]
    public Vector3 desfase = new Vector3(0, 5, -6); // Distancia (X, Y, Z) a la que la cámara estará del jugador

    [Header("Suavizado")]
    public float suavizado = 5.0f; // Qué tan "elástica" es la cámara al seguir al jugador

    void Start()
    {
        // Busca automáticamente al personaje por su etiqueta (Tag)
        GameObject jugador = GameObject.FindWithTag("Player");
        
        if (jugador != null)
        {
            objetivo = jugador.transform;
        }
        else
        {
            Debug.LogError("¡No se encontró ningún objeto con la etiqueta 'Player'! Por favor, asígnala en el Inspector.");
        }
    }

    // LateUpdate se ejecuta justo después del Update del jugador. 
    // Esto evita que la cámara tiemble (jittering) mientras el personaje se mueve.
    void LateUpdate()
    {
        if (objetivo == null) return;

        // Posición ideal a la que la cámara debería ir
        Vector3 posicionDeseada = objetivo.position + desfase;
        
        // Viaje suave desde la posición actual de la cámara a la posición deseada
        Vector3 posicionSuave = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
        
        // Aplicamos la posición
        transform.position = posicionSuave;

        // Obligamos a la cámara a mirar siempre hacia el jugador
        transform.LookAt(objetivo.position + Vector3.up * 1.2f); // Apunta un poco más arriba de los pies
    }
}