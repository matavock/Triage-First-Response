using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntryTicket : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image ticketImage;
    public PassportController modal;
    public EntryTicketData entryTicket;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ticketImage == null)
            return;

        Color color = ticketImage.color;
        color.a = 0.85f;
        ticketImage.color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ticketImage == null)
            return;

        Color color = ticketImage.color;
        color.a = 1f;
        ticketImage.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (modal != null)
            modal.ShowEntryTicket(entryTicket);
    }

    public void SetTicketInfo(EntryTicketData ticket)
    {
        entryTicket = ticket;
    }
}
