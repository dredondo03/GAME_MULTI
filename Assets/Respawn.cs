using UnityEngine;

public class VoidZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player =
                other.GetComponent<PlayerHealth>();

            player.Respawn();
        }
    }
}