using UnityEngine;
using UnityEngine.UI;

public class InstantAnswerFeedbackToggle : MonoBehaviour
{
    private const string PrefKey = "InstantAnswerFeedback";

    public Toggle toggle;

    private void Awake()
    {
        if (toggle == null)
            toggle = GetComponent<Toggle>();
    }

    private void OnEnable()
    {
        if (toggle == null)
            return;

        toggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt(PrefKey, 0) == 1);
        toggle.onValueChanged.AddListener(SetInstantAnswerFeedback);
    }

    private void OnDisable()
    {
        if (toggle != null)
            toggle.onValueChanged.RemoveListener(SetInstantAnswerFeedback);
    }

    private void SetInstantAnswerFeedback(bool isOn)
    {
        PlayerPrefs.SetInt(PrefKey, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}
