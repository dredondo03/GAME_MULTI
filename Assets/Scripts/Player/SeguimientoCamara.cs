using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{
    private Transform objetivo;

    [Header("Configuración de Distancia")]
    public Vector3 desfase = new Vector3(0, 5, -6);

    [Header("Suavizado")]
    public float suavizado = 5.0f;

    [Header("Colisión con paredes")]
    public float minDistancia = 1.5f;

    void Start()
    {
        GameObject jugador = GameObject.FindWithTag("Player");

        if (jugador != null)
            objetivo = jugador.transform;
        else
            Debug.LogError("¡No se encontró ningún objeto con la etiqueta 'Player'!");
    }

    void LateUpdate()
    {
        if (objetivo == null) return;

        // El desfase ahora sigue la dirección a la que mira el jugador
        Vector3 posicionDeseada = objetivo.position + objetivo.TransformDirection(desfase);
        Vector3 direccion = posicionDeseada - objetivo.position;

        int capaJugador = objetivo.gameObject.layer;
        int mascara = ~(1 << capaJugador);

        RaycastHit hit;
        if (Physics.Raycast(objetivo.position, direccion.normalized, out hit, direccion.magnitude, mascara))
        {
            float distanciaSegura = Mathf.Max(hit.distance - 0.3f, minDistancia);
            posicionDeseada = objetivo.position + direccion.normalized * distanciaSegura;
        }

        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado * Time.deltaTime);
        transform.LookAt(objetivo.position + Vector3.up * 1.2f);
    }
}