using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Purpose: Allows players to select a colorblind mode from a dropdown menu to customize visual settings for accessibility.
/// Responsibilities: Sets the selected colorblind mode and applies the changes to the game’s visuals. The chosen mode is saved in the options data.
/// Usage: Attach this script to a GameObject with a `TMP_Dropdown` to control the colorblind mode selection.
/// </summary>


public class SetColorBlindMode : MonoBehaviour
{
    TMP_Dropdown dropDown;

    void Start()
    {
        dropDown = GetComponent<TMP_Dropdown>();
        dropDown.onValueChanged.AddListener(OnValueSelected);
        dropDown.value = AccessibilitySaveObject.Instance.OptionsData.ColorBlindMode;
    }

    void OnValueSelected(int mode)
    {
        OptionsManager.OptionValues.ColorBlindMode = mode;
        ChangeColorBlindMode.SetColorBlind?.Invoke(mode);
    }
}
