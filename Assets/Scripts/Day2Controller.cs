using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Globalization;
using System;
using Unity.VectorGraphics;

    [Serializable]
    public class MedicalRecordData
    {
        public string fullName;
        public string serialNumber;
        public bool hasCard = true;
        public string gender;
        public string region;
        public string issuingHospital;
        public string expirationDate;
    }

public class Day2Controller : MonoBehaviour
{
    [Header("UI")]
    public Image patientImage;
    public Image dialogueImage;

    [Header("�������� (�������) � ���� �������")]
    public Sprite[] patientSprites;        // ����������� �� �������
    public Sprite dialogueSprites;

    [Header("�������� (�������)")]
    public string[] patientDialogues;
    public TMP_Text dialogue;

    [Header("���������� ������")]
    public bool[] correctAnswers;          // true = ������ ���� ������, false = ��������

    [Header("�������� (��������� ��������)")]
    public int[] acceptDepressionValue;

    [Header("�������� (��������� ������)")]
    public int[] rejectDepressionValue;

    [Header("�������� (�������� ���� ��������)")]
    public int[] acceptWealthValue;

    [Header("�������� (�������� ���� ������)")]
    public int[] rejectWealthValue;

    [Header("���������")]
    public float delayBetweenPatients = 1.0f;
    public string summarySceneName = "DaySummary";

    [Header("Sound")]
    public AudioSource paperAudioSource;   // ������ � AudioSource
    public AudioClip paperAppearSound;     // ���� ������
    public AudioMixerGroup sfxMixerGroup;
    /*
    public AudioSource passportAudioSource;   // ������ � AudioSource
    public AudioClip passportAppearSound;     // ���� ��������
    */

    [Header("Sliders")]
    public Slider depressionSlider;   // ������ � AudioSource
    public Slider wealthSlider;

    private int currentIndex = -1;
    private bool isSwitching = false;
    public string[] patientNames;
    public string[] patientPassportNames;
    public int[] patientAges;
    public string[] patientComplaints;
    public string[] patientSex;

    [Header("Медкарты")]
    public bool checkMedicalRecords = true;
    public string currentCalendarDate = "03.05.2026";
    public MedicalRecordData[] medicalRecords;
    public string[] patientBirth;

    public Paper Paper;
    public GameObject paperObject;
    private Coroutine paperCoroutine;

    public Passport Passport;
    public GameObject passportObject;
    private Coroutine passportCoroutine;


    public GameObject extraButtonsPanel;

