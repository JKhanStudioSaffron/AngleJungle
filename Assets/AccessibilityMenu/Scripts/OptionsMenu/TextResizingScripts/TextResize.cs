using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Purpose: Handles resizing of text based on a font multiplier for dynamic UI text scaling.
/// Responsibilities: Resizes the text component's font size according to a given multiplier. Listens for changes in the multiplier and applies them to the text component.
/// Usage: Attach this script to any UI element with a `TMP_Text` component that should be affected by text resizing.
/// </summary>


public class TextResize : MonoBehaviour
{
    public TextType TypeOfText;
    TMP_Text text;
    public static Action<int> ResizeText;
    bool applied;

    static Dictionary<TextType, float[]> fontMultipliers = new Dictionary<TextType, float[]>();
    

    [ContextMenu("CalculateValues")]
    void CalculateMultipliers()
    {
        int enumCount = Enum.GetValues(typeof(TextType)).Length;

        if (fontMultipliers.Count >= enumCount)
            return;

        int n = 4; //number of font sizes

        for (int j = 0; j < enumCount; j++)
        {
            float[] multipliers = new float[4];
            for (int i = 0; i < n; i++)
            {
                float val = AccessibilityClassUtility.fontSizeMin[j] + (i * ((AccessibilityClassUtility.fontSizeMax[j] - AccessibilityClassUtility.fontSizeMin[j]) / (n - 1)));
                multipliers[i] = (float)Math.Round(val/ AccessibilityClassUtility.fontSizeMin[j],2);
            }
            fontMultipliers.Add((TextType)j, multipliers);
        }
    }

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        text.enableAutoSizing = true;

        if (!applied)
        {
            ResizeTextWithMultiplier(AccessibilitySaveObject.Instance.OptionsData.FontMultiplier);
        }
    }

    private void OnEnable()
    {
        ResizeText += ResizeTextWithMultiplier;
        CalculateMultipliers();
    }

    private void OnDisable()
    {
        ResizeText -= ResizeTextWithMultiplier;
    }
    void ResizeTextWithMultiplier(int fontSize)
    {
        if (text != null)
        {
            text.fontSizeMin = AccessibilityClassUtility.fontSizeMin[(int) TypeOfText];
            text.fontSizeMax = fontMultipliers[TypeOfText][fontSize] * AccessibilityClassUtility.fontSizeMin[(int)TypeOfText];
            applied = true;
        }
    }


}
