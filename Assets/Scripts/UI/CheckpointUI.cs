using UnityEngine;
using TMPro;
using System.Collections;

public class CheckpointUI : MonoBehaviour
{
    public static CheckpointUI Instance;

    [Header("Configuración")]
    public TextMeshProUGUI checkpointText;
    public float displayDuration = 2f; // Segundos que aparece el mensaje

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        checkpointText.gameObject.SetActive(false);
    }

    public void ShowCheckpointMessage()
    {
        StopAllCoroutines();
        StartCoroutine(ShowAndHide());
    }

    private IEnumerator ShowAndHide()
    {
        checkpointText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        checkpointText.gameObject.SetActive(false);
    }
}