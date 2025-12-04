using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer mixer;

    public Slider master;
    public Slider music;
    public Slider sfx;

    [Header("Video")]
    public Toggle fullscreen;

    void Start()
    {
        // читаем сохранённые значения (0..1), по умолчанию 0.75
        float masterVal = PlayerPrefs.GetFloat("MasterVol", 0.75f);
        float musicVal = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        float sfxVal = PlayerPrefs.GetFloat("SFXVol", 0.75f);

        // ставим в слайдеры
        if (master != null) master.value = masterVal;
        if (music != null) music.value = musicVal;
        if (sfx != null) sfx.value = sfxVal;

        // сразу применяем к микшеру
        ApplyVolume("MasterVol", masterVal);
        ApplyVolume("MusicVol", musicVal);
        ApplyVolume("SFXVol", sfxVal);

        fullscreen.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        Screen.fullScreen = fullscreen.isOn;
    }

    public void SetMaster(float v)
    {
        ApplyVolume("MasterVol", v);
        PlayerPrefs.SetFloat("MasterVol", v);
    }

    public void SetMusic(float v)
    {
        ApplyVolume("MusicVol", v);
        PlayerPrefs.SetFloat("MusicVol", v);
    }

    public void SetSFX(float v)
    {
        ApplyVolume("SFXVol", v);
        PlayerPrefs.SetFloat("SFXVol", v);
    }

    public void SetFullscreen(bool isOn)
    {
        Screen.fullScreen = isOn;
        PlayerPrefs.SetInt("Fullscreen", isOn ? 1 : 0);
    }

    void ApplyVolume(string paramName, float sliderValue)
    {
        // 0 = полная тишина (очень большой минус в dB)
        if (sliderValue <= 0.0001f)
        {
            mixer.SetFloat(paramName, -80f);
        }
        else
        {
            float dB = Mathf.Log10(sliderValue) * 20f;
            mixer.SetFloat(paramName, dB);
        }
    }
}