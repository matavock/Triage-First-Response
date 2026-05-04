using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Day4SummaryUI : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text totalText;
    public TMP_Text correctText;
    public TMP_Text incorrectText;
    public TMP_Text depressionText;
    public TMP_Text wealthText;
    public TMP_Text winLoseText;
    public TMP_Text nextScene;
    public TMP_Text mainMenuText;

    public bool winFlag = true;
    public string nextSceneName = "Day5";

    void Start()
    {
        winFlag = true;

        if (titleText != null)
            titleText.text = DayStats.depression > 0 && DayStats.wealth > 0
                ? "Поздравляем с завершением четвертого дня!"
                : "День завершен";

        if (totalText != null)
            totalText.text = $"Всего пациентов: {DayStats.total}";
        if (correctText != null)
            correctText.text = $"Верных решений: {DayStats.correct}";
        if (incorrectText != null)
            incorrectText.text = $"Неверных решений: {DayStats.incorrect}";

        DayStats.depression = Mathf.Clamp(DayStats.depression, 0, 100);
        DayStats.wealth = Mathf.Clamp(DayStats.wealth, 0, 100);

        if (depressionText != null)
            depressionText.text = $"Уровень счастья: {DayStats.depression}/100";
        if (wealthText != null)
            wealthText.text = $"Количество денег: {DayStats.wealth}/100";

        if (DayStats.depression <= 0)
        {
            if (winLoseText != null)
                winLoseText.text = "К сожалению, из-за постоянных моральных дилемм вы впали в тяжелую депрессию.\nПопробуйте еще раз!";
            winFlag = false;
        }

        if (DayStats.wealth <= 0)
        {
            if (winLoseText != null)
                winLoseText.text = "К сожалению, из-за частых штрафов на работе вас уволили и вы влезли в долги.\nПопробуйте еще раз!";
            winFlag = false;
        }

        if (DayStats.depression <= 0 && DayStats.wealth <= 0)
        {
            if (winLoseText != null)
                winLoseText.text = "К сожалению, из-за постоянных моральных дилемм, а также штрафов от начальства вас настигла смерть в нищете.\nПопробуйте еще раз!";
            winFlag = false;
        }

        if (!winFlag)
        {
            if (nextScene != null)
                nextScene.text = "В МЕНЮ";

            nextSceneName = "MainMenu";
            PlayerPrefs.SetInt("Day4", 0);
            PlayerPrefs.SetInt("Day5", -1);
            PlayerPrefs.SetInt("Happiness", 50);
            PlayerPrefs.SetInt("Wealth", 50);
            PlayerPrefs.SetString("CurrentScene", "Day1");
        }
        else
        {
            if (nextScene != null)
                nextScene.text = "ДЕНЬ 5";
            if (mainMenuText != null)
                mainMenuText.text = "В МЕНЮ";

            nextSceneName = "Day5";
            PlayerPrefs.SetInt("Day4", 1);
            PlayerPrefs.SetInt("Day5", 0);
            PlayerPrefs.SetInt("Happiness", DayStats.depression);
            PlayerPrefs.SetInt("Wealth", DayStats.wealth);
            PlayerPrefs.SetString("CurrentScene", "Day5");
        }

        PlayerPrefs.Save();
    }

    public void OnNextButtonPressed()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void OnMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
