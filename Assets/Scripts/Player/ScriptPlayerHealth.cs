
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives;

    void Start()
    {
        currentLives = maxLives;
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;

        Debug.Log("Vidas: " + currentLives);

        if (currentLives <= 0)
        {
            Die();
        }
    }

    public void AddLife(int amount)
    {
        currentLives += amount;

        if (currentLives > maxLives)
        {
            currentLives = maxLives;
        }

        Debug.Log("Vida actual: " + currentLives);
    }

    void Die()
    {
        Debug.Log("Jugador muerto");

        // Reiniciar escena o respawn
    }
}

