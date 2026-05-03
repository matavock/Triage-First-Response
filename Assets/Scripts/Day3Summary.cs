using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Day3SummaryUI : MonoBehaviour
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
    public string nextSceneName = "Day4";

    void Start()
    {
        winFlag = true;

        if (DayStats.depression > 0 && DayStats.wealth > 0)
        {
            if (titleText != null)
                titleText.text = "Поздравляем с завершением третьего дня!";
        }
        else if (titleText != null)
        {
            titleText.text = "День завершен";
        }

        if (totalText != null)
            totalText.text = $"Всего пациентов: {DayStats.total}";

        if (correctText != null)
            correctText.text = $"Верных решений: {DayStats.correct}";

        if (incorrectText != null)
            incorrectText.text = $"Неверных решений: {DayStats.incorrect}";

        if (depressionText != null)
        {
            DayStats.depression = Mathf.Clamp(DayStats.depression, 0, 100);
            depressionText.text = $"Уровень счастья: {DayStats.depression}/100";
        }

        if (wealthText != null)
        {
            DayStats.wealth = Mathf.Clamp(DayStats.wealth, 0, 100);
            wealthText.text = $"Количество денег: {DayStats.wealth}/100";
        }

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
            PlayerPrefs.SetInt("Day3", 0);
            PlayerPrefs.SetInt("Day4", -1);
            PlayerPrefs.SetInt("Happiness", 50);
            PlayerPrefs.SetInt("Wealth", 50);
            PlayerPrefs.SetString("CurrentScene", "Day1");
        }
        else
        {
            if (nextScene != null)
                nextScene.text = "ДЕНЬ 4";
            if (mainMenuText != null)
                mainMenuText.text = "В МЕНЮ";

            nextSceneName = "Day4";
            PlayerPrefs.SetInt("Day3", 1);
            PlayerPrefs.SetInt("Day4", 0);
            PlayerPrefs.SetInt("Happiness", DayStats.depression);
            PlayerPrefs.SetInt("Wealth", DayStats.wealth);
            PlayerPrefs.SetString("CurrentScene", "Day4");
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

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
