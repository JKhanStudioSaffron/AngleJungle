using TMPro;
using UnityEngine;

/// <summary>
/// Purpose: Initializes gem properties, setting angle values and updating text.
/// Responsibilities:
/// - Assigns `AngleVal` to the `GemScript`'s `gemAngle`.
/// - Updates the displayed angle text in `AngleText`.
/// Usage:
/// - Attach this script to a GameObject with a `TMP_Text` and `Gem` component.
/// - Set `AngleVal` in the inspector to reflect the desired gem angle.
/// </summary>

public class GemProperty : MonoBehaviour
{
    public int AngleVal;
    public TMP_Text AngleText;
    public Gem GemScript;

    public void Start()
    {
        GemScript.gemAngle = AngleVal;
        AngleText.text = $"{AngleVal}°";
    }
}
