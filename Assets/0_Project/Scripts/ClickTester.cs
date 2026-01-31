using UnityEngine;
using UnityEngine.EventSystems; // Importante per l'interazione UI

public class ClickTester : MonoBehaviour, IPointerDownHandler
{
    public string objectName = "Oggetto Di Scena";

    // Questo metodo viene chiamato quando clicchi sull'oggetto (se ha un Collider e PhysicsRaycaster)
    // OPPURE se è un elemento UI (se ha un RaycastTarget)
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"<color=green>CLICCATO: {objectName}</color>");
    }

    // Fallback per oggetti 3D semplici con OnMouseDown (se non usi il sistema EventSystem completo)
    void OnMouseDown()
    {
        // Se il blocker UI funziona, questo NON dovrebbe scattare quando c'è un pannello UI davanti
        // Nota: OnMouseDown a volte passa attraverso la UI se non c'è un EventSystem configurato per bloccarlo.
        // Per sicurezza, controlliamo se il mouse è sopra una UI.
        if (!EventSystem.current.IsPointerOverGameObject()) 
        {
             Debug.Log($"<color=green>CLICCATO (MouseDown): {objectName}</color>");
        }
        else
        {
             Debug.Log($"<color=yellow>CLICK BLOCCATO dalla UI su: {objectName}</color>");
        }
    }
}
