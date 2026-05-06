using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class EntryPermitData
{
    public bool hasPermit = true;
    public string fullName;
    public string medicalCardSerial;
    public string purpose;
    public string duration;
    public int durationDays;
    public string enterBy;
}

[Serializable]
public class PatientIdCardData
{
    public bool hasCard = true;
    public string fullName;
    public string gender;
    public string birthDate;
}

[Serializable]
public class FingerprintData
{
    public bool required;
    public string patientPrint;
    public string archivePrint;
    public string alias;
    public bool claimsNameChange;
}

public class Day4Controller : MonoBehaviour
{
    [Header("UI")]
    public Image patientImage;
    public Image dialogueImage;
    public Sprite[] patientSprites;
    public Sprite dialogueSprites;
    public string[] patientDialogues;
    public TMP_Text dialogue;

    [Header("Answers")]
    public bool[] correctAnswers;
    public int[] acceptDepressionValue;
    public int[] rejectDepressionValue;
    public int[] acceptWealthValue;
    public int[] rejectWealthValue;

    [Header("Settings")]
    public float delayBetweenPatients = 1.0f;
    public string summarySceneName = "Day4Summary";
    public string currentCalendarDate = "17.03.2026";
    public string localRegion = "Северодвинский регион";

    [Header("Sound")]
    public AudioSource paperAudioSource;
    public AudioClip paperAppearSound;
    public AudioMixerGroup sfxMixerGroup;

    [Header("Sliders")]
    public Slider depressionSlider;
    public Slider wealthSlider;

    [Header("Patients")]
    public string[] patientNames;
    public string[] patientPassportNames;
    public int[] patientAges;
    public string[] patientComplaints;
    public string[] patientSex;
    public string[] patientBirth;
    public MedicalRecordData[] medicalRecords;
    public EntryPermitData[] entryPermits;
    public PatientIdCardData[] idCards;
    public FingerprintData[] fingerprints;

    [Header("Documents")]
    public Paper Paper;
    public GameObject paperObject;
    public Passport Passport;
    public GameObject passportObject;
    public Day4Document entryPermitDocument;
    public GameObject entryPermitObject;
    public Day4Document idCardDocument;
    public GameObject idCardObject;
    public Day4Document fingerprintDocument;
    public GameObject fingerprintObject;
    public GameObject fingerprintButtonObject;
    public GameObject extraButtonsPanel;

    private int currentIndex = -1;
    private bool isSwitching;
    private Coroutine paperCoroutine;
    private Coroutine passportCoroutine;
    private Coroutine entryPermitCoroutine;
    private Coroutine idCardCoroutine;

    void Start()
    {
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 0);
        PlayerPrefs.SetString("CurrentScene", SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();

        DayStats.depression = PlayerPrefs.GetInt("Happiness", 50);
        DayStats.wealth = PlayerPrefs.GetInt("Wealth", 50);
        DayStats.Reset();
        DayStats.total = patientSprites.Length;

        if (paperAudioSource != null && sfxMixerGroup != null)
            paperAudioSource.outputAudioMixerGroup = sfxMixerGroup;

        StartCoroutine(DelayedStartFirstPatient());
    }

    private IEnumerator DelayedStartFirstPatient()
    {
        yield return new WaitForSeconds(1f);
        ShowNextPatient();
    }

    public void OnAdmitButtonPressed()
    {
        if (isSwitching) return;

        bool correct = IsCurrentPatientAcceptable();
        DayStats.depression += acceptDepressionValue[currentIndex];
        DayStats.wealth += acceptWealthValue[currentIndex];
        UpdateSliders();
        if (correct) DayStats.correct++;
        else DayStats.incorrect++;

        StartCoroutine(SwitchToNextPatient());
    }

    public void OnRejectButtonPressed()
    {
        if (isSwitching) return;

        bool correct = !IsCurrentPatientAcceptable();
        DayStats.depression += rejectDepressionValue[currentIndex];
        DayStats.wealth += rejectWealthValue[currentIndex];
        UpdateSliders();
        if (correct) DayStats.correct++;
        else DayStats.incorrect++;

        StartCoroutine(SwitchToNextPatient());
    }

    public void OnFingerprintButtonPressed()
    {
        if (fingerprintObject != null)
            fingerprintObject.SetActive(true);
    }

