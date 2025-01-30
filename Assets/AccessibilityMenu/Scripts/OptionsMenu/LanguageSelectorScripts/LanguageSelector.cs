
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

/// <summary>
/// Purpose: Allows players to change the language of the game through a dropdown menu.
/// Responsibilities: Handles the language selection, switches the locale, and refreshes the UI text to reflect the language change. Also manages language reset to the saved option.
/// Usage: Attach this script to a GameObject with a `TMP_Dropdown` to control language selection. The dropdown should be populated with the available language options.
/// </summary>

public class LanguageSelector : MonoBehaviour
{
    public TMP_Dropdown languageDropdown; // Reference to the dropdown UI

    IEnumerator Start()
    {
        // Set the dropdown to the current locale
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        yield return new WaitForSeconds(0.2f);
        languageDropdown.value = AccessibilitySaveObject.Instance.OptionsData.Language;
    }

    public void OnLanguageChanged(int index)
    {
        // Switch to the selected locale
        var selectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        LocalizationSettings.SelectedLocale = selectedLocale;
        OptionsManager.OptionValues.Language = index;
        Invoke(nameof(RefreshText), 0.5f);
    }

    public static void ValueReset()
    {
        var selectedLocale = LocalizationSettings.AvailableLocales.Locales[AccessibilitySaveObject.Instance.OptionsData.Language];
        LocalizationSettings.SelectedLocale = selectedLocale;
    }

    void RefreshText()
    {
        RTLTextFixer.RefreshText?.Invoke();
        TextResize.ResizeText?.Invoke(AccessibilitySaveObject.Instance.OptionsData.FontMultiplier);

    }
}
