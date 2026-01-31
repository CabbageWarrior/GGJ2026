using UnityEngine;
using UnityEngine.EventSystems; // Required for UI events

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("The Image to Show")]
    public GameObject iconImage; 

    // Called when Mouse enters the button area
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowIcon();
    }

    // Called when Mouse leaves the button area
    public void OnPointerExit(PointerEventData eventData)
    {
        HideIcon();
    }

    // Called when Mouse Clicks (or Finger Taps)
    public void OnPointerDown(PointerEventData eventData)
    {
        ShowIcon(); // Ensures it shows on tap for mobile
    }

    // Called when Click/Tap is released
    public void OnPointerUp(PointerEventData eventData)
    {
        // On mobile, you might want to keep it or hide it.
        // Usually, we hide it or let the button action change the scene.
        // For now, let's keep it visible until the pointer exits (for mouse)
        // or hide it if you want a "flash" effect.
    }

    void ShowIcon()
    {
        if (iconImage != null) iconImage.SetActive(true);
    }

    void HideIcon()
    {
        if (iconImage != null) iconImage.SetActive(false);
    }

    // Safety check: ensure it's hidden when the object is disabled (e.g. menu closes)
    void OnDisable()
    {
        HideIcon();
    }
}
