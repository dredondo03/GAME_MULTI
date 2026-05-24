using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;

    public Transform respawnPoint;

    private CharacterController controller;

    void Start()
    {
        currentLives = maxLives;
        controller = GetComponent<CharacterController>();
    }

    public void TakeDamage()
    {
        currentLives--;

        Debug.Log("Vidas restantes: " + currentLives);

        controller.enabled = false;
        transform.position = respawnPoint.position;
        controller.enabled = true;

        if (currentLives <= 0)
        {
            Debug.Log("GAME OVER");
        }
    }

    public void Heal()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
        }
    }
}