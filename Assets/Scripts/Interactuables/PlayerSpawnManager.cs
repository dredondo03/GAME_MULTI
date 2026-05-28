using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    // Aquí se guardará la posición del último checkpoint tocado
    private Vector3 ultimoPuntoSeguro;

    void Start()
    {
        // Al iniciar el juego, el punto seguro por defecto será la posición 
        // en la que colocaste al jugador manualmente en la escena.
        ultimoPuntoSeguro = transform.position;
    }

    // Esta función la llama el Checkpoint cuando pasas sobre él
    public void ActualizarPuntoDeControl(Vector3 nuevaPosicion)
    {
        // Si el nuevo punto es diferente al que ya teníamos, lo actualizamos
        if (ultimoPuntoSeguro != nuevaPosicion)
        {
            ultimoPuntoSeguro = nuevaPosicion;
            Debug.Log("¡Punto de control guardado con éxito en la posición: " + ultimoPuntoSeguro);
        }
    }

    // Esta función la llama el plano del vacío cuando te caes
    public void TeletransportarAlPuntoSeguro()
    {
        Debug.Log("Teletransportando al jugador al último punto seguro...");

        // DETALLE IMPORTANTE PARA UNITY 6: 
        // Si tu personaje usa un componente 'CharacterController' para moverse, 
        // Unity bloquea la teletransportación directa por telemetría física. 
        // Para evitar errores, lo desactivamos un milisegundo, lo movemos y lo reactivamos.
        CharacterController controller = GetComponent<CharacterController>();
        
        if (controller != null)
        {
            controller.enabled = false;
            transform.position = ultimoPuntoSeguro;
            controller.enabled = true;
        }
        else
        {
            // Si te mueves con Rigidbody o transform normal, esta línea basta:
            transform.position = ultimoPuntoSeguro;
        }

        // Si tu personaje cae con físicas de Rigidbody, reiniciamos su velocidad 
        // para que no aparezca en el spawn cayendo a toda velocidad debido a la inercia.
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
        }
    }
}