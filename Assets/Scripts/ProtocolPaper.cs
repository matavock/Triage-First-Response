using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ProtocolPaper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image paperImage;
    public ProtocolModalController modal;

    public string patientName;
    public int patientAge;
    [TextArea] public string patientComplaints;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (paperImage != null)
        {
            var c = paperImage.color;
            c.a = 0.85f;
            paperImage.color = c;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // выделение назад
        if (paperImage != null)
        {
            var c = paperImage.color;
            c.a = 1f;
            paperImage.color = c;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        modal.Show(patientName, patientAge, patientComplaints);
    }

    public void SetPatientInfo(string name, int age, string complaints)
    {
        patientName = name;
        patientAge = age;
        patientComplaints = complaints;
    }
}