using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Purpose: Allows players to adjust the text size in the game through a dropdown menu.
/// Responsibilities: Handles the text size selection, applies the selected font multiplier, and updates the saved option for text size.
/// Usage: Attach this script to a GameObject with a `TMP_Dropdown` to control text size selection. The dropdown should be populated with the available size options.
/// </summary>


public class CallResizeText : MonoBehaviour
{
    TMP_Dropdown textSizeDropDown;

    void Start()
    {
        textSizeDropDown = GetComponent<TMP_Dropdown>();
        textSizeDropDown.onValueChanged.AddListener((val) =>
        {
            ResizeTextOnValueChange(val);
        });
        textSizeDropDown.value = AccessibilitySaveObject.Instance.OptionsData.FontMultiplier;
    }

    public void ResizeTextOnValueChange(int val)
    {
        OptionsManager.OptionValues.FontMultiplier = val;
        TextResize.ResizeText?.Invoke(val);
    }
}
