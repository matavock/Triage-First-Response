using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DoctorHandbookHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public Image handbookImage;
    public GameObject rulesPanel;
    public TMP_Text rulesText;

    [TextArea]
    public string handbookContent;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hoverSound;
    public AudioMixerGroup sfxMixerGroup;

    [Header("Hover")]
    public Color hoverColor = new Color(0.62f, 0.62f, 0.62f, 1f);

    private Color originalColor;

    private void Start()
    {
        if (handbookImage == null)
            handbookImage = GetComponent<Image>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && sfxMixerGroup != null)
            audioSource.outputAudioMixerGroup = sfxMixerGroup;

        if (rulesPanel != null)
            rulesPanel.SetActive(false);

        if (handbookImage != null)
            originalColor = handbookImage.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (handbookImage != null)
            handbookImage.color = hoverColor;

        if (rulesPanel != null)
            rulesPanel.SetActive(true);

        if (rulesText != null)
            rulesText.text = handbookContent;

        if (audioSource != null && hoverSound != null)
            audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (handbookImage != null)
            handbookImage.color = originalColor;

        if (rulesPanel != null)
            rulesPanel.SetActive(false);
    }
}
