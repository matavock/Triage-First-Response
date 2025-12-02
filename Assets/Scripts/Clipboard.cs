using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Clipboard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image clipboardImage;
    //public ProtocolModalController modal;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (clipboardImage != null)
        {
            var c = clipboardImage.color;
            c.a = 0.7f;
            clipboardImage.color = c;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // выделение назад
        if (clipboardImage != null)
        {
            var c = clipboardImage.color;
            c.a = 1f;
            clipboardImage.color = c;
        }
    }
}