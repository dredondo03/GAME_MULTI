using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;

    void Update()
    {
        if (player == null)
            return;

        Vector3 targetPosition = player.position;

        // Mantener enemigo en el suelo
        targetPosition.y = transform.position.y;

        // Mover hacia el jugador
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        // Mirar al jugador
        transform.LookAt(targetPosition);
    }
}