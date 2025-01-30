using EasyAlphabetArabic;
using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Purpose: Fixes and applies correct Arabic and Urdu text formatting for TMP_Text and TMP_Dropdown components.
/// Responsibilities: Corrects Arabic and Urdu text rendering issues for TextMeshPro text and dropdown options, ensuring proper right-to-left display and character joining.
/// Usage: Attach to a GameObject with TMP_Text or TMP_Dropdown components. Use `SetUpReferences` to initialize references, and `Fix` to apply text corrections.
/// </summary>


public class RTLTextFixer : MonoBehaviour
{
    public static Action RefreshText;

    public TMP_Text Text;
    public TMP_Dropdown DropDown;
    bool fixedText;

    [ContextMenu("SetUpReferences")]
    public void SetUpReferences()
    {
        Text = GetComponent<TMP_Text>();
        DropDown = GetComponent<TMP_Dropdown>();
    }

    private void OnEnable()
    {
        RefreshText += Fix;
        if (!fixedText)
        {
            fixedText = true;
            Invoke(nameof(Fix), 1f);
        }
    }

    [ContextMenu("Fix")]
    // Use this for initialization
    void Fix()
    {
        if (Text != null)
        {
            string fixedText = EasyArabicCore.CorrectTextMeshPro(Text.text);
            Text.text = fixedText;
        }

        if (DropDown != null)
        {
            foreach (var option in DropDown.options)
            {
                string fixedText = EasyArabicCore.CorrectTextMeshPro(option.text);

                option.text = fixedText;
            }
        }
    }
}
