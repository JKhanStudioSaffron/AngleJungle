using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Purpose: Ensures every option in the Language Dropdown has its own font regardless of the language selected.
/// Responsibilities: Updates the font of the dropdown's selected text and dynamically applies the correct fonts to each dropdown option when expanded.
/// Usage: Attach this script to 'Item Label'. Assign fonts in the same order as  the dropdown options. The script automatically updates fonts when the dropdown is opened or a selection is made.
/// </summary>

public class CustomLanguageDropDown : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public List<TMP_FontAsset> fonts;

    void Start()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        StartCoroutine(WaitAndUpdateDropdownFonts());
    }

    void OnDropdownValueChanged(int index)
    {
        if (index >= 0 && index < fonts.Count)
        {
            dropdown.captionText.font = fonts[index]; 
        }

        StartCoroutine(WaitAndUpdateDropdownFonts()); 
    }

    IEnumerator WaitAndUpdateDropdownFonts()
    {
        yield return new WaitForEndOfFrame();

        Transform dropdownList = dropdown.transform.Find("Dropdown List");

        if (dropdownList == null)
        { 
            yield break;
        } 

        TMP_Text[] items = dropdownList.GetComponentsInChildren<TMP_Text>();

        for (int i = 0; i < items.Length; i++)
        {
            if (i < fonts.Count)
            {
                items[i].font = fonts[i]; 
            }
        }
    }
}
