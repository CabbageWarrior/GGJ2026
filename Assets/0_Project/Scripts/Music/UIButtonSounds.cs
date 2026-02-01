using UnityEngine;
using UnityEngine.EventSystems;
using MyGame.Audio; // Ensure this is here if using Namespaces

public class UIButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AudioManager.Instance != null && AudioManager.Instance.UI != null)
        {
            AudioManager.Instance.UI.PlayHover();
        }
    }

    // --- YOU ONLY NEED ONE OF THESE ---
    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("Button Click Detected on: " + gameObject.name); // Optional Debug

        if (AudioManager.Instance != null && AudioManager.Instance.UI != null)
        {
            AudioManager.Instance.UI.PlayClick();
        }
    }
}
