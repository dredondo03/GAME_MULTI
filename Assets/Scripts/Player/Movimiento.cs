using UnityEngine;

public class Movimiento : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    private CharacterController controller;
    private Vector3 direccionMovimiento;

    [Header("Físicas de Gravedad")]
    private float gravedad = -9.81f;
    private Vector3 velocidadVertical;

    [Header("Detección de Suelo")]
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;
    private bool isGrounded;

    void Start()
    {
        // Obtenemos el Character Controller asignado en el objeto
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Detectar si el personaje está tocando el suelo
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
        else
        {
            // Si no hay un GroundCheck asignado, usamos la propiedad interna del controller
            isGrounded = controller.isGrounded;
        }

        // 2. Reiniciar la velocidad de caída si ya estamos estables en el suelo
        if (isGrounded && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f; // Un valor pequeño negativo para mantenerlo pegado al piso
        }

        // 3. Capturar teclas WASD / Flechas
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        direccionMovimiento = new Vector3(moveX, 0f, moveZ).normalized;

        // 4. Mover al personaje en los ejes X y Z (Horizontal)
        if (direccionMovimiento.magnitude >= 0.1f)
        {
            controller.Move(direccionMovimiento * moveSpeed * Time.deltaTime);
        }

        // 5. Aplicar gravedad constantemente en el eje Y (Vertical)
        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}