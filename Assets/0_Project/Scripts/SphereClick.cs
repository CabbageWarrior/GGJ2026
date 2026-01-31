using UnityEngine;
using UnityEngine.EventSystems;

public class SphereClick : MonoBehaviour
{
    void OnMouseDown()
    {
        // Questo controllo serve per non cliccare attraverso la UI (es. pulsanti pausa)
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // --- IL TUO CODICE QUI ---
        Debug.Log("SFERA COLPITA! Eseguo azione.");
        
        // Esempio: Cambia colore
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
}
