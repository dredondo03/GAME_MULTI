using UnityEngine;
using UnityEngine.AI; // Obligatorio para NavMesh

public class EnemigoGuardian : MonoBehaviour
{
    private Transform objetivoPlayer;
    private NavMeshAgent agente;

    [Header("Configuración de Alerta")]
    [Tooltip("La distancia en metros a la que el enemigo puede verte")]
    public float rangoDeVision = 8.0f;
    
    // Estados del enemigo
    private bool jugadorDetectado = false;
    private bool jugadorEscondido = false;
    private Vector3 ultimaPosicionConocida;
    private bool buscandoUltimaPosicion = false;
    private bool alejandose = false;

    [Header("Comportamiento de Búsqueda")]
    public float radioDeAlejamiento = 6.0f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            objetivoPlayer = jugador.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró ningún objeto con el tag 'Player' en la escena.");
        }

        // Al iniciar, como es estático, le decimos al NavMesh que no se mueva a ningún lado
        if (agente != null)
        {
            agente.isStopped = true; 
        }
    }

    void Update()
    {
        if (objetivoPlayer == null || agente == null) return;

        // Calculamos la distancia actual entre este enemigo y tu personaje
        float distanciaAlJugador = Vector3.Distance(transform.position, objetivoPlayer.position);

        // Si el jugador no ha sido detectado aún, el enemigo vigila en modo estático
        if (!jugadorDetectado)
        {
            // En cuanto el jugador entra en el rango de visión, se activa la persecución
            if (distanciaAlJugador <= rangoDeVision && !jugadorEscondido)
            {
                jugadorDetectado = true;
                agente.isStopped = false; // Le permitimos caminar al NavMesh
                Debug.Log("¡El Guardia te ha visto! Iniciando persecución.");
            }
            return; // Mientras no te vea, frena el código aquí para quedarse quieto
        }

        // ==========================================
        // LÓGICA DE MOVIMIENTO (Igual al primer enemigo)
        // ==========================================
        
        // ESTADO 1: Te vio y te persigue en tiempo real
        if (!jugadorEscondido)
        {
            agente.SetDestination(objetivoPlayer.position);
        }
        // ESTADO 2: Te escondiste, va a revisar el escondite
        else if (buscandoUltimaPosicion)
        {
            agente.SetDestination(ultimaPosicionConocida);

            if (!agente.pathPending && agente.remainingDistance <= 0.8f)
            {
                buscandoUltimaPosicion = false;
                CalcularPuntoAleatorioValido();
            }
        }
        // ESTADO 3: No te encontró y se aleja a patrullar
        else if (alejandose)
        {
            if (!agente.pathPending && agente.remainingDistance <= 0.8f)
            {
                alejandose = false;
                jugadorDetectado = false; // Vuelve a su estado de alerta estático en la nueva zona
                agente.isStopped = true;  // Se queda quieto de nuevo
                Debug.Log("El guardia perdió el rastro y volvió a ponerse en guardia estática.");
            }
        }
    }

    void CalcularPuntoAleatorioValido()
    {
        Vector2 desplazamientoAleatorio = Random.insideUnitCircle.normalized * radioDeAlejamiento;
        Vector3 puntoTentativo = new Vector3(
            transform.position.x + desplazamientoAleatorio.x,
            transform.position.y,
            transform.position.z + desplazamientoAleatorio.y
        );

        NavMeshHit hit;
        if (NavMesh.SamplePosition(puntoTentativo, out hit, 3.0f, NavMesh.AllAreas))
        {
            agente.SetDestination(hit.position);
            alejandose = true;
        }
    }

    // Funciones de comunicación con el script PlayerHide
    public void PerderDeVistaALJugador(Vector3 posicionDelJugador)
    {
        if (!jugadorDetectado) return; // Si nunca te vio, ignora el evento
        jugadorEscondido = true;
        buscandoUltimaPosicion = true;
        alejandose = false;
        ultimaPosicionConocida = posicionDelJugador;
    }

    public void VolverAVerAlJugador()
    {
        jugadorEscondido = false;
        buscandoUltimaPosicion = false;
        alejandose = false;
    }

    // DIBUJAR EL RANGO EN LA VENTANA DE ESCENA
    // Esto es puramente visual para ayudarte a ti como desarrollador a ver el radio en 3D
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoDeVision);
    }
}