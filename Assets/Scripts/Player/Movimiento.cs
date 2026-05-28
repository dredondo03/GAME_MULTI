using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento Horizontal")]
    public float velocidad = 5.0f;
    public float velocidadRotacion = 10.0f; // <- NUEVO: Controla qué tan rápido gira el personaje
    private CharacterController controller;

    [Header("Gravedad y Salto")]
    public float fuerzaSalto = 3.0f;
    public float gravedad = -9.81f;
    private Vector3 velocidadVertical; 

    [Header("Detección de Suelo Profesional")]
    [Tooltip("Arrastra aquí un objeto vacío posicionado exactamente en los pies del jugador")]
    public Transform detectorSuelo;
    
    [Tooltip("Radio de la esfera de detección (0.2 o 0.3 suele ser ideal)")]
    public float radioSuelo = 0.3f;
    
    [Tooltip("Capa (Layer) asignada a las plataformas y al suelo de tu mapa")]
    public LayerMask capaSuelo;

    private bool estaEnElSuelo;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. DETECCIÓN MANUAL DE SUELO
        estaEnElSuelo = Physics.CheckSphere(detectorSuelo.position, radioSuelo, capaSuelo);

        // 2. MOVIMIENTO HORIZONTAL
        float moverHorizontal = Input.GetAxis("Horizontal");
        float moverVertical = Input.GetAxis("Vertical");

        // Creamos el vector de dirección basado en los inputs
        Vector3 direccionMovimiento = new Vector3(moverHorizontal, 0, moverVertical).normalized;

        // Si el jugador está presionando alguna tecla de movimiento...
        if (direccionMovimiento.magnitude >= 0.1f)
        {
            // NUEVO: Calculamos la rotación hacia la dirección del movimiento
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
            
            // NUEVO: Giramos suavemente desde la rotación actual a la rotación objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);

            // Movemos al jugador hacia adelante (su "adelante" local ahora coincide con la dirección)
            controller.Move(direccionMovimiento * velocidad * Time.deltaTime);
        }

        // 3. CONTROL DE GRAVEDAD
        if (estaEnElSuelo && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f; 
        }

        // 4. CONTROL DEL SALTO
        if (Input.GetButtonDown("Jump") && estaEnElSuelo)
        {
            velocidadVertical.y = Mathf.Sqrt(fuerzaSalto * -2f * gravityVal());
        }

        // Aplicamos la gravedad
        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }

    // Método auxiliar para evitar conflictos de signos en la fórmula matemática del salto
    private float gravityVal() => gravedad > 0 ? -gravedad : gravedad;

    void OnDrawGizmosSelected()
    {
        if (detectorSuelo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(detectorSuelo.position, radioSuelo);
        }
    }
}