using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Configuración")]
    public float rotacionSpeed = 100f; // Velocidad de rotación alrededor del jugador

    private Transform objetivo;
    private float distancia;

    void Start()
    {
        GameObject jugador = GameObject.FindWithTag("Player");
        if (jugador != null)
        {
            objetivo = jugador.transform;
            distancia = Vector3.Distance(transform.position, objetivo.position);
        }
    }

    void LateUpdate()
    {
        if (objetivo == null) return;

        // Solo rota mientras se mantenga clic derecho
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");

            // Rota la cámara alrededor del jugador en el eje Y
            transform.RotateAround(objetivo.position, Vector3.up, mouseX * rotacionSpeed * Time.deltaTime);
        }
    }
}