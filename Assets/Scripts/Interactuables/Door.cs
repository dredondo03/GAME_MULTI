// Door.cs
// Adjuntar a: cada Puerta en la escena
// El jugador presiona 'E' al acercarse para intentar abrirla

using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Llave Requerida")]
    public string requiredKeyID = "key_main_exit"; // ID de la llave que abre esta puerta

    [Header("Configuración de Apertura")]
    public float interactRange = 2.5f;     // Distancia para poder interactuar
    public float openAngle = 90f;          // Grados que rota al abrirse
    public float openSpeed = 3f;           // Velocidad de apertura
    public KeyCode interactKey = KeyCode.E;

    private bool isOpen = false;           // Estado de la puerta
    private bool isUnlocked = false;       // Si ya fue desbloqueada
    private Quaternion closedRotation;     // Rotación cerrada (inicial)
    private Quaternion openRotation;       // Rotación abierta (objetivo)
    private Transform playerTransform;
    private PlayerInventory playerInventory;

    void Start()
    {
        // Guardar la rotación inicial como "cerrada"
        closedRotation = transform.rotation;
        // Calcular la rotación "abierta" como una rotación adicional en Y
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        // Buscar al jugador en la escena
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerInventory = playerTransform.GetComponent<PlayerInventory>();
    }

    void Update()
    {
        // Verificar distancia al jugador
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist > interactRange) return; // Demasiado lejos, ignorar

        // Detectar tecla de interacción
        if (Input.GetKeyDown(interactKey))
        {
            TryOpen();
        }

        // Animar la apertura/cierre suavemente con Lerp
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, openSpeed * Time.deltaTime);
    }

    void TryOpen()
    {
        if (isOpen) return; // Ya está abierta

        if (isUnlocked || playerInventory.HasKey(requiredKeyID))
        {
            // Tiene la llave: abrir
            isOpen = true;
            isUnlocked = true; // Queda desbloqueada para siempre
            playerInventory.RemoveKey(requiredKeyID); // Consumir la llave
            Debug.Log("[Puerta] ¡Abierta con llave!");
        }
        else
        {
            // No tiene la llave
            Debug.Log("[Puerta] Necesitas la llave correcta.");
            // Aquí podrías reproducir un sonido de "puerta bloqueada"
        }
    }
}