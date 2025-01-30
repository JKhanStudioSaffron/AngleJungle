using UnityEditor;
using UnityEngine;
using Wilberforce;

/// <summary>
/// Purpose: Automatically adds the `Colorblind` and `ChangeColorBlindMode` scripts to all existing and newly added Camera components in the scene.
/// Responsibilities: Scans for existing `Camera` components and adds the necessary scripts if they are not already attached. Also ensures that scripts are added when new Camera components are created.
/// Usage: This script runs automatically in the editor. It is initialized when the editor starts and adds the required scripts to `Camera` components.
/// </summary>


[InitializeOnLoad]
public class ColorBlindScriptAdd
{
    [MenuItem("Tools/Add Script to Existing Camera")]
    static void AutoAddScriptToCamera()
    {
        // Check and add script when the editor is initialized
        AddScriptToAllExistingCamera();
    }

    private static void AddScriptToAllExistingCamera()
    {
        // Find all existing TMP_Text components in the scene
        Camera[] tmpTexts = UnityEngine.Object.FindObjectsOfType<Camera>();

        foreach (var tmpText in tmpTexts)
        {
            // Add your custom script if not already attached
            if (!tmpText.gameObject.GetComponent<Colorblind>())
            {
                tmpText.gameObject.AddComponent<Colorblind>();
            }
            if (!tmpText.gameObject.GetComponent<ChangeColorBlindMode>())
            {
                tmpText.gameObject.AddComponent<ChangeColorBlindMode>();
            }
        }
    }

    static ColorBlindScriptAdd()
    {
        // Subscribe to the component addition event

        UnityEditor.ObjectFactory.componentWasAdded += OnComponentAdded;
        //uncomment below if you are looking to apply colorblind mode on every camera automatically
        //EditorApplication.hierarchyChanged += AutoAddScriptToCamera;
    }

    private static void OnComponentAdded(Component component)
    {
        if (component is Camera) // Check if the added component is TextMeshPro
        {
            // Check if the GameObject already has the desired script
            if (!component.gameObject.GetComponent<Colorblind>())
            {
                component.gameObject.AddComponent<Colorblind>();
            }

            if (!component.gameObject.GetComponent<ChangeColorBlindMode>())
            {
                component.gameObject.AddComponent<ChangeColorBlindMode>();
            }
        }
    }
}
