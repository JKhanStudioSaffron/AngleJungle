using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Purpose: Manages custom cursor behavior and animations for different input events.
/// Responsibilities:
/// - Changes cursor sprite based on input events (idle, tap, and held).
/// - Positions the custom cursor sprite in sync with the touch/mouse position.
/// - Hides the system cursor for a custom in-game cursor experience.
/// - Assigns the cursor to a specific layer based on the provided LayerMask.
/// - Enables or disables itself based on the platform using BuildTargetFlags.
/// Usage:
/// - Attach this script to a GameObject with a SpriteRenderer component to act as a custom cursor.
/// - Link `Idle`, `Tap`, and `Held` sprites in the inspector for animation based on user interactions.
/// - Set `CameraLayerToUse` to define which camera will render the cursor.
/// - The cursor is only enabled if the current platform matches the specified `BuildTargetFlags`.
/// </summary>

public class CursorManager : MonoBehaviour
{
    static CursorManager Instance;
    public Sprite Idle, Tap, Held;
    public SpriteRenderer CursorImg;
    bool animationPlaying;
    Camera cam;
    public LayerMask CameraLayerToUse; // sets which camera to refer based on their culling masks
    int layer;
    public BuildTargetFlags BuildTargets; // sets on which platform to show cursor on
    
    private void Awake()
    {
        //checks if current platform exists in the platform list
        if ((BuildTargets & Application.platform.ToBuildTargetFlag()) != 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
            return;
        }

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        layer = GetFirstLayer(CameraLayerToUse);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.layer = layer;
        }
    }

    int GetFirstLayer(LayerMask mask)
    {
        int layer = 0;
        int bitmask = mask.value;

        while ((bitmask & 1) == 0 && layer < 32) // Find first set bit
        {
            bitmask >>= 1;
            layer++;
        }

        return layer < 32 ? layer : 0; // Return 0 if no layer is found
    }

    Camera GetCamera()
    {
        List<Camera> allCams = new();
        allCams.AddRange(FindObjectsByType<Camera>(FindObjectsSortMode.None));
        return allCams.Find(cam => (cam.cullingMask & CameraLayerToUse.value) == CameraLayerToUse.value);
    }

    void Start()
    {
        // Hide the system cursor if desired
        Cursor.visible = false;
        CursorImg.sprite = Idle;

        InputManager.Instance.Held += PlayHeldAnimation;
        InputManager.Instance.Pressed += PlayTapAnimation;
        InputManager.Instance.Released += PlayIdleAnimation;

#if UNITY_ANDROID || UNITY_IOS
        InputManager.Instance.Held += ChangePositionOnHeld;
        InputManager.Instance.Pressed += ChangePositionOnTouch;
#else
        InputManager.Instance.Moved += ChangePositionWithMouseMove;
#endif

    }

    void PlayTapAnimation(Vector3 mousePos)
    {
        if (!animationPlaying)
        {
            animationPlaying = true;
            CursorImg.sprite = Tap;
        }
    }

    void PlayHeldAnimation(Vector3 mousePos)
    {
        if (!animationPlaying)
        {
            animationPlaying = true;
            CursorImg.sprite = Held;
        }
    }

    void PlayIdleAnimation()
    {
        CursorImg.sprite = Idle;
        animationPlaying = false;
    }

    void ChangePositionOnTouch(Vector3 mousePos)
    {
        if (cam == null)
            cam = GetCamera();
        else
            transform.position = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane * 200));
    }

    void ChangePositionOnHeld(Vector3 mousePos)
    {
        if (cam == null)
            cam = GetCamera();
        else
            transform.position = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane * 200));
    }

    void ChangePositionWithMouseMove(Vector3 mousePos)
    {
        Vector3 cursorPosition = mousePos;
        // Clamp the cursor position within the screen bounds if needed
        cursorPosition.x = Mathf.Clamp(cursorPosition.x, 0, Screen.width);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, 0, Screen.height);

        // Update the in-game cursor's position
        if (cam == null)
            cam = GetCamera();
        else
            transform.position = cam.ScreenToWorldPoint(new Vector3(cursorPosition.x, cursorPosition.y, cam.nearClipPlane * 200));
    }
}
