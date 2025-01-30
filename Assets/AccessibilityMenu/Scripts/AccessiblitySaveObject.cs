using System.IO;
using UnityEngine;

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
        if (File.Exists(SavePath))
        {
            string json;
#if UNITY_WEBGL
            json = PlayerPrefs.GetString(AccessibilityClassUtility.AccessibilitySaveData);
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
