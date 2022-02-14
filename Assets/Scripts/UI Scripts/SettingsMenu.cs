using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Settings{
    masterVol,
    musicVol,
    SFXVol,

    enumSize
}

public class SettingsMenu : MonoBehaviour
{
    private const float DEFAULT_VOL = 1;

    private float masterVol = DEFAULT_VOL;
    private float musicVol = DEFAULT_VOL;
    private float SFXVol = DEFAULT_VOL;

    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private Slider SFXVolSlider;

    void Start()
    {
        // If settings exist, load them
        if( PlayerPrefs.HasKey(Settings.masterVol.ToString()) ){
            masterVol = PlayerPrefs.GetFloat(Settings.masterVol.ToString());
            masterVolSlider.value = masterVol;

            musicVol = PlayerPrefs.GetFloat(Settings.musicVol.ToString());
            musicVolSlider.value = musicVol;

            SFXVol = PlayerPrefs.GetFloat(Settings.SFXVol.ToString());
            SFXVolSlider.value = SFXVol;
        }
        else{   // Setup
            PlayerPrefs.SetFloat(Settings.masterVol.ToString(), masterVol);
            PlayerPrefs.SetFloat(Settings.musicVol.ToString(), musicVol);
            PlayerPrefs.SetFloat(Settings.SFXVol.ToString(), SFXVol);
            PlayerPrefs.Save();
        }
    }

    public void AdjustMasterVolume()
    {
        masterVol = masterVolSlider.value;
        PlayerPrefs.SetFloat(Settings.masterVol.ToString(), masterVol);
        PlayerPrefs.Save();

        // Adjust actual value
        AudioManager.instance.AdjustMasterVolume(masterVol);
    }

    public void AdjustMusicVolume()
    {
        musicVol = musicVolSlider.value;
        PlayerPrefs.SetFloat(Settings.musicVol.ToString(), musicVol);
        PlayerPrefs.Save();

        // Adjust actual value
        AudioManager.instance.AdjustMusicVolume(musicVol);
    }

    public void AdjustSFXVolume()
    {
        SFXVol = SFXVolSlider.value;
        PlayerPrefs.SetFloat(Settings.SFXVol.ToString(), SFXVol);
        PlayerPrefs.Save();

        // Adjust actual value
        AudioManager.instance.AdjustSFXVolume(SFXVol);
    }
}
