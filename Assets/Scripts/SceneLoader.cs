using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu() => SceneManager.LoadScene("MainMenu");
    public void LoadGame() => SceneManager.LoadScene("Day1");
    public void LoadSettings() => SceneManager.LoadScene("SettingsScene");
    public void QuitGame()
    {
        Debug.Log("Game closed");
        Application.Quit();
    }
    public void OpenDaySelect()
    {
        SceneManager.LoadScene("DaySelect");
    }
}