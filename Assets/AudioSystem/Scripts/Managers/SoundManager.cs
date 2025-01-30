using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Purpose: Handles the UI sliders and toggles for volumes and Mute state of each sound
/// Responsibilities: Change Mixer volumes with respect to slider and toggle values.
/// Usage: Add it in the scene where the UI exists then add references to it
/// </summary>

public class SoundManager : MonoBehaviour
{
    public Slider SfxSlider, MusicSlider, MasterSlider;
    public Toggle SfxToggle, MusicToggle, MasterToggle;

    private float sfxPref, musicPref, masterPref;
    GameObject musicManager;

    private void OnDisable()
    {
        InitializePrefs();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => AudioSaveObject.Instance != null);

        //add events to sliders and toggle to listen to value changes and apply functionality
        SfxSlider.onValueChanged.AddListener(delegate { SetSfxVolume(); });
        MusicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        MasterSlider.onValueChanged.AddListener(delegate { SetMasterVolume(); });

        SfxToggle.onValueChanged.AddListener(delegate { SetSfxMuteState(); });
        MusicToggle.onValueChanged.AddListener(delegate { SetMusicMuteState(); });
        MasterToggle.onValueChanged.AddListener(delegate { SetMasterMuteState(); });

        InitializePrefs();
    }

    void InitializePrefs()
    {
        sfxPref = AudioSaveObject.Instance.SoundData.SfxVolume;
        musicPref = AudioSaveObject.Instance.SoundData.MusicVolume;
        masterPref = AudioSaveObject.Instance.SoundData.MasterVolume;

        //initialize sliders with last saved values
        SfxSlider.value = sfxPref;
        MusicSlider.value = musicPref;
        MasterSlider.value = masterPref;

        //initialize toggle with respect to slider values
        SfxToggle.isOn = SfxSlider.value <= -80f;
        MusicToggle.isOn = MusicSlider.value <= -80f;
        MasterToggle.isOn = MasterSlider.value <= -80f;
    }

    //set sfx slider value
    public void SetSfxVolume()
    {
        float decibelValue = SfxSlider.value;
        AudioManager.Instance.AudioMixer.SetSFXVolume(decibelValue);
        if (SfxSlider.value > -80f)
            SfxToggle.isOn = false;
        else
            SfxToggle.isOn = true;

    }

    //set music slider value
    public void SetMusicVolume()
    {
        float decibelValue = MusicSlider.value;
        AudioManager.Instance.AudioMixer.SetMusicVolume(decibelValue);
        if (MusicSlider.value > -80f)
            MusicToggle.isOn = false;
        else
            MusicToggle.isOn = true;

    }

    //set master slider value
    public void SetMasterVolume()
    {
        float decibelValue = MasterSlider.value;
        AudioManager.Instance.AudioMixer.SetMasterVolume(decibelValue);
        if (MasterSlider.value > -80f)
            MasterToggle.isOn = false;
        else
            MasterToggle.isOn = true;

    }

    //set sfx toggle
    public void SetSfxMuteState()
    {
        bool isMute = SfxToggle.isOn;
        float decibelValue = isMute ? -80f : AudioManager.Instance.AudioMixer.GetSFXVolume(); 
        SfxSlider.value = decibelValue;
        AudioManager.Instance.AudioMixer.SetSFXVolume(decibelValue);
    }

    //set music toggle
    public void SetMusicMuteState()
    {
        bool isMute = MusicToggle.isOn;
        float decibelValue = isMute ? -80f : AudioManager.Instance.AudioMixer.GetMusicVolume(); 
        MusicSlider.value = decibelValue;
        AudioManager.Instance.AudioMixer.SetMusicVolume(decibelValue);

    }

    //set master toggle
    public void SetMasterMuteState()
    {
        bool isMute = MasterToggle.isOn;
        float decibelValue = isMute ? -80f : AudioManager.Instance.AudioMixer.GetMasterVolume(); 
        MasterSlider.value = decibelValue;
        AudioManager.Instance.AudioMixer.SetMasterVolume(decibelValue);
    }

    public void Apply()
    {
        AudioSaveObject.Instance.SoundData.SfxVolume = AudioManager.Instance.AudioMixer.GetSFXVolume();
        AudioSaveObject.Instance.SoundData.MusicVolume = AudioManager.Instance.AudioMixer.GetMusicVolume();
        AudioSaveObject.Instance.SoundData.MasterVolume = AudioManager.Instance.AudioMixer.GetMasterVolume();
        AudioSaveObject.Instance.Save();
    }
}
