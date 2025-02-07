using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Purpose: Assigns a specific camera to a Canvas based on a defined LayerMask.
/// Responsibilities:
/// - Determines the correct camera layer from the provided LayerMask.
/// - Sets the Canvas render mode to `ScreenSpaceCamera`.
/// - Finds and assigns the first camera that matches the specified layer.
/// Usage:
/// - Attach this script to a GameObject with a `Canvas` component.
/// - Assign `CameraLayerToUse` in the inspector to specify which camera should be used.
/// - Ensures the Canvas is rendered by the appropriate camera.
/// </summary>

public class SetCameraToCanvas : MonoBehaviour
{
    public LayerMask CameraLayerToUse;

    private void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = GetCamera();
    }

    Camera GetCamera()
    {
        List<Camera> allCams = new();
        allCams.AddRange(FindObjectsByType<Camera>(FindObjectsSortMode.None));
        return allCams.Find(cam => (cam.cullingMask & CameraLayerToUse.value) == CameraLayerToUse.value);
    }
}
