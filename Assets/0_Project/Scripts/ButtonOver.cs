using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("The Image to Show")]
    public GameObject iconImage; 

    void Awake()
    {
        // SAFETY: Automatically hide the icon when the game starts (or menu opens)
        // This means you can keep it visible in the Editor for easier positioning!
        HideIcon();
    }

    // Also ensure it resets whenever the menu is re-opened
    void OnEnable()
    {
        HideIcon();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowIcon();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideIcon();
    }

    // Optional: Keep showing on click (good for mobile feeling)
    public void OnPointerDown(PointerEventData eventData)
    {
        ShowIcon();
    }

    void ShowIcon()
    {
        if (iconImage != null) iconImage.SetActive(true);
    }

    void HideIcon()
    {
        if (iconImage != null) iconImage.SetActive(false);
    }
}
