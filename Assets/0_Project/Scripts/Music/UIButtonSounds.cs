using UnityEngine;
using UnityEngine.EventSystems;
using MyGame.Audio; // Ensure this is here if using Namespaces

public class UIButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
     public AudioManager audioManager;
    public void OnPointerEnter(PointerEventData eventData)
    {
       
        if (audioManager != null && audioManager.UI != null)
        {
            audioManager.UI.PlayHover();
        }
    }

    // --- YOU ONLY NEED ONE OF THESE ---
    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("Button Click Detected on: " + gameObject.name); // Optional Debug

        if (audioManager != null && audioManager.UI != null)
        {
            audioManager.UI.PlayClick();
        }
    }
}
