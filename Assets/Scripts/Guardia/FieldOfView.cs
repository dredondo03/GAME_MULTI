// FieldOfView.cs
// Adjuntar a: Guard
// Calcula si el jugador está dentro del cono de visión

using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("Parámetros de Visión")]
    public float viewRadius = 10f;          // Radio máximo de detección visual
    [Range(0, 360)]
    public float viewAngle = 90f;           // Ángulo del cono de visión (grados)
    public LayerMask targetMask;            // Layer del jugador
    public LayerMask obstacleMask;          // Layer de paredes/objetos que bloquean visión

    [HideInInspector] public bool canSeePlayer = false;    // ¿Ve al jugador ahora?
    [HideInInspector] public Transform visibleTarget = null; // Referencia al jugador si lo ve

    void Update()
    {
        FindVisibleTargets();
    }

    void FindVisibleTargets()
    {
        canSeePlayer = false;
        visibleTarget = null;

        // Buscar todos los colliders del jugador dentro del radio
        Collider[] targetsInRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (Collider target in targetsInRadius)
        {
            Transform targetTransform = target.transform;

            // Calcular dirección al jugador
            Vector3 dirToTarget = (targetTransform.position - transform.position).normalized;

            // Verificar si el jugador está dentro del ángulo del cono
            float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);
            if (angleToTarget > viewAngle / 2f) continue; // Fuera del cono, ignorar

            // Verificar si hay un obstáculo entre el guardia y el jugador
            float distToTarget = Vector3.Distance(transform.position, targetTransform.position);
            bool blocked = Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask);

            if (!blocked)
            {
                // ¡Ve al jugador!
                canSeePlayer = true;
                visibleTarget = targetTransform;
                break; // Solo necesitamos detectar uno
            }
        }
    }

    // Convierte un ángulo en grados a un vector de dirección (útil para Gizmos)
    public Vector3 DirFromAngle(float angleDegrees, bool globalAngle)
    {
        if (!globalAngle) angleDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }

    // Dibuja el cono de visión en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftBound = DirFromAngle(-viewAngle / 2f, false);
        Vector3 rightBound = DirFromAngle(viewAngle / 2f, false);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftBound * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBound * viewRadius);

        if (canSeePlayer && visibleTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, visibleTarget.position);
        }
    }
}