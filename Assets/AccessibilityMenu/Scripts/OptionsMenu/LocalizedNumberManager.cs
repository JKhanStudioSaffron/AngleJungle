using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

/// <summary>
/// Purpose: Manages localized number strings some strings for use in the game.
/// Responsibilities: Loads and stores localized string representations of digits (0-9) and the label for Level from a specified `LocalizedStringTable`.
/// Usage: Attach to a GameObject responsible for handling localized number and time unit strings. Set `LocalizedTable` in the inspector to the appropriate localization table.
/// </summary>


public class LocalizedNumberManager : MonoBehaviour
{
    public static List<string> Numbers = new();
    public LocalizedStringTable LocalizedTable;
    public static string Level;

    private void Awake()
    {
        Numbers.Clear();

        for (int i = 0; i < 10; i++)
        {
            Numbers.Add(LocalizedTable.GetTable().GetEntry(i.ToString()).Value);
        }

        Level = LocalizedTable.GetTable().GetEntry("Level").Value;
    }

    public static string GetLocalizedNumber(int number)
    {
        int[] digits = number.ToString().Select(digit => digit - '0').ToArray();

        CultureInfo culture = new CultureInfo(LocalizationSettings.SelectedLocale.Identifier.Code);
        if (culture.TextInfo.IsRightToLeft)
        {
            digits = digits.Reverse().ToArray();
        }

        string localizedString = "";
        for(int i = 0; i < digits.Length; i++)
        {
            localizedString += Numbers[digits[i]];
        }

        return localizedString;
    }
}
