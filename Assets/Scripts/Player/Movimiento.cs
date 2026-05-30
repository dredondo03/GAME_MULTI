using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Movimiento Horizontal")]
    public float velocidad = 5.0f;
    public float velocidadRotacion = 8.0f; // <- NUEVO: Controla qué tan rápido gira el personaje
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

    [Header("Cámara")]
    public Transform camaraPrincipal; // Arrastra la Main Camera en el Inspector
    
    private bool estaEnElSuelo;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        estaEnElSuelo = Physics.CheckSphere(detectorSuelo.position, radioSuelo, capaSuelo);

        float moverHorizontal = Input.GetAxis("Horizontal");
        float moverVertical = Input.GetAxis("Vertical");

        // Dirección relativa a donde mira la cámara
        Vector3 adelanteCamara = camaraPrincipal.forward;
        Vector3 derechaCamara = camaraPrincipal.right;

        // Ignoramos el eje Y para que no se mueva hacia arriba/abajo
        adelanteCamara.y = 0;
        derechaCamara.y = 0;
        adelanteCamara.Normalize();
        derechaCamara.Normalize();

        Vector3 direccionMovimiento = (adelanteCamara * moverVertical + derechaCamara * moverHorizontal).normalized;

        if (direccionMovimiento.magnitude >= 0.1f)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionMovimiento);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, velocidadRotacion * Time.deltaTime);
            controller.Move(direccionMovimiento * velocidad * Time.deltaTime);
        }

        // Gravedad y salto igual que antes
        if (estaEnElSuelo && velocidadVertical.y < 0)
            velocidadVertical.y = -2f;

        if (Input.GetButtonDown("Jump") && estaEnElSuelo)
            velocidadVertical.y = Mathf.Sqrt(fuerzaSalto * -2f * gravityVal());

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