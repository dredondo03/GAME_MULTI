// HidingSystem.cs
// Adjuntar a: Snake_Player
// Requiere: SnakeMovement.cs en el mismo GameObject

using UnityEngine;

public class HidingSystem : MonoBehaviour
{
    [Header("Configuración")]
    public float hideRange = 2f;              // Distancia máxima para activar escondite
    public KeyCode hideKey = KeyCode.E;        // Tecla para esconderse/salir
    public LayerMask hideSpotLayer;            // Layer de los objetos "escondite"

    [Header("Cámara al Esconderse")]
    public Transform cameraTransform;          // Cámara del jugador
    public Vector3 hiddenCameraOffset = new Vector3(0, 0, 0); // Posición local de la cámara dentro del escondite

    // Estado público (el guardia y el movimiento lo leen)
    [HideInInspector] public bool isHiding = false;
    [HideInInspector] public Transform currentHideSpot = null; // El escondite activo

    // Referencias internas
    private Vector3 originalCameraLocalPos;    // Posición original de la cámara
    private Collider playerCollider;
    private Rigidbody rb;
    private GuardAI guardAI;                    // Para notificarle al guardia

    void Start()
    {
        playerCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        originalCameraLocalPos = cameraTransform.localPosition;

        // Buscar al guardia en la escena para notificarle eventos
        guardAI = FindFirstObjectByType<GuardAI>();
    }

    void Update()
    {
        if (Input.GetKeyDown(hideKey))
        {
            if (isHiding)
                ExitHideSpot();  // Si ya estaba escondido, salir
            else
                TryHide();       // Si no, intentar esconderse
        }
    }

    void TryHide()
    {
        // Buscar todos los colliders dentro del radio de escondite
        Collider[] nearby = Physics.OverlapSphere(transform.position, hideRange, hideSpotLayer);

        if (nearby.Length == 0) return; // No hay escondite cerca

        // Usar el más cercano
        Transform closest = nearby[0].transform;
        float minDist = Vector3.Distance(transform.position, closest.position);
        foreach (var col in nearby)
        {
            float d = Vector3.Distance(transform.position, col.transform.position);
            if (d < minDist) { minDist = d; closest = col.transform; }
        }

        EnterHideSpot(closest);
    }

    void EnterHideSpot(Transform spot)
    {
        isHiding = true;
        currentHideSpot = spot;

        // Teletransportar al jugador dentro del escondite (colisión desactivada)
        playerCollider.enabled = false;
        rb.isKinematic = true; // Desactivar física para que no caiga
        transform.position = spot.position; // Mover el cuerpo al escondite

        // Mover la cámara al interior del objeto (para efecto visual)
        cameraTransform.localPosition = hiddenCameraOffset;
        cameraTransform.localRotation = Quaternion.identity; // Mirar al frente dentro del escondite
    }

    public void ExitHideSpot()
    {
        isHiding = false;

        // Restaurar física
        playerCollider.enabled = true;
        rb.isKinematic = false;

        // Mover al jugador ligeramente al lado del escondite para no quedar dentro de la geometría
        transform.position = currentHideSpot.position + currentHideSpot.right * 1.2f;

        // Restaurar posición de la cámara
        cameraTransform.localPosition = originalCameraLocalPos;

        currentHideSpot = null;
    }

    // Método llamado por el guardia para sacar al jugador a la fuerza
    public void ForceExit()
    {
        if (isHiding) ExitHideSpot();
    }

    // Dibuja el radio de escondite en el editor (solo visible en Scene View)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, hideRange);
    }
}