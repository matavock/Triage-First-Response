using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ClipboardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public Image clipboardImage;
    public GameObject rulesPanel;
    public TMP_Text rulesText;

    [Header("Контент правила для этого дня")]
    [TextArea]
    public string rulesContent;

    [Header("Звук")]
    public AudioSource audioSource;
    public AudioClip hoverSound;

    [Header("Эффект наведения")]
    public float hoverAlpha = 0.8f;

    private float originalAlpha;

    void Start()
    {
        if (clipboardImage == null)
            clipboardImage = GetComponent<Image>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (rulesPanel != null)
            rulesPanel.SetActive(false);

        if (clipboardImage != null)
            originalAlpha = clipboardImage.color.a;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // подсветка клипборда
        if (clipboardImage != null)
        {
            var c = clipboardImage.color;
            c.a = hoverAlpha;
            clipboardImage.color = c;
        }

        // показать панель с правилами
        if (rulesPanel != null)
            rulesPanel.SetActive(true);

        // установить текст правил
        if (rulesText != null)
            rulesText.text = rulesContent;

        // звук при наведении
        if (audioSource != null && hoverSound != null)
            audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // вернуть альфу
        if (clipboardImage != null)
        {
            var c = clipboardImage.color;
            c.a = originalAlpha;
            clipboardImage.color = c;
        }

        // спрятать панель с правилами
        if (rulesPanel != null)
            rulesPanel.SetActive(false);
    }
}
