using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PenHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image penImage;
    public float hoverAlpha = 0.7f;

    private float originalAlpha;

    void Start()
    {
        if (penImage == null)
            penImage = GetComponent<Image>();

        originalAlpha = penImage.color.a;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (penImage == null) return;

        Color c = penImage.color;
        c.a = hoverAlpha;
        penImage.color = c;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (penImage == null) return;

        Color c = penImage.color;
        c.a = originalAlpha;
        penImage.color = c;
    }
}