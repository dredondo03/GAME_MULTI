// SnakeMovement.cs
// Adjuntar a: Snake_Player
// Requiere: Rigidbody, CapsuleCollider, Camera hijo

using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [Header("Movimiento Base")]
    public float walkSpeed = 3f;         // Velocidad de deslizamiento normal
    public float sprintSpeed = 6f;       // Velocidad al correr (sprint)
    public float currentSpeed;           // Velocidad activa en este frame
    public bool debugMode = true;       // Activa mensajes de depuración en la consola

    [Header("Estamina")]
    public float maxStamina = 100f;      // Estamina máxima
    public float stamina;                // Estamina actual
    public float staminaDrain = 20f;     // Cuánta estamina se consume por segundo al correr
    public float staminaRegen = 10f;     // Cuánta estamina se recupera por segundo al no correr

    [Header("Impulso (Salto de Serpiente)")]
    public float lungeForce = 5f;        // Fuerza del impulso hacia adelante
    public float lungeCooldown = 1.5f;   // Segundos entre impulsos
    private float lungeTimer;            // Contador del cooldown
    private bool isGrounded;             // Si la serpiente toca el suelo

    [Header("Cámara")]
    public float mouseSensitivity = 2f;  // Sensibilidad del mouse
    public Transform cameraTransform;    // Referencia a la cámara hija
    private float xRotation = 0f;        // Rotación acumulada en X (mirar arriba/abajo)

    // Referencias internas
    private Rigidbody rb;
    private HidingSystem hidingSystem;   // Referencia al sistema de escondite

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hidingSystem = GetComponent<HidingSystem>();
        stamina = maxStamina;
        lungeTimer = 0f;

        // Ocultar y bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (debugMode)
            Debug.Log($"[SnakeMovement] Start | rb={(rb != null ? "OK" : "MISSING")} | hidingSystem={(hidingSystem != null ? "OK" : "NONE")} | maxStamina={maxStamina}");
    }

    void Update()
    {
        // Si el jugador está escondido, no puede moverse
        if (hidingSystem != null && hidingSystem.isHiding)
        {
            if (debugMode) Debug.Log("[SnakeMovement] Update | estado: escondido");
            return;
        }

        if (debugMode)
            Debug.Log($"[SnakeMovement] Update | inputH={Input.GetAxisRaw("Horizontal")} inputV={Input.GetAxisRaw("Vertical")} sprint={Input.GetKey(KeyCode.LeftShift)} stamina={stamina:F1} lungeTimer={lungeTimer:F2}");

        HandleCameraRotation();  // Girar la vista con el mouse
        HandleStamina();         // Calcular estamina y velocidad
        HandleLunge();           // Detectar y aplicar impulso
    }

    void FixedUpdate()
    {
        // Si está escondido, no mover el rigidbody
        if (hidingSystem != null && hidingSystem.isHiding) return;

        HandleMovement();        // Mover el Rigidbody (en FixedUpdate para física)
    }

    void HandleCameraRotation()
    {
        // Leer entrada del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotar el cuerpo completo en el eje Y (izquierda/derecha)
        transform.Rotate(Vector3.up * mouseX);

        // Limitar la rotación vertical de la cámara entre -80 y +80 grados
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleStamina()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && stamina > 0f;

        if (isSprinting)
        {
            // Drenar estamina mientras se corre
            stamina -= staminaDrain * Time.deltaTime;
            stamina = Mathf.Max(stamina, 0f); // No bajar de 0
            currentSpeed = sprintSpeed;
        }
        else
        {
            // Regenerar estamina cuando no se corre
            stamina += staminaRegen * Time.deltaTime;
            stamina = Mathf.Min(stamina, maxStamina); // No subir del máximo
            currentSpeed = walkSpeed;
        }
    }

    void HandleMovement()
    {
        // Leer ejes WASD / flechas
        float moveX = Input.GetAxisRaw("Vertical"); // A/D
        float moveZ = Input.GetAxisRaw("Horizontal");   // W/S

        // Construir dirección de movimiento relativa a la orientación del jugador
        Vector3 moveDir = transform.right * moveX + transform.forward * moveZ;
        moveDir.Normalize(); // Evitar que la diagonal sea más rápida

        // Calcular velocidad objetivo
        Vector3 targetVelocity = moveDir * currentSpeed;

        // Conservar la velocidad vertical (gravedad) del Rigidbody
        targetVelocity.y = rb.linearVelocity.y;

        if (debugMode)
            Debug.Log($"[SnakeMovement] HandleMovement | moveDir={moveDir} currentSpeed={currentSpeed} targetVelocity={targetVelocity}");

        // Aplicar velocidad al Rigidbody
        rb.linearVelocity = targetVelocity;
    }

    void HandleLunge()
    {
        // Reducir el cooldown con el tiempo
        if (lungeTimer > 0f)
            lungeTimer -= Time.deltaTime;

        // Detectar si la serpiente está en el suelo (raycast corto hacia abajo)
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.4f);

        if (debugMode)
            Debug.Log($"[SnakeMovement] HandleLunge | grounded={isGrounded} lungeTimer={lungeTimer:F2}");

        // Presionar Espacio para el impulso, solo si está en el suelo y el cooldown pasó
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && lungeTimer <= 0f)
        {
            // Impulso hacia adelante y ligeramente hacia arriba
            Vector3 lungeDirection = transform.forward + Vector3.up * 0.4f;
            rb.AddForce(lungeDirection.normalized * lungeForce, ForceMode.Impulse);
            lungeTimer = lungeCooldown; // Reiniciar cooldown

            if (debugMode)
                Debug.Log($"[SnakeMovement] Lunge activated | direction={lungeDirection.normalized} force={lungeForce}");
        }
    }

    // Getter público para que la UI pueda leer la estamina
    public float GetStaminaPercent() => stamina / maxStamina;
}