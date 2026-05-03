using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Paper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image paperImage;
    public PaperController modal;

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

    public void SetPatientInfo(string name, int age, string complaints, bool shouldAdmit)
    {
        patientName = name;
        patientAge = age;
        patientComplaints = FormatComplaints(complaints, shouldAdmit);
    }

    private string FormatComplaints(string complaints, bool shouldAdmit)
    {
        if (PlayerPrefs.GetInt("InstantAnswerFeedback", 0) != 1)
            return complaints;

        string answer = shouldAdmit ? "принять" : "отказать";
        return complaints + "\nВерный ответ: " + answer;
    }
}