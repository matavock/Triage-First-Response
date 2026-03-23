using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DaySummaryUI : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text totalText;
    public TMP_Text correctText;
    public TMP_Text incorrectText;

    // Сцена, в которую перейти после экрана итогов
    public string nextSceneName = "MainMenu";

    void Start()
    {
        if (titleText != null)
            titleText.text = "День завершён";

        if (totalText != null)
            totalText.text = $"Всего пациентов: {DayStats.total}";

        if (correctText != null)
            correctText.text = $"Верных решений: {DayStats.correct}";

        if (incorrectText != null)
            incorrectText.text = $"Неверных решений: {DayStats.incorrect}";

        //int currentDay = 1; // для Day1, для Day2 будет 2 и т.д.
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 1);

        int unlocked = PlayerPrefs.GetInt("UnlockedDay", 1);

        if (currentDay >= unlocked)
        {
            int newUnlocked = currentDay + 1;
            PlayerPrefs.SetInt("UnlockedDay", currentDay + 1);
        }

        int nextDay = currentDay + 1;
        PlayerPrefs.SetInt("CurrentDay", nextDay);

        PlayerPrefs.Save();
    }

    public void OnNextButtonPressed()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("CurrentDay", 1);
        PlayerPrefs.SetInt("UnlockedDay", 1);
        PlayerPrefs.Save();

        // Перезагрузить сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}