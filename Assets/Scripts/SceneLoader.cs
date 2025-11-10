using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu() => SceneManager.LoadScene("MainMenu");
    public void LoadGame() => SceneManager.LoadScene("Game");
    public void LoadSettings() => SceneManager.LoadScene("SettingsScene");
    public void QuitGame()
    {
        Debug.Log("Game closed");
        Application.Quit();
    }
}