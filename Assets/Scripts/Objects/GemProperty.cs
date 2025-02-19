using TMPro;
using UnityEngine;

/// <summary>
/// Purpose: Initializes gem properties, setting angle values and updating text.
/// Responsibilities:
/// - Assigns Sign to the `GemScript`'s `gemAngle` based on AngleType
/// - Assigns `AngleVal` to the `GemScript`'s `gemAngle`.
/// - Updates the displayed angle text in `AngleText`.
/// Usage:
/// - Attach this script to a GameObject with a `TMP_Text` and `Gem` component.
/// - Set `AngleVal` in the inspector to reflect the desired gem angle.
/// - Set `AngleType` in the inspector to reflect the desired gem angle's Sign.
/// </summary>

public class GemProperty : MonoBehaviour
{
    public int AngleVal;
    public AngleType AngleType;
    public TMP_Text AngleText;
    public Gem GemScript;

    public void Start()
    {
        if (AngleType == AngleType.AntiClockWise && AngleVal > 0)
            AngleVal *= -1;

        GemScript.gemAngle = AngleVal;
        AngleText.text = $"{AngleVal}°";
    }
}
