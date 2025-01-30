using UnityEngine.Audio;
using UnityEngine;

public class AudioClassUtility 
{
    [Header("Exposed Parameters")]
    public const string MasterVolumeParam = "MasterVolume";
    public const string MusicVolumeParam = "MusicVolume";
    public const string SfxVolumeParam = "SFXVolume";
    public const string SoundSaveData = "SoundSaveDataFile";

}

/// <summary>
/// Tracks Sound Settings in Options Screen
/// </summary>
[System.Serializable]
public class SoundSettings
{
    public float SfxVolume;
    public float MusicVolume;
    public float MasterVolume;

    public SoundSettings(float sfxVolume, float musicVolume, float masterVolume)
    {
        SfxVolume = sfxVolume;
        MusicVolume = musicVolume;
        MasterVolume = masterVolume;
    }
}

// <summary>
/// Defines audio properties, including volume, pitch, and playback options.
/// </summary>
[System.Serializable]
public class Audios
{
    public Clips Audio;
    public AudioClip Clip;
    [Range(0f, 1f)]
    public float Volume;

    [Range(-3f, 3f)]
    public float Pitch;

    [Range(0, 256)]
    public int Priority;

    public bool Loop = false;
    public bool PlayOnAwake = false;
    public AudioMixerGroup Mixer;
}


