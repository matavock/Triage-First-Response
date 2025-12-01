using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PatientImageController : MonoBehaviour
{
    [Header("UI")]
    public Image patientImage;

    [Header("Пациенты (спрайты)")]
    public Sprite[] patientSprites;        // изображения по порядку

    [Header("Правильные ответы")]
    public bool[] correctAnswers;          // true = должен быть принят, false = отказать

    [Header("Настройки")]
    public float delayBetweenPatients = 1.0f;
    public string summarySceneName = "DaySummary";

    [Header("Sound")]
    public AudioSource paperAudioSource;   // объект с AudioSource
    public AudioClip paperAppearSound;     // звук бумаги

    private int currentIndex = -1;
    private bool isSwitching = false;
    public string[] patientNames;
    public int[] patientAges;
    public string[] patientComplaints;
    public ProtocolPaper protocolPaper;
    public GameObject paperObject;
    private Coroutine paperCoroutine;
    public GameObject actionButtons;

    void Start()
    {
        DayStats.Reset();

        // общее число пациентов в дне
        DayStats.total = patientSprites.Length;

        // стартуем день с задержкой появления первого пациента
        StartCoroutine(DelayedStartFirstPatient());
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

        if (actionButtons != null)
        {
            actionButtons.SetActive(false);
        }
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
        // убираем кнопки admit reject
        if (actionButtons != null)
        {
            actionButtons.SetActive(false);
        }
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

        // ЗАПУСКАЕМ ЗАДЕРЖКУ НА ПОЯВЛЕНИЕ БУМАГИ
        paperCoroutine = StartCoroutine(ShowPaperWithDelay(0.35f));

        // ОБНОВЛЯЕМ БУМАГУ
        protocolPaper.SetPatientInfo(
            patientNames[currentIndex],
            patientAges[currentIndex],
            patientComplaints[currentIndex]
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