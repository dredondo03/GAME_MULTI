
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 6f;

    [Header("Salto")]
    public float jumpHeight = 2f;
    public float gravity = -20f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private CharacterController controller;

    private Vector3 velocity;
    private bool isGrounded;

    // private Animator animator; // Agregar arriba


    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // animator = GetComponentInChildren<Animator>();

        // if (animator == null)
        //     Debug.LogError("No se encontró Animator en " + gameObject.name + " ni en sus hijos");

    }

    void Update()
    {
        // Detectar suelo
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );

        // Evitar acumulación de gravedad
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movimiento
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        
        // animator.SetFloat("Speed", move.magnitude);
        // Debug.Log("Speed: " + move.magnitude + " | Animator: " + animator.gameObject.name);

        controller.Move(move * speed * Time.deltaTime);

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravedad
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}

