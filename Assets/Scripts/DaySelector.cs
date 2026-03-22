using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DaySelectController : MonoBehaviour
{
    public Button[] dayButtons;

    void Start()
    {
        UpdateButtons();
    }

    void UpdateButtons()
    {
        int unlockedDay = PlayerPrefs.GetInt("UnlockedDay", 1);

        for (int i = 0; i < dayButtons.Length; i++)
        {
            int dayIndex = i + 1;

            if (dayIndex <= unlockedDay)
                dayButtons[i].interactable = true;
            else
                dayButtons[i].interactable = false;
        }
    }

    public void LoadDay(int day)
    {
        SceneManager.LoadScene("Day" + day);
    }
}