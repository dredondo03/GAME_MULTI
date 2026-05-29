using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Configuración")]
    public float moveSpeed = 3f;

    void Update()
    {
        // Solo mueve la cámara mientras se mantenga presionado el clic derecho
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            transform.position += new Vector3(mouseX, mouseY, 0) * moveSpeed * Time.deltaTime;
        }
    }
}