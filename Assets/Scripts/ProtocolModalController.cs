using UnityEngine;
using TMPro;

public class ProtocolModalController : MonoBehaviour
{
    [Header("UI ссылки")]
    public GameObject blurPanel;          // BlurPanel (затемнение/блюр фона)
    public GameObject modalPanel;         // сам ProtocolModalPanel

    public TMP_Text nameText;
    public TMP_Text ageText;
    public TMP_Text complaintsText;

    void Awake()
    {
        // на всякий случай скрываем при старте
        if (modalPanel != null) modalPanel.SetActive(false);
        if (blurPanel != null) blurPanel.SetActive(false);
    }

    public void Show(string name, int age, string complaints)
    {
        if (blurPanel != null) blurPanel.SetActive(true);
        if (modalPanel != null) modalPanel.SetActive(true);

        if (nameText != null) nameText.text = "Имя: " + name;
        if (ageText != null) ageText.text = "Возраст: " + age.ToString();
        if (complaintsText != null) complaintsText.text = "Жалобы: " + complaints;
    }

    public void Hide()
    {
        if (blurPanel != null) blurPanel.SetActive(false);
        if (modalPanel != null) modalPanel.SetActive(false);
    }
}