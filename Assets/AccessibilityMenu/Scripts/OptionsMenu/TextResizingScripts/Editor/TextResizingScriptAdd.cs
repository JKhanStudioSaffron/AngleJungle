using TMPro;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Purpose: Automatically adds the `TextResize` and `Fixer` scripts to all existing and newly added TMP_Text components in the scene.
/// Responsibilities: Scans for existing `TMP_Text` components and adds the necessary scripts if they are not already attached. Also ensures that scripts are added when new TMP_Text components are created.
/// Usage: This script runs automatically in the editor. It is initialized when the editor starts and adds the required scripts to `TMP_Text` components.
/// </summary>


[InitializeOnLoad]
public class TextResizingScriptAdd
{
    [MenuItem("Tools/Add Script to Existing TMP")]
    static void AutoAddScriptToTMP()
    {
        // Check and add script when the editor is initialized
        AddScriptToAllExistingTMP();
    }

    private static void AddScriptToAllExistingTMP()
    {
        // Find all existing TMP_Text components in the scene
        TMP_Text[] tmpTexts = UnityEngine.Object.FindObjectsOfType<TMP_Text>();

        foreach (var tmpText in tmpTexts)
        {
            // Add your custom script if not already attached
            if (!tmpText.gameObject.GetComponent<TextResize>())
            {
                tmpText.gameObject.AddComponent<TextResize>();
            }

            if (!tmpText.gameObject.GetComponent<RTLTextFixer>())
            {
                tmpText.gameObject.AddComponent<RTLTextFixer>();
            }
        }
    }

    static TextResizingScriptAdd()
    {
        // Subscribe to the component addition event

        UnityEditor.ObjectFactory.componentWasAdded += OnComponentAdded;
        EditorApplication.hierarchyChanged += AutoAddScriptToTMP;
    }

    private static void OnComponentAdded(Component component)
    {
        if (component is TMP_Text) // Check if the added component is TextMeshPro
        {
            // Check if the GameObject already has the desired script
            if (!component.gameObject.GetComponent<TextResize>())
            {
                // Add the script to the GameObject
                component.gameObject.AddComponent<TextResize>();
            }

            if (!component.gameObject.GetComponent<RTLTextFixer>())
            {
                component.gameObject.AddComponent<RTLTextFixer>();
            }
        }
    }
}
