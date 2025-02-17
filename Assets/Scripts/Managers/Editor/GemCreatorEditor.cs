using UnityEditor;
using UnityEngine;

/// <summary>
/// Purpose: Custom editor for `GemCreator`, adding an inspector button to generate gems.
/// Responsibilities:
/// - Extends Unity's Editor class to modify the `GemCreator` inspector.
/// - Draws default inspector fields.
/// - Provides a "Create Gems" button to invoke `CreateGems()` from the inspector.
/// Usage:
/// - Attach to a script inside an "Editor" folder.
/// - Automatically enhances the `GemCreator` component's inspector.
/// </summary>

[CustomEditor(typeof(GemCreator))]
public class GemCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GemCreator manager = (GemCreator)target;

        if (GUILayout.Button("Create Gems"))
        {
            manager.CreateGems(); 
        }
    }
}
