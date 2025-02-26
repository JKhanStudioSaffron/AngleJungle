using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization.Settings;

/// <summary>
/// Purpose: Manages saving and loading game options to ensure persistence across sessions.
/// Responsibilities:
/// - Maintains a singleton instance for global access.
/// - Saves `OptionsData` to a JSON file or `PlayerPrefs` (for WebGL).
/// - Loads previously saved options data upon initialization.
/// Usage: Attach to a persistent GameObject in your scene. 
/// - Use `SaveOptionsState(OptionsData data)` to save updated options.
/// - Access `Instance.OptionsData` for the current game options.
/// </summary>


public class AccessibilitySaveObject : MonoBehaviour
{
    public static AccessibilitySaveObject Instance;

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
    public OptionsData OptionsData = new();

    //need it for webgl builds as localization asset was not preloading.
    //use this function where Locales are loaded
    public async Task InitializeLocalization(Action loadAfterInit = null)
    {
        if (!LocalizationSettings.InitializationOperation.IsDone)
            await LocalizationSettings.InitializationOperation.Task;

        loadAfterInit?.Invoke();
    }

    public void ReloadInstance()
    {
        OptionsData = new();
        Save();
        Instance = Load();
    }

    public void SaveOptionsState(OptionsData data)
    {
        OptionsData = data;
        Save();
    }

    public string SavePath => Path.Combine(Application.persistentDataPath, "AccessibilitySaveFile.json");

    void Save()
    {
        string json = JsonUtility.ToJson(Instance, true);
#if UNITY_WEBGL
        PlayerPrefs.SetString(AccessibilityClassUtility.AccessibilitySaveData, json);
#else
        File.WriteAllText(SavePath, json);
#endif
    }

    AccessibilitySaveObject Load()
    {
        string json = "";
#if UNITY_WEBGL
            json = PlayerPrefs.GetString(AccessibilityClassUtility.AccessibilitySaveData);
#else
        if (File.Exists(SavePath))
        {
            
            json = File.ReadAllText(SavePath);
#endif
        }
        else
        {
            Debug.LogWarning("Save file not found.");
        }
        JsonUtility.FromJsonOverwrite(json, Instance);
        return Instance;
    }
}
