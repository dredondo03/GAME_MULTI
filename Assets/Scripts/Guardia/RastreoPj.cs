using UnityEngine;

public class RastreoPJ : MonoBehaviour
{
    public float velocidad = 3f;
    public string nombreObjetivo = "PJ";

    private Transform objetivo;

    void Start()
    {
        GameObject pj = GameObject.Find(nombreObjetivo);

        if (pj != null)
        {
            objetivo = pj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontro un objeto llamado " + nombreObjetivo);
        }
    }

    void Update()
    {
        if (objetivo == null)
        {
            return;
        }

        Vector3 posicionObjetivo = objetivo.position;
        posicionObjetivo.y = transform.position.y;

        transform.position = Vector3.MoveTowards(
            transform.position,
            posicionObjetivo,
            velocidad * Time.deltaTime
        );

        Vector3 direccion = posicionObjetivo - transform.position;

        if (direccion != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direccion);
        }
    }
}
