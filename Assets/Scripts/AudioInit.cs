using UnityEngine;
using UnityEngine.Audio;

public class AudioInit : MonoBehaviour
{
    public AudioMixer mixer;

    void Start()
    {
        Apply("MasterVol");
        Apply("MusicVol");
        Apply("SFXVol");
    }

    void Apply(string paramName)
    {
        float v = PlayerPrefs.GetFloat(paramName, 0.75f);

        if (v <= 0.0001f)
        {
            mixer.SetFloat(paramName, -80f);
        }
        else
        {
            mixer.SetFloat(paramName, Mathf.Log10(v) * 20f);
        }
    }
}