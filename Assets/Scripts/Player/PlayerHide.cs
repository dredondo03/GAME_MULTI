using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    private bool estaCercaDeEscondite = false;
    private bool estaEscondido = false;
    private Vector3 posicionAntesDeEsconderse;

    // Guardamos una referencia al script del enemigo para avisarle
    private EnemigoSeguidor scriptEnemigo; 

    void Start()
    {
        // Buscamos al enemigo en la escena al iniciar
        scriptEnemigo = Object.FindFirstObjectByType<EnemigoSeguidor>();
    }

    void Update()
    {
        // Si el jugador está cerca del escondite y presiona la tecla H
        if (estaCercaDeEscondite && Input.GetKeyDown(KeyCode.H))
        {
            if (!estaEscondido)
            {
                Esconderse();
            }
            else
            {
                SalirDelEscondite();
            }
        }
    }

    void Esconderse()
    {
        estaEscondido = true;
        Debug.Log("¡Te has escondido! El enemigo te perderá de vista.");

        // 1. Le avisamos al enemigo que ya no nos ve y le damos nuestra última posición
        if (scriptEnemigo != null)
        {
            scriptEnemigo.PerderDeVistaALJugador(transform.position);
        }

        // 2. Guardamos la posición actual
        posicionAntesDeEsconderse = transform.position;

        // 3. Hacemos "invisible" al jugador desactivando sus componentes visuales o su movimiento
        // Desactivamos el MeshRenderer para que no se vea el modelo 3D
        if (GetComponent<MeshRenderer>() != null) GetComponent<MeshRenderer>().enabled = false;
        
        // Si tu FBX tiene los renders en los hijos (lo normal en personajes complejos), usa esto:
        foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>()) r.enabled = false;
        foreach (SkinnedMeshRenderer r in GetComponentsInChildren<SkinnedMeshRenderer>()) r.enabled = false;

        // Opcional: Si tienes un script de movimiento propio del jugador, desactívalo aquí para que no se mueva mientras está escondido.
    }

 void SalirDelEscondite()
{
    estaEscondido = false;
    Debug.Log("Saliste del escondite.");

    // Avisa a todos los enemigos normales
    EnemigoSeguidor[] enemigosNormales = Object.FindObjectsByType<EnemigoSeguidor>(FindObjectsSortMode.None);
    foreach (EnemigoSeguidor e in enemigosNormales) e.VolverAVerAlJugador();

    // Avisa a todos los guardianes
    EnemigoGuardian[] guardianes = Object.FindObjectsByType<EnemigoGuardian>(FindObjectsSortMode.None);
    foreach (EnemigoGuardian g in guardianes) g.VolverAVerAlJugador();

    // (El resto de tu código para volver a activar los MeshRenderers...)
    if (GetComponent<MeshRenderer>() != null) GetComponent<MeshRenderer>().enabled = true;
    foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>()) r.enabled = true;
    foreach (SkinnedMeshRenderer r in GetComponentsInChildren<SkinnedMeshRenderer>()) r.enabled = true;
}

    // Detectamos cuando el jugador entra al Trigger del objeto FBX de esconderse
    private void OnTriggerEnter(Collider other)
    {
        // Puedes ponerle un Tag al objeto escondite (ej: "Escondite") para estar seguros
        if (other.CompareTag("Escondite") || other.name.Contains("Escondite"))
        {
            estaCercaDeEscondite = true;
            Debug.Log("Estás cerca de un escondite. Presiona 'H' para entrar.");
        }
    }

    // Detectamos cuando el jugador se aleja del escondite
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Escondite") || other.name.Contains("Escondite"))
        {
            estaCercaDeEscondite = false;
        }
    }
}