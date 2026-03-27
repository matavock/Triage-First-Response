using UnityEngine;
using UnityEngine.UI;
public class DeprProgBar : MonoBehaviour
{
    public Slider progressSlider;

    void Start()
    {
        progressSlider.value = PlayerPrefs.GetInt("Happiness");
    }
    void Update()
    {
        progressSlider.value = DayStats.depression;
    }
}
