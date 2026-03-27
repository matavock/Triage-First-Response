using UnityEngine;
using UnityEngine.UI;
public class WealthProgBar : MonoBehaviour
{
    public Slider progressSlider;

    void Start()
    {
        progressSlider.value = PlayerPrefs.GetInt("Wealth");
    }
    void Update()
    {
        progressSlider.value = DayStats.wealth;
    }
}
