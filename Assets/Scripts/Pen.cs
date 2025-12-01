using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pen : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject actionButtons;
    //public Image admitButton;
    //public Image rejectButton;
    public Image penImage;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (penImage != null)
        {
            var c = penImage.color;
            c.a = 0.85f;
            penImage.color = c;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // выделение назад
        if (penImage != null)
        {
            var c = penImage.color;
            c.a = 1f;
            penImage.color = c;
        }
    }
    public void OnPointerClick()
    {
        if (actionButtons != null)
        {
            actionButtons.SetActive(true);
        }
    }


}