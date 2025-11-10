using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider master, sfx;
    public Toggle fullscreen;

    void Start()
    {
        master.value = PlayerPrefs.GetFloat("MasterVol", 0.75f);
        sfx.value = PlayerPrefs.GetFloat("SFXVol", 0.75f);
        fullscreen.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }

    public void SetMaster(float v) { mixer.SetFloat("MasterVol", Mathf.Log10(v) * 20); PlayerPrefs.SetFloat("MasterVol", v); }
    public void SetSFX(float v) { mixer.SetFloat("SFXVol", Mathf.Log10(v) * 20); PlayerPrefs.SetFloat("SFXVol", v); }
    public void SetFullscreen(bool isOn) { Screen.fullScreen = isOn; PlayerPrefs.SetInt("Fullscreen", isOn ? 1 : 0); }
}