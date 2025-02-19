using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Purpose: Manages the application of user options (colorblind mode, font size, and language).
/// Responsibilities: Initializes option values from saved data, applies settings, and updates saved preferences.
/// Usage: Attach to a GameObject responsible for managing options. Use ApplySettings to persist changes.
/// </summary>


public class OptionsManager : MonoBehaviour
{
    public static OptionsData OptionValues;
    public GameObject OptionsPanel;

    private IEnumerator Start()
    {
        OptionsPanel.SetActive(false);
        yield return new WaitUntil(()=> AccessibilitySaveObject.Instance != null);
        OptionValues = new(AccessibilitySaveObject.Instance.OptionsData.ColorBlindMode,
                             AccessibilitySaveObject.Instance.OptionsData.FontMultiplier,
                             AccessibilitySaveObject.Instance.OptionsData.Language);

        OptionsPanel.SetActive(true);
    }

    public void ApplySettings()
    {
        MusicManager.Instance?.PlayClickButton();

        AccessibilitySaveObject.Instance.SaveOptionsState(OptionValues);

        OptionValues = new(AccessibilitySaveObject.Instance.OptionsData.ColorBlindMode,
                             AccessibilitySaveObject.Instance.OptionsData.FontMultiplier,
                             AccessibilitySaveObject.Instance.OptionsData.Language);
    }

    public void GoToMM()
    {
        MusicManager.Instance?.PlayClickButton();
        SceneManager.LoadScene(Global.SCENE_START);
    }
}
