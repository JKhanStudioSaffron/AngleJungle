using UnityEngine;
using UnityEngine.Audio;


/// <summary>
/// Purpose: Handles the volume of Audio Mixers
/// Responsibilities: Set and Get values of Master, Music, Sfx volumes to slider
/// Usage: Add script on the GameObject make it a child of AudioManager and add its reference to
/// AudioManager.cs
/// </summary>

public class AudioMixerManager : MonoBehaviour
{
    public AudioMixer AudioMixer;

    public void Start()
    {
        SetMasterVolume(AudioSaveObject.Instance.SoundData.MasterVolume);
        SetMusicVolume(AudioSaveObject.Instance.SoundData.MusicVolume);
        SetSFXVolume(AudioSaveObject.Instance.SoundData.SfxVolume);
    }

    //set master volume
    public void SetMasterVolume(float volume)
    {
        AudioMixer.SetFloat(AudioClassUtility.MasterVolumeParam, volume);
    }

    //set music Volume
    public void SetMusicVolume(float volume)
    {
        AudioMixer.SetFloat(AudioClassUtility.MusicVolumeParam, volume);
    }

    //set sfx Volume
    public void SetSFXVolume(float volume)
    {
        AudioMixer.SetFloat(AudioClassUtility.SfxVolumeParam, volume);
    }

    public float GetMasterVolume()
    {
        AudioMixer.GetFloat(AudioClassUtility.MasterVolumeParam, out float value);
        return value;
    }

    public float GetMusicVolume()
    {
        AudioMixer.GetFloat(AudioClassUtility.MusicVolumeParam, out float value);
        return value;
    }

    public float GetSFXVolume()
    {
        AudioMixer.GetFloat(AudioClassUtility.SfxVolumeParam, out float value);
        return value;
    }
}