    void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 0);
        PlayerPrefs.SetString("CurrentScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();

        DayStats.depression = PlayerPrefs.GetInt("Happiness", 50);
        DayStats.wealth = PlayerPrefs.GetInt("Wealth", 50);
        DayStats.Reset();

        // ����� ����� ��������� � ���
        DayStats.total = patientSprites.Length;


        // �������� ���� � ��������� ��������� ������� ��������
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
    //      ������ "�������"
    // ----------------------------
    public void OnAdmitButtonPressed()
    {
        if (isSwitching) return;

        bool correct = IsCurrentPatientAcceptable() == true;
        DayStats.depression += acceptDepressionValue[currentIndex];
        depressionSlider.value = DayStats.depression;
        //DayStats.depression += acceptDepressionValue[currentIndex];
        DayStats.wealth += acceptWealthValue[currentIndex];
        wealthSlider.value = DayStats.wealth;
        //DayStats.wealth += acceptWealthValue[currentIndex];
        if (correct) DayStats.correct++;
        else DayStats.incorrect++;

        StartCoroutine(SwitchToNextPatient());
    }

    // ----------------------------
    //      ������ "��������"
    // ----------------------------
    public void OnRejectButtonPressed()
    {
        if (isSwitching) return;

        bool correct = IsCurrentPatientAcceptable() == false;
        DayStats.depression += rejectDepressionValue[currentIndex];
        depressionSlider.value = DayStats.depression;
        DayStats.wealth += rejectWealthValue[currentIndex];
        wealthSlider.value = DayStats.wealth;
        if (correct) DayStats.correct++;
        else DayStats.incorrect++;

        StartCoroutine(SwitchToNextPatient());
    }

    // ������ �������� � ������ ��������
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

        if (passportObject != null)
            passportObject.SetActive(false);

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

        // ���������� ������
        if (paperObject != null && patientImage.gameObject.activeSelf)
        {
            paperObject.SetActive(true);

            // ������������� ���� ���� ��� � ��� ���������
            if (paperAudioSource != null && paperAppearSound != null)
            {
                paperAudioSource.PlayOneShot(paperAppearSound);
            }
        }

        paperCoroutine = null;
    }

    private IEnumerator ShowPassportWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ���������� ������
        if (passportObject != null && patientImage.gameObject.activeSelf)
        {
            passportObject.SetActive(true);
            /*
            // ������������� ���� ���� ��� � ��� ���������
            if (paperAudioSource != null && paperAppearSound != null)
            {
                paperAudioSource.PlayOneShot(paperAppearSound);
            }
            */

        }

        passportCoroutine = null;
    }

    private void ShowNextPatient()
    {
        currentIndex++;

        // ��� �������� �����������
        if (currentIndex >= patientSprites.Length)
        {
            EndDayAndGoToSummary();
            return;
        }

        // �������� ������ ������ ��� ������ �������
        if (extraButtonsPanel != null)
            extraButtonsPanel.SetActive(false);

        // �������� ������ �������� ��������� ������, ���� ��� ��������
        if (paperCoroutine != null)
        {
            StopCoroutine(paperCoroutine);
            paperCoroutine = null;
        }

        // �������� ������ �������� ��������� ���������, ���� ��� ��������
        if (paperCoroutine != null)
        {
            StopCoroutine(paperCoroutine);
            paperCoroutine = null;
        }

        // �������� ������ ����� ������� ������ ��������
        if (paperObject != null)
            paperObject.SetActive(false);

        // �������� ������ ����� ������� ������ ��������
        if (passportObject != null)
            passportObject.SetActive(false);

        // ���������� ������ ��������
        patientImage.sprite = patientSprites[currentIndex];
        patientImage.color = new Color(1f, 1f, 1f, 1f);
        patientImage.gameObject.SetActive(true);

        // ���������� ������ ������ ��������
        dialogueImage.sprite = dialogueSprites;
        dialogue.text = patientDialogues[currentIndex];
        dialogueImage.color = new Color(1f, 1f, 1f, 1f);
        dialogueImage.gameObject.SetActive(true);
        dialogue.gameObject.SetActive(true);

        // ��������� �������� �� ��������� ������
        paperCoroutine = StartCoroutine(ShowPaperWithDelay(0.35f));

        // ��������� �������� �� ��������� ��������
        passportCoroutine = StartCoroutine(ShowPassportWithDelay(0.70f));

        // ��������� ������
        Paper.SetPatientInfo(
            patientNames[currentIndex],
            patientAges[currentIndex],
            patientComplaints[currentIndex],
            IsCurrentPatientAcceptable()
        );

        // ��������� �������
        Passport.SetPatientInfo(
            patientPassportNames[currentIndex],
            patientSex[currentIndex],
            patientBirth[currentIndex],
            GetMedicalRecord(currentIndex)
        );
    }
    private bool IsCurrentPatientAcceptable()
    {
        bool baseAnswer = currentIndex >= 0 && currentIndex < correctAnswers.Length && correctAnswers[currentIndex];

        if (!checkMedicalRecords || !HasConfiguredMedicalRecords())
            return baseAnswer;

        return baseAnswer && IsMedicalRecordValid(currentIndex);
    }

    private bool HasConfiguredMedicalRecords()
    {
        return medicalRecords != null && medicalRecords.Length > 0;
    }

    private MedicalRecordData GetMedicalRecord(int index)
    {
        if (!HasConfiguredMedicalRecords() || index < 0 || index >= medicalRecords.Length)
            return null;

        return medicalRecords[index];
    }

    private bool IsMedicalRecordValid(int index)
    {
        MedicalRecordData record = GetMedicalRecord(index);
        if (record == null || !record.hasCard)
            return false;

        return GenderMatchesAppearance(index, record.gender)
            && IsAccreditedIssuer(record.region, record.issuingHospital)
            && IsExpirationDateValid(record.expirationDate);
    }

    private bool GenderMatchesAppearance(int index, string gender)
    {
        string normalizedGender = NormalizeGender(gender);
        if (string.IsNullOrEmpty(normalizedGender))
            return false;

        string appearanceGender = GetAppearanceGender(index);
        return !string.IsNullOrEmpty(appearanceGender) && normalizedGender == appearanceGender;
    }

    private string GetAppearanceGender(int index)
    {
        if (patientSprites == null || index < 0 || index >= patientSprites.Length || patientSprites[index] == null)
            return string.Empty;

        string spriteName = patientSprites[index].name;
        if (spriteName.IndexOf("Woman", StringComparison.OrdinalIgnoreCase) >= 0)
            return "F";
        if (spriteName.IndexOf("Man", StringComparison.OrdinalIgnoreCase) >= 0)
            return "M";

        return string.Empty;
    }

    private string NormalizeGender(string gender)
    {
        if (string.IsNullOrWhiteSpace(gender))
            return string.Empty;

        string value = gender.Trim();
        string upperValue = value.ToUpperInvariant();
        if (upperValue.StartsWith("М") || upperValue.StartsWith("M"))
            return "M";
        if (upperValue.StartsWith("Ж") || upperValue.StartsWith("F") || upperValue.StartsWith("W"))
            return "F";

        if (value.Equals("M", StringComparison.OrdinalIgnoreCase) || value.Equals("Male", StringComparison.OrdinalIgnoreCase) || value.Equals("Man", StringComparison.OrdinalIgnoreCase) || value.Equals("М", StringComparison.OrdinalIgnoreCase) || value.Equals("Мужской", StringComparison.OrdinalIgnoreCase))
            return "M";
        if (value.Equals("F", StringComparison.OrdinalIgnoreCase) || value.Equals("Female", StringComparison.OrdinalIgnoreCase) || value.Equals("Woman", StringComparison.OrdinalIgnoreCase) || value.Equals("Ж", StringComparison.OrdinalIgnoreCase) || value.Equals("Женский", StringComparison.OrdinalIgnoreCase))
            return "F";

        return string.Empty;
    }

    private bool IsAccreditedIssuer(string region, string issuingHospital)
    {
        if (string.IsNullOrWhiteSpace(region) || string.IsNullOrWhiteSpace(issuingHospital))
            return false;

        string normalizedRegion = NormalizeMedicalRecordText(region);
        string normalizedIssuer = NormalizeMedicalRecordText(issuingHospital);

        return AccreditedClinics.TryGetValue(normalizedRegion, out HashSet<string> clinics)
            && clinics.Contains(normalizedIssuer);
    }

    private bool IsExpirationDateValid(string expirationDate)
    {
        if (!TryParseMedicalDate(expirationDate, out DateTime expiresOn))
            return false;

        if (!TryParseMedicalDate(currentCalendarDate, out DateTime currentDate))
            currentDate = DateTime.Today;

        return expiresOn.Date > currentDate.Date;
    }

    private bool TryParseMedicalDate(string value, out DateTime date)
    {
        string[] formats =
        {
            "dd.MM.yyyy",
            "d.MM.yyyy",
            "dd.M.yyyy",
            "d.M.yyyy",
            "yyyy-MM-dd",
            "MM/dd/yyyy",
            "M/d/yyyy"
        };

        return DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
            || DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    }

    private static string NormalizeMedicalRecordText(string value)
    {
        return value.Trim()
            .Replace("“", "\"")
            .Replace("”", "\"")
            .Replace("«", "\"")
            .Replace("»", "\"")
            .Replace("  ", " ")
            .ToUpperInvariant();
    }

    private static readonly Dictionary<string, HashSet<string>> AccreditedClinics = new Dictionary<string, HashSet<string>>
    {
        { NormalizeMedicalRecordText("Северодвинский регион"), new HashSet<string> { NormalizeMedicalRecordText("МСЧ \"Полярная\""), NormalizeMedicalRecordText("КБ-41 \"Мороз\""), NormalizeMedicalRecordText("Больница имени Холода") } },
        { NormalizeMedicalRecordText("Залесский регион"), new HashSet<string> { NormalizeMedicalRecordText("Дубрава ЦРБ"), NormalizeMedicalRecordText("Поликлиника № 9"), NormalizeMedicalRecordText("Санаторий \"Исток\"") } },
        { NormalizeMedicalRecordText("Пригорский округ"), new HashSet<string> { NormalizeMedicalRecordText("Медцентр \"Высотка\""), NormalizeMedicalRecordText("Клиника \"Хребет\""), NormalizeMedicalRecordText("МСЧ \"Гранит\"") } },
        { NormalizeMedicalRecordText("Междуреченский узел"), new HashSet<string> { NormalizeMedicalRecordText("Заводская больница"), NormalizeMedicalRecordText("КБ-12 \"Причал\""), NormalizeMedicalRecordText("Медцентр \"Русло\"") } },
        { NormalizeMedicalRecordText("Степновская губерния"), new HashSet<string> { NormalizeMedicalRecordText("Ковыльская больница"), NormalizeMedicalRecordText("КБ-5 \"Марево\""), NormalizeMedicalRecordText("Медсанчасть \"Закат\"") } },
        { NormalizeMedicalRecordText("Озерский протекторат"), new HashSet<string> { NormalizeMedicalRecordText("НИИ \"Глубина\""), NormalizeMedicalRecordText("Клиника \"Рябь\""), NormalizeMedicalRecordText("МСЧ \"Камыш\"") } },
        { NormalizeMedicalRecordText("Острожский сектор"), new HashSet<string> { NormalizeMedicalRecordText("Тюремная МСЧ"), NormalizeMedicalRecordText("КБ-88 \"Заслон\""), NormalizeMedicalRecordText("Больница \"Вече\"") } }
    };

    private void EndDayAndGoToSummary()
    {
        if (patientImage != null)
            patientImage.gameObject.SetActive(false);

        if (paperObject != null)
            paperObject.SetActive(false);

        if (passportObject != null)
            passportObject.SetActive(false);

        SceneManager.LoadScene(summarySceneName);
    }
}
