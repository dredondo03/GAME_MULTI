using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("Panel de Game Over")]
    public GameObject gameOverPanel;
    public Button restartButton;

    void Start()
    {
        gameOverPanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pausa el juego
    }

    private void RestartGame()
    {
        Time.timeScale = 1f; // Restaura el tiempo antes de recargar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}