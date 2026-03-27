using TMPro;
using UnityEngine;

public class PassportController : MonoBehaviour
{
    [Header("UI ссылки")]
    public GameObject blurPanel;          // BlurPanel (затемнение/блюр фона)
    public GameObject modalPanel;         // сам ProtocolModalPanel

    public TMP_Text nameText;
    public TMP_Text sexText;
    public TMP_Text birthText;

    void Awake()
    {
        // на всякий случай скрываем при старте
        if (modalPanel != null) modalPanel.SetActive(false);
        if (blurPanel != null) blurPanel.SetActive(false);
    }

    public void Show(string name, string sex, string birth)
    {
        if (blurPanel != null) blurPanel.SetActive(true);
        if (modalPanel != null) modalPanel.SetActive(true);

        if (nameText != null) nameText.text = "Имя: " + name;
        if (sexText != null) sexText.text = "Пол: " + sex.ToString();
        if (birthText != null) birthText.text = "Дата рождения: " + birth;
    }

    public void Hide()
    {
        if (blurPanel != null) blurPanel.SetActive(false);
        if (modalPanel != null) modalPanel.SetActive(false);
    }
}
