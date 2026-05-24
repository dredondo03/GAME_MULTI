using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player =
            other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}