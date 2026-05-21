// GuardAI.cs
// Adjuntar a: Guard
// Requiere: NavMeshAgent, FieldOfView.cs en el mismo GameObject

using UnityEngine;
using UnityEngine.AI;

public class GuardAI : MonoBehaviour
{
    // ── ESTADOS POSIBLES ──────────────────────────────────────────────────────
    public enum GuardState
    {
        Patrol,       // Camina entre puntos de patrulla
        Investigate,  // Va a la última posición sospechosa
        Chase,        // Persigue activamente al jugador
        SearchHide    // Registra un escondite específico
    }

    [Header("Estado Actual (Solo Lectura)")]
    public GuardState currentState = GuardState.Patrol;

    // ── PATRULLA ──────────────────────────────────────────────────────────────
    [Header("Patrulla")]
    public Transform[] patrolPoints;          // Array de waypoints en la escena
    public float patrolWaitTime = 2f;         // Segundos de espera en cada punto
    private int currentPatrolIndex = 0;       // Índice del punto actual
    private float waitTimer = 0f;             // Temporizador de espera

    // ── VELOCIDADES ───────────────────────────────────────────────────────────
    [Header("Velocidades")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float investigateSpeed = 3.5f;

    // ── INVESTIGACIÓN ─────────────────────────────────────────────────────────
    [Header("Investigación")]
    public float investigateTime = 8f;        // Segundos buscando antes de volver a patrullar
    private float investigateTimer = 0f;
    private Vector3 lastKnownPosition;        // Última posición conocida del jugador

    // ── DETECCIÓN POR SONIDO ──────────────────────────────────────────────────
    [Header("Detección por Sonido")]
    public float hearingRange = 6f;           // Radio en el que el guardia escucha al jugador corriendo

    // ── REGISTRO DE ESCONDITES ────────────────────────────────────────────────
    [Header("Registro de Escondites")]
    public float searchHideTime = 5f;         // Segundos inspeccionando el escondite
    public float hideSpotCheckRange = 1f;     // Distancia para "entrar" al escondite y revisar
    private float searchHideTimer = 0f;
    private Transform targetHideSpot = null;  // El escondite que va a registrar

    // ── REFERENCIAS ───────────────────────────────────────────────────────────
    private NavMeshAgent agent;
    private FieldOfView fov;
    private Transform playerTransform;
    private HidingSystem playerHiding;
    private SnakeMovement playerMovement;
    private bool playerWasHiding = false;     // Para detectar si el jugador acaba de esconderse

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();

        // Buscar al jugador por su tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        playerHiding = player.GetComponent<HidingSystem>();
        playerMovement = player.GetComponent<SnakeMovement>();

        if (agent != null && !agent.isOnNavMesh)
            Debug.LogWarning("[GuardAI] NavMeshAgent no está colocado en la NavMesh. Revisa el Base Offset y la posición inicial.");

        // Comenzar en patrulla hacia el primer punto
        GoToNextPatrolPoint();
    }

    void Update()
    {
        // Detectar sonido del jugador (si está corriendo cerca)
        CheckHearing();

        // Manejar la detección visual
        HandleVisualDetection();

        // Ejecutar el comportamiento del estado actual
        switch (currentState)
        {
            case GuardState.Patrol:      UpdatePatrol();      break;
            case GuardState.Investigate: UpdateInvestigate(); break;
            case GuardState.Chase:       UpdateChase();       break;
            case GuardState.SearchHide:  UpdateSearchHide();  break;
        }
    }

    // ── DETECCIÓN POR SONIDO ──────────────────────────────────────────────────
    void CheckHearing()
    {
        if (playerHiding.isHiding) return; // No escucha al jugador escondido

        float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        bool playerIsSprinting = playerMovement.currentSpeed > playerMovement.walkSpeed + 0.1f;

        // Si el jugador está corriendo dentro del rango de escucha
        if (distToPlayer <= hearingRange && playerIsSprinting)
        {
            if (currentState == GuardState.Patrol) // Solo cambia si estaba patrullando
            {
                lastKnownPosition = playerTransform.position;
                ChangeState(GuardState.Investigate);
            }
        }
    }