    private IEnumerator SwitchToNextPatient()
    {
        isSwitching = true;
        HidePatient();
        yield return new WaitForSeconds(delayBetweenPatients);
        ShowNextPatient();
        isSwitching = false;
    }

    private void ShowNextPatient()
    {
        currentIndex++;
        if (currentIndex >= patientSprites.Length)
        {
            EndDayAndGoToSummary();
            return;
        }

        StopDocumentCoroutines();
        HidePatient();

        patientImage.sprite = patientSprites[currentIndex];
        patientImage.color = Color.white;
        patientImage.gameObject.SetActive(true);

        dialogueImage.sprite = dialogueSprites;
        dialogueImage.color = Color.white;
        dialogueImage.gameObject.SetActive(true);
        dialogue.text = patientDialogues[currentIndex];
        dialogue.gameObject.SetActive(true);

        paperCoroutine = StartCoroutine(ShowObjectWithDelay(paperObject, 0.35f, true));
        passportCoroutine = StartCoroutine(ShowObjectWithDelay(passportObject, 0.70f, false));

        if (IsOutOfTown(currentIndex))
            entryPermitCoroutine = StartCoroutine(ShowObjectWithDelay(entryPermitObject, 1.05f, false));
        else
            idCardCoroutine = StartCoroutine(ShowObjectWithDelay(idCardObject, 1.05f, false));

        bool shouldAdmit = IsCurrentPatientAcceptable();
        Paper.SetPatientInfo(patientNames[currentIndex], patientAges[currentIndex], patientComplaints[currentIndex], shouldAdmit);
        Passport.SetPatientInfo(patientPassportNames[currentIndex], patientSex[currentIndex], patientBirth[currentIndex], GetMedicalRecord(currentIndex));
        UpdateDay4Documents();

        if (fingerprintButtonObject != null)
            fingerprintButtonObject.SetActive(true);
    }

    private IEnumerator ShowObjectWithDelay(GameObject target, float delay, bool playPaperSound)
    {
        yield return new WaitForSeconds(delay);
        if (target != null && patientImage.gameObject.activeSelf)
        {
            target.SetActive(true);
            if (playPaperSound && paperAudioSource != null && paperAppearSound != null)
                paperAudioSource.PlayOneShot(paperAppearSound);
        }
    }

    private void UpdateDay4Documents()
    {
        EntryPermitData permit = GetEntryPermit(currentIndex);
        if (entryPermitDocument != null)
            entryPermitDocument.SetDocument("ENTRY PERMIT", FormatEntryPermit(permit));

        PatientIdCardData idCard = GetIdCard(currentIndex);
        if (idCardDocument != null)
            idCardDocument.SetDocument("ID CARD", FormatIdCard(idCard));

        FingerprintData fingerprint = GetFingerprint(currentIndex);
        if (fingerprintDocument != null)
            fingerprintDocument.SetDocument("Fingerprint Archive", FormatFingerprint(fingerprint));
    }

    private bool IsCurrentPatientAcceptable()
    {
        return currentIndex >= 0
            && currentIndex < correctAnswers.Length
            && correctAnswers[currentIndex];
    }

    private bool IsMedicalRecordValid(int index)
    {
        MedicalRecordData record = GetMedicalRecord(index);
        return record != null
            && record.hasCard
            && GenderMatchesPatientData(index, record.gender)
            && IsAccreditedIssuer(record.region, record.issuingHospital)
            && IsExpirationDateValid(record.expirationDate);
    }

    private bool IsEntryPermitValid(int index)
    {
        MedicalRecordData record = GetMedicalRecord(index);
        EntryPermitData permit = GetEntryPermit(index);
        return record != null
            && permit != null
            && permit.hasPermit
            && NamesMatch(permit.fullName, record.fullName)
            && StringsMatch(permit.medicalCardSerial, record.serialNumber)
            && PurposeDurationValid(permit)
            && IsEnterByValid(permit.enterBy);
    }

    private bool IsIdCardValid(int index)
    {
        MedicalRecordData record = GetMedicalRecord(index);
        PatientIdCardData idCard = GetIdCard(index);
        return record != null
            && idCard != null
            && idCard.hasCard
            && NamesMatch(idCard.fullName, record.fullName)
            && NormalizeGender(idCard.gender) == NormalizeGender(record.gender)
            && DatesMatch(idCard.birthDate, patientBirth[index]);
    }

