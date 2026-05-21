using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;

    private Rigidbody rb;
    private Animator animator; // ← Agregar esto

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // ← Agregar esto
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveZ);

        if (movement.sqrMagnitude > 0.001f)
        {
            Vector3 moveDirection = movement.normalized;
            rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));

            animator.SetFloat("Speed", movement.magnitude); // ← Moviéndose
        }
        else
        {
            animator.SetFloat("Speed", 0f); // ← Quieto
        }
    }
}