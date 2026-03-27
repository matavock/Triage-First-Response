using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeController : MonoBehaviour
{
    [Header("UI ссылки")]
    public GameObject blurPanel;          // BlurPanel (затемнение/блюр фона)
    public GameObject modalPanel;
    public Button exitButton;
    public void OnMenuButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
