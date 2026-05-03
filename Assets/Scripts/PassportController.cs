using TMPro;
using UnityEngine;

public class PassportController : MonoBehaviour
{
    [Header("UI")]
    public GameObject blurPanel;
    public GameObject modalPanel;

    public TMP_Text nameText;
    public TMP_Text sexText;
    public TMP_Text birthText;

    void Awake()
    {
        if (modalPanel != null) modalPanel.SetActive(false);
        if (blurPanel != null) blurPanel.SetActive(false);
    }

    public void Show(string name, string sex, string birth, MedicalRecordData medicalRecord = null, EntryTicketData entryTicket = null)
    {
        if (blurPanel != null) blurPanel.SetActive(true);
        if (modalPanel != null) modalPanel.SetActive(true);

        if (nameText != null) nameText.text = "Имя: " + name;
        if (sexText != null) sexText.text = "Пол: " + sex;

        if (birthText != null)
        {
            string text = "Дата рождения: " + birth;
            if (medicalRecord != null)
            {
                if (medicalRecord.hasCard)
                {
                    text += "\n\nMedical ID";
                    if (!string.IsNullOrEmpty(medicalRecord.fullName))
                        text += "\nИмя: " + medicalRecord.fullName;
                    if (!string.IsNullOrEmpty(medicalRecord.serialNumber))
                        text += "\nСерийный номер: " + medicalRecord.serialNumber;
                    text += "\nПол в медкарте: " + medicalRecord.gender;
                    text += "\nРегион: " + medicalRecord.region;
                    text += "\nВыдана: " + medicalRecord.issuingHospital;
                    text += "\nГодна до: " + medicalRecord.expirationDate;
                }
                else
                {
                    text += "\n\nMedical ID: отсутствует";
                }
            }

            if (entryTicket != null)
            {
                if (entryTicket.hasTicket)
                {
                    text += "\n\nEntry Ticket";
                    text += "\nИмя: " + entryTicket.fullName;
                    text += "\nПол: " + entryTicket.gender;
                    text += "\nСерийный номер карты: " + entryTicket.medicalCardSerial;
                    text += "\nДата прибытия: " + entryTicket.arrivalDate;
                }
                else
                {
                    text += "\n\nEntry Ticket: отсутствует";
                }
            }

            birthText.text = text;
        }
    }

    public void Hide()
    {
        if (blurPanel != null) blurPanel.SetActive(false);
        if (modalPanel != null) modalPanel.SetActive(false);
    }
}
