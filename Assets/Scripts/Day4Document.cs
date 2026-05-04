using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Day4Document : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image documentImage;
    public PassportController modal;
    public string documentTitle;
    [TextArea] public string documentBody;

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetAlpha(0.85f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetAlpha(1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (modal != null)
            modal.ShowTextDocument(documentTitle, documentBody);
    }

    public void SetDocument(string title, string body)
    {
        documentTitle = title;
        documentBody = body;
    }

    private void SetAlpha(float alpha)
    {
        if (documentImage == null)
            return;

        Color color = documentImage.color;
        color.a = alpha;
        documentImage.color = color;
    }
}
