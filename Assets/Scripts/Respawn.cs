using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VoidZone : MonoBehaviour
{
    public Font font;
    public int fontSize = 80;
    public Color textColor = Color.red;
    
    private bool gameOverShown = false; // Flag para evitar repeticiones

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !gameOverShown)
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();

            // Pierde una vida
            player.CambiarVida(-1);

            // Respawnea si aún tiene vidas
            if (player.vidaActual > 0)
            {
                player.Respawn();
            }
            else
            {
                gameOverShown = true;
                MostrarGameOver();
            }
        }
    }

    private void MostrarGameOver()
    {
        Debug.Log("Game Over - Sin vidas restantes");

        // Pausa el juego
        Time.timeScale = 0f;

        // Crear Canvas
        GameObject canvasObject = new GameObject("GameOverCanvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Crear fondo oscuro
        Image panelImage = canvasObject.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.7f);

        // Crear Text
        GameObject textObject = new GameObject("GameOverText");
        textObject.transform.SetParent(canvasObject.transform, false);
        
        Text gameOverText = textObject.AddComponent<Text>();
        gameOverText.text = "Game Over\nJuego Terminado";
        gameOverText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        gameOverText.fontSize = 60;
        gameOverText.fontStyle = FontStyle.Bold;
        gameOverText.alignment = TextAnchor.MiddleCenter;
        gameOverText.color = textColor;

        // Posicionar texto al centro
        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = new Vector2(800, 200);

        // Espera 3 segundos y reinicia la escena
        Invoke("ReiniciarJuego", 3f);
    }

    private void ReiniciarJuego()
    {
        Time.timeScale = 1f; // Reanuda el tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}