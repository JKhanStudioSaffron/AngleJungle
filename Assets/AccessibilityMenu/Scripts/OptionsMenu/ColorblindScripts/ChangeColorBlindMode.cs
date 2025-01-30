using System;
using UnityEngine;
using Wilberforce;

/// <summary>
/// Purpose: Manages the application of the selected color blind mode.
/// Responsibilities: Sets the appropriate color blind mode using the `Colorblind` script based on the saved mode or triggered changes.
/// Usage: Attach to a GameObject with the `Colorblind` script. Use the `SetColorBlind` action to change the color blind mode at runtime.
/// </summary>

public class ChangeColorBlindMode : MonoBehaviour
{
    Colorblind colorBlindScript;
    public static Action<int> SetColorBlind;
    bool isApplied;

    private void Start()
    {
        colorBlindScript = GetComponent<Colorblind>();

        if (!isApplied)
        {
            isApplied = true;
            SetMode(AccessibilitySaveObject.Instance.OptionsData.ColorBlindMode);
        }
    }

    private void OnEnable()
    {
        SetColorBlind += SetMode;
    }

    private void OnDisable()
    {
        SetColorBlind -= SetMode;
    }
    void SetMode(int mode)
    {
        colorBlindScript.Type = mode;
    }

}
