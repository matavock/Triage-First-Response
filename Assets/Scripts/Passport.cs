using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Passport : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image licenseImage;
    public PassportController modal;
    public string patientName;
    public string patientSex;
    public string patientBirth;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (licenseImage != null)
        {
            var c = licenseImage.color;
            c.a = 0.85f;
            licenseImage.color = c;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // выделение назад
        if (licenseImage != null)
        {
            var c = licenseImage.color;
            c.a = 1f;
            licenseImage.color = c;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        modal.Show(patientName, patientSex, patientBirth);
    }

    public void SetPatientInfo(string name, string sex, string birth)
    {
        patientName = name;
        patientSex = sex;
        patientBirth = birth;
    }
}
