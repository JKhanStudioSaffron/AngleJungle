using System.IO;
using UnityEngine;

/// <summary>
/// Purpose: Manages saving and loading audio settings persistently across game sessions.
/// Responsibilities:
/// - Stores audio-related data in `SoundData`.
/// - Saves audio settings to a file or `PlayerPrefs` for WebGL builds.
/// - Loads saved audio settings on initialization.
/// Usage: Attach to a GameObject in the scene. 
/// - Call `Save()` to persist the current audio settings.
/// - Automatically loads saved settings during initialization.
/// </summary>

public class AudioSaveObject : MonoBehaviour
{
    public static AudioSaveObject Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Instance = Load();
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);
    }

    [HideInInspector]
    public SoundSettings SoundData;

    public string SavePath => Path.Combine(Application.persistentDataPath, "AudioSaveFile.json");

    public void Save()
    {
        string json = JsonUtility.ToJson(Instance, true);
#if UNITY_WEBGL
        PlayerPrefs.SetString(AudioClassUtility.SoundSaveData, json);
#else
        File.WriteAllText(SavePath, json);
#endif
    }

    AudioSaveObject Load()
    {
        if (File.Exists(SavePath))
        {
            string json;
#if UNITY_WEBGL
            json = PlayerPrefs.GetString(AudioClassUtility.SoundSaveData);
#else
            json = File.ReadAllText(SavePath);
#endif
            JsonUtility.FromJsonOverwrite(json, Instance);
        }
        else
        {
            Debug.LogWarning("Save file not found.");
        }
        return Instance;
    }
}
