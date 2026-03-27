using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Day1SummaryUI : MonoBehaviour
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

    // Сцена, в которую перейти после экрана итогов
    public string nextSceneName = "Day2";

    void Start()
    {
        winFlag = true;
        if (DayStats.depression > 0 && DayStats.wealth > 0)
        {
            if (titleText != null)
                titleText.text = "Поздравляем с завершением первого дня!";
        }
        else
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
            if (DayStats.depression <= 0)
                DayStats.depression = 0;
            if (DayStats.depression >= 100)
                DayStats.depression = 100;
            depressionText.text = $"Уровень счастья: {DayStats.depression}/100";
        }

        if (wealthText != null)
        {
            if (DayStats.wealth <= 0)
                DayStats.wealth = 0;
            if (DayStats.wealth >= 100)
                DayStats.wealth = 100;
            wealthText.text = $"Количество денег: {DayStats.wealth}/100";
        }

        if (DayStats.depression <= 0)
        {
            winLoseText.text = $"К сожалению, из-за постоянных моральных дилемм вы впали в тяжелую депрессию.\nПопробуйте еще раз!";
            winFlag = false;
        }

        if (DayStats.wealth <= 0)
        {
            winLoseText.text = $"К сожалению, из-за частых штрафов на работе вас уволили и вы влезли в долги.\nПопробуйте еще раз!";
            winFlag = false;
        }


        if (DayStats.depression <= 0 && DayStats.wealth <= 0)
        {
            winLoseText.text = $"К сожалению, из-за постоянных моральных дилемм, а также штрафов от начальства вас настигла смерть в нищете.\nПопробуйте еще раз!";
            winFlag = false;
        }

        if (!winFlag)
        {
            nextScene.text = "В МЕНЮ";
            nextSceneName = "MainMenu";
            PlayerPrefs.SetInt("Day1", 0);
            PlayerPrefs.SetInt("Day2", -1);
            PlayerPrefs.SetInt("Happiness", 50);
            PlayerPrefs.SetInt("Wealth", 50);
            PlayerPrefs.SetString("CurrentScene", "Day1");
            PlayerPrefs.Save();

        }
        else
        {
            nextScene.text = "ДЕНЬ 2";
            mainMenuText.text = "В МЕНЮ";
            nextSceneName = "Day2";
            PlayerPrefs.SetInt("Day1", 1);
            PlayerPrefs.SetInt("Day2", 0);
            PlayerPrefs.SetInt("Happiness", DayStats.depression);
            PlayerPrefs.SetInt("Wealth", DayStats.wealth);
            PlayerPrefs.SetString("CurrentScene", "Day2");
            PlayerPrefs.Save();
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

        // Перезагрузить сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}