    private bool FingerprintRequired(int index)
    {
        FingerprintData fingerprint = GetFingerprint(index);
        return fingerprint != null && fingerprint.required;
    }

    private bool IsFingerprintValid(int index)
    {
        FingerprintData fingerprint = GetFingerprint(index);
        if (fingerprint == null || !fingerprint.required)
            return true;

        if (!StringsMatch(fingerprint.patientPrint, fingerprint.archivePrint))
            return false;

        if (!fingerprint.claimsNameChange)
            return true;

        string currentLastName = LastName(patientPassportNames[index]);
        return !string.IsNullOrWhiteSpace(currentLastName)
            && NormalizeText(fingerprint.alias).Contains(NormalizeText(currentLastName));
    }

    private bool IsOutOfTown(int index)
    {
        MedicalRecordData record = GetMedicalRecord(index);
        return record != null && NormalizeText(record.region) != NormalizeText(localRegion);
    }

    private MedicalRecordData GetMedicalRecord(int index) => GetArrayValue(medicalRecords, index);
    private EntryPermitData GetEntryPermit(int index) => GetArrayValue(entryPermits, index);
    private PatientIdCardData GetIdCard(int index) => GetArrayValue(idCards, index);
    private FingerprintData GetFingerprint(int index) => GetArrayValue(fingerprints, index);

    private T GetArrayValue<T>(T[] array, int index) where T : class
    {
        if (array == null || index < 0 || index >= array.Length)
            return null;

        return array[index];
    }

    private bool PurposeDurationValid(EntryPermitData permit)
    {
        string purpose = NormalizeText(permit.purpose);
        if (purpose == "EXAM")
            return permit.durationDays >= 2 && permit.durationDays <= 15;
        if (purpose == "TREATMENT")
            return permit.durationDays >= 15 && permit.durationDays <= 93;
        if (purpose == "REHAB")
            return permit.durationDays >= 30 && permit.durationDays <= 1095;
        if (purpose == "CARE")
            return NormalizeText(permit.duration) == "FOREVER";

        return false;
    }

    private bool IsExpirationDateValid(string expirationDate)
    {
        return TryParseDate(expirationDate, out DateTime expiresOn)
            && TryParseDate(currentCalendarDate, out DateTime currentDate)
            && expiresOn.Date > currentDate.Date;
    }

    private bool IsEnterByValid(string enterBy)
    {
        return TryParseDate(enterBy, out DateTime validUntil)
            && TryParseDate(currentCalendarDate, out DateTime currentDate)
            && validUntil.Date >= currentDate.Date;
    }

    private bool DatesMatch(string left, string right)
    {
        return TryParseDate(left, out DateTime leftDate)
            && TryParseDate(right, out DateTime rightDate)
            && leftDate.Date == rightDate.Date;
    }

    private bool TryParseDate(string value, out DateTime date)
    {
        string[] formats = { "dd.MM.yyyy", "d.MM.yyyy", "dd.M.yyyy", "d.M.yyyy", "yyyy-MM-dd" };
        return DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
            || DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    }

    private bool GenderMatchesPatientData(int index, string gender)
    {
        if (patientSex == null || index < 0 || index >= patientSex.Length)
            return false;

        return NormalizeGender(gender) == NormalizeGender(patientSex[index]);
    }

    private string NormalizeGender(string gender)
    {
        string value = NormalizeText(gender);
        if (value.StartsWith("М") || value.StartsWith("M"))
            return "M";
        if (value.StartsWith("Ж") || value.StartsWith("F") || value.StartsWith("W"))
            return "F";

        return string.Empty;
    }

    private bool NamesMatch(string left, string right)
    {
        string[] leftParts = NormalizeText(left).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        string[] rightParts = NormalizeText(right).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (leftParts.Length != rightParts.Length)
            return false;

        Array.Sort(leftParts);
        Array.Sort(rightParts);
        return string.Join(" ", leftParts) == string.Join(" ", rightParts);
    }

    private bool StringsMatch(string left, string right)
    {
        return NormalizeText(left) == NormalizeText(right);
    }

    private string LastName(string fullName)
    {
        string[] parts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length > 0 ? parts[parts.Length - 1] : string.Empty;
    }

    private string NormalizeText(string value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.Trim().Replace("  ", " ").ToUpperInvariant();
    }

    private bool IsAccreditedIssuer(string region, string issuingHospital)
    {
        return AccreditedClinics.TryGetValue(NormalizeText(region), out HashSet<string> clinics)
            && clinics.Contains(NormalizeText(issuingHospital));
    }

