using UnityEngine;

public class PenController : MonoBehaviour
{
    public GameObject extraButtonsPanel;

    public void OnPenClicked()
    {
        if (extraButtonsPanel != null)
        {
            extraButtonsPanel.SetActive(true);
        }
    }
}