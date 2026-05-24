using UnityEngine;

public class HideSystem : MonoBehaviour
{
    public GameObject hideSpot;

    private bool canHide;
    private bool hidden;

    private Renderer playerRenderer;

    void Start()
    {
        playerRenderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        if (canHide && Input.GetKeyDown(KeyCode.Space))
        {
            hidden = !hidden;
            playerRenderer.enabled = !hidden;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == hideSpot)
        {
            canHide = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == hideSpot)
        {
            canHide = false;

            if (hidden)
            {
                hidden = false;
                playerRenderer.enabled = true;
            }
        }
    }

    // Corregido: Ahora este método está dentro de la clase
    public bool IsHidden()
    {
        return hidden;
    }
}