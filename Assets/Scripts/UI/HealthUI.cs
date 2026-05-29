using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("Referencia al jugador")]
    public PlayerHealth playerHealth;

    [Header("Componente Image del Canvas")]
    public Image heartsImage;

    [Header("Sprites de corazones")]
    [Tooltip("Índice 0 = 1 corazón | Índice 1 = 2 corazones | Índice 2 = 3 corazones")]
    public Sprite[] heartSprites = new Sprite[3];

    void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogError("HealthUI: No se asignó PlayerHealth en el Inspector.");
            return;
        }

        // Suscribirse al evento de cambio de vidas
        playerHealth.OnLivesChanged += UpdateHeartsDisplay;

        // Mostrar el estado inicial
        UpdateHeartsDisplay(playerHealth.CurrentLives);
    }

    void OnDestroy()
    {
        // Desuscribirse para evitar errores si el objeto se destruye
        if (playerHealth != null)
            playerHealth.OnLivesChanged -= UpdateHeartsDisplay;
    }

    private void UpdateHeartsDisplay(int currentLives)
    {
        if (heartsImage == null || heartSprites == null) return;

        // Ocultar imagen si no quedan vidas (durante el respawn)
        if (currentLives <= 0)
        {
            heartsImage.enabled = false;
            return;
        }

        heartsImage.enabled = true;

        // currentLives 1→índice 0, 2→índice 1, 3→índice 2
        int index = Mathf.Clamp(currentLives - 1, 0, heartSprites.Length - 1);

        if (heartSprites[index] != null)
            heartsImage.sprite = heartSprites[index];
        else
            Debug.LogWarning($"HealthUI: Sprite en índice {index} no asignado.");
    }
}