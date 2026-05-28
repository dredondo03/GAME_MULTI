using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vidas")]
    public int maxLives = 3;
    private int currentLives;

    void Start()
    {
        // El jugador inicia con el máximo de vidas permitido
        currentLives = maxLives;
        Debug.Log("Vidas iniciales: " + currentLives);
    }

    // El enemigo llamará a esta función para restar vida
    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        Debug.Log("El jugador recibió daño. Vidas restantes: " + currentLives);

        if (currentLives <= 0)
        {
            Die();
        }
    }

    // El objeto del mapa llamará a esta función para sumar vida
    public void Heal(int amount)
    {
        // Si ya está lleno, no hace nada
        if (currentLives >= maxLives)
        {
            Debug.Log("Vida ya está al máximo.");
            return;
        }

        currentLives += amount;
        
        // Evitamos que las vidas superen el máximo establecido
        currentLives = Mathf.Min(currentLives, maxLives);
        Debug.Log("¡Vida recuperada! Vidas actuales: " + currentLives);
    }

    private void Die()
    {
        Debug.Log("¡Game Over! El personaje se quedó sin vidas.");
        // Desactivamos al personaje (puedes cambiar esto por tu lógica de reinicio)
        gameObject.SetActive(false); 
    }
}