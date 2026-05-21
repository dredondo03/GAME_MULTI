// StaminaUI.cs
// Adjuntar a: cualquier GameObject de UI en el Canvas

using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    public Image staminaBar;             // Image con Image Type = Filled
    public SnakeMovement snakeMovement;  // Referencia al jugador

    void Update()
    {
        if (snakeMovement == null) return;
        // Actualizar el fill amount (0 = vacío, 1 = lleno) de la barra
        staminaBar.fillAmount = snakeMovement.GetStaminaPercent();
    }
}