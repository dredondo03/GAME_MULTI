using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento Horizontal")]
    public float velocidad = 5.0f;
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

    // Esta variable reemplazará al inestable controller.isGrounded
    private bool estaEnElSuelo;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. DETECCIÓN MANUAL DE SUELO
        // Crea una esfera invisible. Si colisiona con la capa elegida, da 'true'.
        estaEnElSuelo = Physics.CheckSphere(detectorSuelo.position, radioSuelo, capaSuelo);

        // 2. MOVIMIENTO HORIZONTAL
        float moverHorizontal = Input.GetAxis("Horizontal");
        float moverVertical = Input.GetAxis("Vertical");

        Vector3 movimientoHorizontal = transform.right * moverHorizontal + transform.forward * moverVertical;
        controller.Move(movimientoHorizontal * velocidad * Time.deltaTime);

        // 3. CONTROL DE GRAVEDAD
        if (estaEnElSuelo && velocidadVertical.y < 0)
        {
            velocidadVertical.y = -2f; // Mantiene al jugador pegado al suelo de forma firme
        }

        // 4. CONTROL DEL SALTO (¡Ahora sí responderá siempre!)
        if (Input.GetButtonDown("Jump") && estaEnElSuelo)
        {
            velocidadVertical.y = Mathf.Sqrt(fuerzaSalto * -2f * gravedad);
        }

        // Aplicamos la gravedad
        velocidadVertical.y += gravedad * Time.deltaTime;
        controller.Move(velocidadVertical * Time.deltaTime);
    }

    // Esto te permite ver la esfera de detección en la ventana de Escena (Editor) para calibrarla
    void OnDrawGizmosSelected()
    {
        if (detectorSuelo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(detectorSuelo.position, radioSuelo);
        }
    }
}