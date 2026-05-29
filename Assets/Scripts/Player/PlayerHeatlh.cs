using UnityEngine;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vidas")]
    public int maxLives = 3;
    private int currentLives;

    [Header("Respawn")]
    public float respawnDelay = 1.5f;
    public PlayerSpawnManager spawnManager;     // Asignar en el Inspector
    
    [Header("Game Over UI")]
    public GameOverUI gameOverUI;
    
    // Evento que la UI escucha para actualizarse
    public event Action<int> OnLivesChanged;

    // Propiedad pública de solo lectura para que la UI acceda al valor
    public int CurrentLives => currentLives;

    void Start()
    {
        currentLives = maxLives;
        OnLivesChanged?.Invoke(currentLives); // Notifica a la UI al inicio
        Debug.Log("Vidas iniciales: " + currentLives);
    }

    public void TakeDamage(int damage)
    {
        currentLives -= damage;
        currentLives = Mathf.Max(currentLives, 0); // Nunca menor a 0
        OnLivesChanged?.Invoke(currentLives);       // Notifica a la UI
        Debug.Log("El jugador recibió daño. Vidas restantes: " + currentLives);

        if (currentLives <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (currentLives >= maxLives)
        {
            Debug.Log("Vida ya está al máximo.");
            return;
        }

        currentLives = Mathf.Min(currentLives + amount, maxLives);
        OnLivesChanged?.Invoke(currentLives); // Notifica a la UI
        Debug.Log("¡Vida recuperada! Vidas actuales: " + currentLives);
    }

    private void Die()
    {
        Debug.Log("¡Te atrapo el guardia!");
        if (gameOverUI != null)
            gameOverUI.ShowGameOver();
        else
            gameObject.SetActive(false);
    }

    private IEnumerator RespawnRoutine()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(respawnDelay);

        spawnManager.TeletransportarAlPuntoSeguro(); // Usa tu script existente
        
        gameObject.SetActive(true);
        Debug.Log("Respawn en último checkpoint guardado.");
    }
}   