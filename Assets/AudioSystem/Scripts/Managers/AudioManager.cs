using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Purpose: Manages audio and music playback through a pooling system to optimize performance and memory usage.
/// Responsibilities: Handles initialization, pooling, playback, pausing, resuming, and stopping of audio and music clips. Ensures efficient reuse of audio sources and manages their state.
/// Usage: Add audio and music clip holders to their respective lists, define pooling counts, and use methods like `PlayAudio()`, `PlayMusic()`, `PauseMusic()`, `ResumeMusic()`, and `StopAudio()` to manage audio in the game.
/// </summary>


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            StartPooling();
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public AudioMixerManager AudioMixer;

    public List<AudioClipHolder> AudiosHolder, MusicHolder;

    public int PooledObjectsAudio, PooledObjectsMusic;

    List<AudioSource> audioSources = new();
    List<AudioSource> musicSources = new();

    void StartPooling()
    {
        for (int i = 0; i < PooledObjectsAudio; i++)
        {
            GameObject audio = new GameObject("Audio");
            audioSources.Add(audio.AddComponent<AudioSource>());
            audio.transform.SetParent(transform);
            audio.SetActive(false);
        }

        for (int i = 0; i < PooledObjectsMusic; i++)
        {
            GameObject music = new GameObject("Music");
            musicSources.Add(music.AddComponent<AudioSource>());
            music.transform.SetParent(transform);
            music.SetActive(false);
        }
    }

    void PoolMore(int count, bool musicPool)
    {
        if (!musicPool)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject audio = new GameObject("Audio");
                audioSources.Add(audio.AddComponent<AudioSource>());
                audio.transform.SetParent(transform);
                audio.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                GameObject music = new GameObject("Music");
                musicSources.Add(music.AddComponent<AudioSource>());
                music.transform.SetParent(transform);
                music.SetActive(false);
            }
        }
    }


    AudioSource GetAudioPooledObject()
    {
        AudioSource audio = audioSources.Find(source => !source.gameObject.activeInHierarchy);
        if (audio != null)
            audio.gameObject.SetActive(true);
        else
        {
            PoolMore(3, false);
            audio = audioSources.Find(source => !source.gameObject.activeInHierarchy);
        }
        return audio;
    }

    AudioSource GetMusicPooledObject()
    {
        AudioSource music = musicSources.Find(source => !source.gameObject.activeInHierarchy);
        if (music != null)
            music.gameObject.SetActive(true);
        else
        {
            PoolMore(3, false);
            music = musicSources.Find(source => !source.gameObject.activeInHierarchy);
        }
        return music;
    }

    void DisableAllPlayedClips()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
                source.gameObject.SetActive(false);
        }
    }

    IEnumerator DisableThisClip(AudioSource clip)
    {
        yield return new WaitUntil(() => !clip.isPlaying);
        clip.gameObject.SetActive(false);
    }

    AudioSource Play(Audios audioCalled, bool isMusic)
    {
        if (audioCalled != null)
        {
            AudioSource source;

            source = isMusic ? GetMusicPooledObject() : GetAudioPooledObject();
            source.gameObject.SetActive(true);
            source.clip = audioCalled.Clip;
            source.volume = isMusic ? 0 : audioCalled.Volume;
            source.loop = audioCalled.Loop;
            source.playOnAwake = audioCalled.PlayOnAwake;
            source.priority = audioCalled.Priority;
            source.pitch = audioCalled.Pitch;
            source.outputAudioMixerGroup = audioCalled.Mixer;
            source.Play();

            source.DOFade(audioCalled.Volume, 0.3f).OnComplete(() =>
            {
                if (!source.loop)
                    StartCoroutine(DisableThisClip(source));
            });

            return source;
        }
        return null;
    }

    public void ReplayAudio(AudioSource source)
    {
        source.gameObject.SetActive(true);
        source.Play();
        StartCoroutine(DisableThisClip(source));
    }

    public AudioSource PlayAudio(Clips soundName)
    {
        Audios audioCalled = null;
        foreach (var audio in AudiosHolder)
        {
            audioCalled = audio.GetAudio(soundName);
            if (audioCalled != null)
                break;
        }
        if (audioCalled != null)
            return Play(audioCalled, false);

        return null;
    }

    public AudioSource PlayMusic(Clips musicName)
    {
        Audios audioCalled = null;

        foreach (var audio in MusicHolder)
        {
            audioCalled = audio.GetAudio(musicName);
            if (audioCalled != null)
                break;
        }
        if (audioCalled != null)
            return Play(audioCalled, true);

        return null;
    }

    public void PauseMusic(Clips musicName, AudioSource audioToPause)
    {

        if (audioToPause == null)
            return;

        audioToPause.DOFade(0, 1f).OnComplete(() =>
        {
            audioToPause.Pause();
        });
    }

    public void ResumeMusic(Clips musicName, AudioSource audioData)
    {
        if (audioData == null)
            return;

        audioData.UnPause();
        audioData.DOFade(1, 0.5f);

    }

    public void StopAudio(Clips audioName, AudioSource audioToStop)
    {

        if (audioToStop == null)
            return;

        audioToStop.DOFade(0, 1f).OnComplete(() =>
        {
            audioToStop.gameObject.SetActive(false);
        });

    }
}