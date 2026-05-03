using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ClipboardHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public Image clipboardImage;
    public GameObject rulesPanel;
    public TMP_Text rulesText;

    [Header("������� ������� ��� ����� ���")]
    [TextArea]
    public string rulesContent;

    [Header("����")]
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioMixerGroup sfxMixerGroup;

    [Header("������ ���������")]
    public bool useHoverColor = false;
    public Color hoverColor = Color.white;

    public float hoverAlpha = 0.8f;

    private Color originalColor;
    private float originalAlpha;

    void Start()
    {
        if (clipboardImage == null)
            clipboardImage = GetComponent<Image>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && sfxMixerGroup != null)
            audioSource.outputAudioMixerGroup = sfxMixerGroup;

        if (rulesPanel != null)
            rulesPanel.SetActive(false);

        if (clipboardImage != null)
        {
            originalColor = clipboardImage.color;
            originalAlpha = clipboardImage.color.a;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ��������� ���������
        if (clipboardImage != null)
        {
            if (useHoverColor)
            {
                clipboardImage.color = hoverColor;
            }
            else
            {
                var c = clipboardImage.color;
                c.a = hoverAlpha;
                clipboardImage.color = c;
            }
        }

        // �������� ������ � ���������
        if (rulesPanel != null)
            rulesPanel.SetActive(true);

        // ���������� ����� ������
        if (rulesText != null)
            rulesText.text = rulesContent;

        // ���� ��� ���������
        if (audioSource != null && hoverSound != null)
            audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ������� �����
        if (clipboardImage != null)
        {
            if (useHoverColor)
            {
                clipboardImage.color = originalColor;
            }
            else
            {
                var c = clipboardImage.color;
                c.a = originalAlpha;
                clipboardImage.color = c;
            }
        }

        // �������� ������ � ���������
        if (rulesPanel != null)
            rulesPanel.SetActive(false);
    }
}
