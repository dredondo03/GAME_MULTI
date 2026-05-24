using UnityEngine;

public class HideSystem : MonoBehaviour
{
    public float hideDistance = 3f;

    private bool hidden = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                hideDistance
            );

            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("HideSpot"))
                {
                    hidden = !hidden;

                    Renderer r = GetComponent<Renderer>();

                    if (r != null)
                    {
                        r.enabled = !hidden;
                    }

                    Debug.Log(hidden ?
                        "Escondido" :
                        "Visible");

                    break;
                }
            }
        }
    }
}