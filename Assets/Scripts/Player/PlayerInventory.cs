// PlayerInventory.cs
// Adjuntar a: Snake_Player
// Sistema de inventario simple basado en IDs de llave

using UnityEngine;
using System.Collections.Generic; // Para usar List<>

public class PlayerInventory : MonoBehaviour
{
    // Lista de IDs de llaves que el jugador ha recogido
    // Ejemplo: "key_main_exit", "key_storage_room"
    private List<string> collectedKeys = new List<string>();

    // Agrega una llave al inventario
    public void AddKey(string keyID)
    {
        if (!collectedKeys.Contains(keyID)) // Evitar duplicados
        {
            collectedKeys.Add(keyID);
            Debug.Log($"[Inventario] Llave recogida: {keyID}");
        }
    }

    // Verifica si el jugador tiene una llave específica
    public bool HasKey(string keyID)
    {
        return collectedKeys.Contains(keyID);
    }

    // Elimina una llave del inventario (se consume al usarla)
    public void RemoveKey(string keyID)
    {
        collectedKeys.Remove(keyID);
    }
}