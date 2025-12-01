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
    }

    public void OnNextButtonPressed()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}