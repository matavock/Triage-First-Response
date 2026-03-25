using UnityEngine;
using UnityEngine.InputSystem;

public class PenController : MonoBehaviour
{
    public GameObject extraButtonsPanel;
    public Key hotkey = Key.Tab;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current[hotkey].wasPressedThisFrame)
        {
            OnPenClicked();
        }
    }

    public void OnPenClicked()
    {
        if (extraButtonsPanel != null)
        {
            extraButtonsPanel.SetActive(true);
        }
    }
}