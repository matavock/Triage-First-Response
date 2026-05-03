using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Unity.VectorGraphics;

public class Day1Controller : MonoBehaviour
{
    [Header("UI")]
    public Image patientImage;
    public Image dialogueImage;

    [Header("Пациенты (спрайты) и окно диалога")]
    public Sprite[] patientSprites;        // изображения по порядку
    public Sprite dialogueSprites;

    [Header("Пациенты (диалоги)")]
    public string[] patientDialogues;
    public TMP_Text dialogue;

    [Header("Правильные ответы")]
    public bool[] correctAnswers;          // true = должен быть принят, false = отказать

    [Header("Пациенты (Счастье принятия)")]
    public int[] acceptDepressionValue;

    [Header("Пациенты (Счастье отказа)")]
    public int[] rejectDepressionValue;

    [Header("Пациенты (Денежная цена принятия)")]
    public int[] acceptWealthValue;

    [Header("Пациенты (Денежная цена отказа)")]
    public int[] rejectWealthValue;

    [Header("Настройки")]
    public float delayBetweenPatients = 1.0f;
    public string summarySceneName = "Day1Summary";

    [Header("Sound")]
    public AudioSource paperAudioSource;   // объект с AudioSource
    public AudioClip paperAppearSound;     // звук бумаги
    public AudioMixerGroup sfxMixerGroup;
    /*
    public AudioSource passportAudioSource;   // объект с AudioSource
    public AudioClip passportAppearSound;     // звук паспорта
    */

    [Header("Sliders")]
    public Slider depressionSlider;   // объект с AudioSource
    public Slider wealthSlider;

    private int currentIndex = -1;
    private bool isSwitching = false;
    public string[] patientNames;
    public int[] patientAges;
    public string[] patientComplaints;

    public Paper Paper;
    public GameObject paperObject;
    private Coroutine paperCoroutine;
    
    public GameObject extraButtonsPanel;

    void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        PlayerPrefs.SetString("CurrentScene", SceneManager.GetActiveScene().name);
        
        PlayerPrefs.SetInt("Happiness", 50);
        PlayerPrefs.SetInt("Wealth", 50);
        PlayerPrefs.Save();
        DayStats.depression = PlayerPrefs.GetInt("Happiness", 50);
        DayStats.wealth = PlayerPrefs.GetInt("Wealth", 50);

        DayStats.Reset();
        // общее число пациентов в дне
        DayStats.total = patientSprites.Length;

        
        // стартуем день с задержкой появления первого пациента
        StartCoroutine(DelayedStartFirstPatient());

        if (paperAudioSource != null && sfxMixerGroup != null)
            paperAudioSource.outputAudioMixerGroup = sfxMixerGroup;
    }

    private IEnumerator DelayedStartFirstPatient()
    {
        yield return new WaitForSeconds(1f);
        ShowNextPatient();
    }

    // ----------------------------
    //      КНОПКА "ПРИНЯТЬ"
    // ----------------------------
    public void OnAdmitButtonPressed()
    {
        if (isSwitching) return;

        bool correct = correctAnswers[currentIndex] == true;
        DayStats.depression += acceptDepressionValue[currentIndex];
        DayStats.wealth += acceptWealthValue[currentIndex];

        if (DayStats.depression <= 0)
            DayStats.depression = 0;
        if (DayStats.wealth <= 0)
            DayStats.wealth = 0;

        depressionSlider.value = DayStats.depression;
        wealthSlider.value = DayStats.wealth;
        //DayStats.wealth += acceptWealthValue[currentIndex];
        if (correct) DayStats.correct++;
        else DayStats.incorrect++;

        StartCoroutine(SwitchToNextPatient());
    }

    // ----------------------------
    //      КНОПКА "ОТКАЗАТЬ"
    // ----------------------------
    public void OnRejectButtonPressed()
    {
        if (isSwitching) return;

        bool correct = correctAnswers[currentIndex] == false;
        DayStats.depression += rejectDepressionValue[currentIndex];
        DayStats.wealth += rejectWealthValue[currentIndex];

        if (DayStats.depression <= 0)
            DayStats.depression = 0;
        if (DayStats.wealth <= 0)
            DayStats.wealth = 0;

        depressionSlider.value = DayStats.depression;
        wealthSlider.value = DayStats.wealth;
        if (correct) DayStats.correct++;
        else DayStats.incorrect++;

        StartCoroutine(SwitchToNextPatient());
    }

    // логика перехода к новому пациенту
    private IEnumerator SwitchToNextPatient()
    {
        isSwitching = true;

        HidePatientImage();
        yield return new WaitForSeconds(delayBetweenPatients);

        ShowNextPatient();
        isSwitching = false;
    }

    private void HidePatientImage()
    {
        if (patientImage != null)
            patientImage.gameObject.SetActive(false);

        if (paperObject != null)
            paperObject.SetActive(false);

        if (extraButtonsPanel != null)
            extraButtonsPanel.SetActive(false);

        if (dialogueImage != null)
            dialogueImage.gameObject.SetActive(false);

        if (dialogue != null)
            dialogue.gameObject.SetActive(false);
    }

    private IEnumerator ShowPaperWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // показываем бумагу
        if (paperObject != null && patientImage.gameObject.activeSelf)
        {
            paperObject.SetActive(true);

            // Воспроизвести звук ОДИН раз — при появлении
            if (paperAudioSource != null && paperAppearSound != null)
            {
                paperAudioSource.PlayOneShot(paperAppearSound);
            }
        }

        paperCoroutine = null;
    }

    private void ShowNextPatient()
    {
        currentIndex++;

        // все пациенты закончились
        if (currentIndex >= patientSprites.Length)
        {
            EndDayAndGoToSummary();
            return;
        }

        // СКРЫВАЕМ ПАНЕЛЬ КНОПОК ДЛЯ НОВОГО КЛИЕНТА
        if (extraButtonsPanel != null)
            extraButtonsPanel.SetActive(false);

        // ОТМЕНЯЕМ старую корутину появления бумаги, если ещё работала
        if (paperCoroutine != null)
        {
            StopCoroutine(paperCoroutine);
            paperCoroutine = null;
        }

        // СКРЫВАЕМ бумагу перед показом нового пациента
        if (paperObject != null)
            paperObject.SetActive(false);

        // ПОКАЗЫВАЕМ НОВОГО ПАЦИЕНТА
        patientImage.sprite = patientSprites[currentIndex];
        patientImage.color = new Color(1f, 1f, 1f, 1f);
        patientImage.gameObject.SetActive(true);

        // ПОКАЗЫВАЕМ ДИАЛОГ НОВОГО ПАЦИЕНТА
        dialogueImage.sprite = dialogueSprites;
        dialogue.text = patientDialogues[currentIndex];
        dialogueImage.color = new Color(1f, 1f, 1f, 1f);
        dialogueImage.gameObject.SetActive(true);
        dialogue.gameObject.SetActive(true);

        // ЗАПУСКАЕМ ЗАДЕРЖКУ НА ПОЯВЛЕНИЕ БУМАГИ
        paperCoroutine = StartCoroutine(ShowPaperWithDelay(0.35f));

        // ОБНОВЛЯЕМ БУМАГУ
        Paper.SetPatientInfo(
            patientNames[currentIndex],
            patientAges[currentIndex],
            patientComplaints[currentIndex],
            correctAnswers[currentIndex]
        );
    }

    private void EndDayAndGoToSummary()
    {
        if (patientImage != null)
            patientImage.gameObject.SetActive(false);

        if (paperObject != null)
            paperObject.SetActive(false);

        SceneManager.LoadScene(summarySceneName);
    }
}