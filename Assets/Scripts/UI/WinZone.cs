using UnityEngine;
using TMPro;
using System.Collections;

public class WinZone : MonoBehaviour
{
    [Header("UI")]
    public GameObject winPanel;
    public TextMeshProUGUI winText;

    void Start()
    {
        winPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ShowWinMessage());
        }
    }

    private IEnumerator ShowWinMessage()
    {
        winPanel.SetActive(true);
        winText.text = "¡Escapaste!";
        Time.timeScale = 0f; // Pausa el juego
        yield return null;
    }
}