    private string FormatEntryPermit(EntryPermitData permit)
    {
        if (permit == null || !permit.hasPermit)
            return "ENTRY PERMIT: отсутствует";

        return "ФИО: " + permit.fullName
            + "\nНомер карты: " + permit.medicalCardSerial
            + "\nPurpose: " + permit.purpose
            + "\nDuration: " + permit.duration
            + "\nEnter by: " + permit.enterBy;
    }

    private string FormatIdCard(PatientIdCardData idCard)
    {
        if (idCard == null || !idCard.hasCard)
            return "ID CARD: отсутствует";

        return "ФИО: " + idCard.fullName
            + "\nПол: " + idCard.gender
            + "\nДата рождения: " + idCard.birthDate;
    }

    private string FormatFingerprint(FingerprintData fingerprint)
    {
        if (fingerprint == null || !fingerprint.required)
            return "Биометрическая сверка не требуется";

        return "Отпечатки пациента: " + fingerprint.patientPrint
            + "\nАрхив Минздрава: " + fingerprint.archivePrint
            + "\nAlias: " + fingerprint.alias;
    }

    private void HidePatient()
    {
        SetActive(patientImage != null ? patientImage.gameObject : null, false);
        SetActive(dialogueImage != null ? dialogueImage.gameObject : null, false);
        SetActive(dialogue != null ? dialogue.gameObject : null, false);
        SetActive(paperObject, false);
        SetActive(passportObject, false);
        SetActive(entryPermitObject, false);
        SetActive(idCardObject, false);
        SetActive(fingerprintObject, false);
        SetActive(extraButtonsPanel, false);
    }

    private void SetActive(GameObject target, bool active)
    {
        if (target != null)
            target.SetActive(active);
    }

    private void UpdateSliders()
    {
        if (depressionSlider != null)
            depressionSlider.value = DayStats.depression;
        if (wealthSlider != null)
            wealthSlider.value = DayStats.wealth;
    }

    private void StopDocumentCoroutines()
    {
        StopIfRunning(ref paperCoroutine);
        StopIfRunning(ref passportCoroutine);
        StopIfRunning(ref entryPermitCoroutine);
        StopIfRunning(ref idCardCoroutine);
    }

    private void StopIfRunning(ref Coroutine coroutine)
    {
        if (coroutine == null)
            return;

        StopCoroutine(coroutine);
        coroutine = null;
    }

    private void EndDayAndGoToSummary()
    {
        HidePatient();
        SceneManager.LoadScene(summarySceneName);
    }

    private static readonly Dictionary<string, HashSet<string>> AccreditedClinics = new Dictionary<string, HashSet<string>>
    {
        { "СЕВЕРОДВИНСКИЙ РЕГИОН", new HashSet<string> { "МСЧ \"ПОЛЯРНАЯ\"", "КБ-41 \"МОРОЗ\"", "БОЛЬНИЦА ИМЕНИ ХОЛОДА" } },
        { "ЗАЛЕССКИЙ РЕГИОН", new HashSet<string> { "ДУБРАВА ЦРБ", "ПОЛИКЛИНИКА № 9", "САНАТОРИЙ \"ИСТОК\"" } },
        { "ПРИГОРСКИЙ ОКРУГ", new HashSet<string> { "МЕДЦЕНТР \"ВЫСОТКА\"", "КЛИНИКА \"ХРЕБЕТ\"", "МСЧ \"ГРАНИТ\"" } },
        { "МЕЖДУРЕЧЕНСКИЙ УЗЕЛ", new HashSet<string> { "ЗАВОДСКАЯ БОЛЬНИЦА", "КБ-12 \"ПРИЧАЛ\"", "МЕДЦЕНТР \"РУСЛО\"" } },
        { "СТЕПНОВСКАЯ ГУБЕРНИЯ", new HashSet<string> { "КОВЫЛЬСКАЯ БОЛЬНИЦА", "КБ-5 \"МАРЕВО\"", "МЕДСАНЧАСТЬ \"ЗАКАТ\"" } },
        { "ОЗЕРСКИЙ ПРОТЕКТОРАТ", new HashSet<string> { "НИИ \"ГЛУБИНА\"", "КЛИНИКА \"РЯБЬ\"", "МСЧ \"КАМЫШ\"" } }
    };
}
