// KeyItem.cs
// Adjuntar a: cada objeto Llave en la escena
// Requiere: Collider con "Is Trigger" activado

using UnityEngine;

public class KeyItem : MonoBehaviour
{
    [Header("Identificación")]
    public string keyID = "key_main_exit"; // ID único de esta llave — debe coincidir con la puerta

    [Header("Visual (Opcional)")]
    public float rotationSpeed = 90f;      // La llave rota para verse bien en la escena

    void Update()
    {
        // Rotar suavemente para hacerla visible
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    // Se activa cuando el jugador (u otro collider) entra al trigger
    void OnTriggerEnter(Collider other)
    {
        // Verificar que es el jugador quien la recogió
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.AddKey(keyID);    // Agregar la llave al inventario
            Destroy(gameObject);        // Destruir el objeto llave de la escena
        }
    }
}