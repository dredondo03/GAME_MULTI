using UnityEngine;

public class HideSystem : MonoBehaviour
{
    public float hideDistance = 3f;
    public LayerMask hideSpotLayer; // configura el layer de HideSpot en el inspector

    private bool hidden = false;
    private Renderer[] renderers;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, hideDistance, hideSpotLayer);
            if (hits.Length == 0)
            {
                Debug.Log("No hay HideSpot cercano");
                return;
            }

            hidden = !hidden;
            foreach (var r in renderers)
                r.enabled = !hidden;

            Debug.Log(hidden ? "Escondido" : "Visible");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, hideDistance);
    }
}