    // ── DETECCIÓN VISUAL ──────────────────────────────────────────────────────
    void HandleVisualDetection()
    {
        bool playerIsHidingNow = playerHiding.isHiding;

        // CASO 1: El guardia ve al jugador directamente (no escondido)
        if (fov.canSeePlayer && !playerIsHidingNow)
        {
            lastKnownPosition = playerTransform.position;
            playerWasHiding = false;
            ChangeState(GuardState.Chase); // ¡Perseguir!
        }

        // CASO 2: El jugador se escondió MIENTRAS el guardia lo estaba viendo
        if (playerIsHidingNow && !playerWasHiding && currentState == GuardState.Chase)
        {
            // El guardia sabe exactamente en qué escondite está
            targetHideSpot = playerHiding.currentHideSpot;
            playerWasHiding = true;
            ChangeState(GuardState.SearchHide); // Ir a registrar ese escondite
        }

        // CASO 3: El guardia perseguía pero ya no ve al jugador y no se escondió
        if (currentState == GuardState.Chase && !fov.canSeePlayer && !playerIsHidingNow)
        {
            // Ir al último lugar conocido a investigar
            ChangeState(GuardState.Investigate);
        }
    }

    // ── COMPORTAMIENTO: PATRULLA ──────────────────────────────────────────────
    void UpdatePatrol()
    {
        if (agent == null || !agent.isOnNavMesh)
            return;

        // Si llegó al punto de patrulla, esperar y luego ir al siguiente
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaitTime)
            {
                waitTimer = 0f;
                GoToNextPatrolPoint();
            }
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        agent.speed = patrolSpeed;

        // Avanzar al siguiente punto (cíclico)
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    // ── COMPORTAMIENTO: INVESTIGAR ────────────────────────────────────────────
    void UpdateInvestigate()
    {
        if (agent == null || !agent.isOnNavMesh)
            return;

        // Ir a la última posición conocida
        agent.destination = lastKnownPosition;

        // Contar tiempo de búsqueda
        investigateTimer += Time.deltaTime;

        // Si llegó al punto o se agotó el tiempo, volver a patrullar
        bool arrivedAtSpot = !agent.pathPending && agent.remainingDistance < 0.5f;
        if (arrivedAtSpot || investigateTimer >= investigateTime)
        {
            investigateTimer = 0f;
            ChangeState(GuardState.Patrol);
        }
    }

    // ── COMPORTAMIENTO: PERSECUCIÓN ───────────────────────────────────────────
    void UpdateChase()
    {
        if (!playerHiding.isHiding)
        {
            // Perseguir directamente al jugador
            agent.destination = playerTransform.position;
            agent.speed = chaseSpeed;
        }
    }

    // ── COMPORTAMIENTO: REGISTRAR ESCONDITE ───────────────────────────────────
    void UpdateSearchHide()
    {
        if (targetHideSpot == null)
        {
            ChangeState(GuardState.Patrol);
            return;
        }

        // Moverse hacia el escondite
        agent.destination = targetHideSpot.position;
        agent.speed = chaseSpeed;

        float distToHide = Vector3.Distance(transform.position, targetHideSpot.position);

        // ¿Llegó al escondite?
        if (distToHide <= hideSpotCheckRange)
        {
            searchHideTimer += Time.deltaTime;

            // Tras 'searchHideTime' segundos, "encontrar" al jugador y sacarlo
            if (searchHideTimer >= searchHideTime)
            {
                searchHideTimer = 0f;

                // Si el jugador SIGUE escondido en ese escondite, sacarlo a la fuerza
                if (playerHiding.isHiding && playerHiding.currentHideSpot == targetHideSpot)
                {
                    playerHiding.ForceExit(); // ¡El guardia lo encontró!
                    Debug.Log("[Guardia] ¡Encontré a la serpiente en el escondite!");
                    lastKnownPosition = playerTransform.position;
                    ChangeState(GuardState.Chase); // Ahora perseguirlo
                }
                else
                {
                    // El jugador ya salió antes de que llegara
                    targetHideSpot = null;
                    ChangeState(GuardState.Investigate);
                }
            }
        }
    }

    // ── CAMBIO DE ESTADO (centralizado para mayor claridad) ──────────────────
    void ChangeState(GuardState newState)
    {
        if (currentState == newState) return; // No cambiar si ya está en ese estado

        currentState = newState;
        Debug.Log($"[Guardia] Nuevo estado: {newState}");

        // Configurar velocidad según el estado
        switch (newState)
        {
            case GuardState.Patrol:
                agent.speed = patrolSpeed;
                GoToNextPatrolPoint();
                break;
            case GuardState.Investigate:
                agent.speed = investigateSpeed;
                agent.destination = lastKnownPosition;
                investigateTimer = 0f;
                break;
            case GuardState.Chase:
                agent.speed = chaseSpeed;
                break;
            case GuardState.SearchHide:
                agent.speed = chaseSpeed;
                searchHideTimer = 0f;
                break;
        }
    }

    // Dibuja el radio de escucha en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
    }
}