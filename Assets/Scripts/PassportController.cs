using TMPro;
using UnityEngine;

public class PassportController : MonoBehaviour
{
    [Header("UI ������")]
    public GameObject blurPanel;          // BlurPanel (����������/���� ����)
    public GameObject modalPanel;         // ��� ProtocolModalPanel

    public TMP_Text nameText;
    public TMP_Text sexText;
    public TMP_Text birthText;

    void Awake()
    {
        // �� ������ ������ �������� ��� ������
        if (modalPanel != null) modalPanel.SetActive(false);
        if (blurPanel != null) blurPanel.SetActive(false);
    }

public void Show(string name, string sex, string birth, MedicalRecordData medicalRecord = null)
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
                    text += "\nПол в медкарте: " + medicalRecord.gender;
                    text += "\nРегион: " + medicalRecord.region;
                    text += "\nВыдана: " + medicalRecord.issuingHospital;
                    text += "\nГодна до: " + medicalRecord.expirationDate;
                }
                else
                {
                    text += "\nМедкарта: отсутствует";
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
