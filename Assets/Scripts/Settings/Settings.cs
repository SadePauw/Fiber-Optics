using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.Rendering;

public class Settings : MonoBehaviour
{
    public AudioMixer masterMixer;
    public TextMeshProUGUI masterMixerText;
    public AudioMixerGroup musicMixer;
    public TextMeshProUGUI musicMixerText;
    public AudioMixerGroup sfxMixer;
    public TextMeshProUGUI sfxMixerText;

    public void SetVolumeMaster(float volume)
    {
        masterMixerText.text = Mathf.RoundToInt(Mathf.InverseLerp(-80, 0, volume) * 100).ToString(); ;
        masterMixer.SetFloat("masterVolume", volume);
    }

    public void SetVolumeMusic(float volume)
    {
        musicMixerText.text = Mathf.RoundToInt(Mathf.InverseLerp(-80, 0, volume) * 100).ToString(); ;
        masterMixer.SetFloat("musicVolume", volume);
    }

    public void SetVolumeSFX(float volume)
    {
        sfxMixerText.text = Mathf.RoundToInt(Mathf.InverseLerp(-80, 0, volume) * 100).ToString(); ;
        masterMixer.SetFloat("sfxVolume", volume);
    }
}
