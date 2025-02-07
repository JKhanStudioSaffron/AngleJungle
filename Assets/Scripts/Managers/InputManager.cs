using System;
using UnityEngine;

/// <summary>
/// Purpose: Manages user input for touch and mouse interactions in the game, providing a unified interface for detecting presses, holds, and releases.
/// Responsibilities: Detects input from both touch devices and desktop environments, triggering corresponding actions when input events occur. It also ensures that the InputManager persists across scenes.
/// Usage: Attach this script to a GameObject that needs to handle user input. Other scripts can subscribe to the Pressed, Held, and Released actions to respond to user interactions.
/// </summary>


public class InputManager : MonoBehaviour
{
    public Action<Vector3> Pressed, Held, Moved;
    public Action Released;

    public static InputManager Instance;

    private Vector3 lastCursorPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        lastCursorPosition = Input.mousePosition;
    }

    private void Update()
    {
        #if UNITY_ANDROID || UNITY_IOS
        // Check if there's at least one touch on the screen
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch (index 0)
            
            // Handle touch phases
            if (touch.phase == TouchPhase.Began)
            {
                // Touch began (equivalent to mouse down)
                Pressed?.Invoke(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                // Touch is being held (equivalent to mouse hold)
                Held?.Invoke(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // Touch ended (equivalent to mouse up)
                Released?.Invoke();
            }
        }

        #else

        //checks if mouse moves
        if (lastCursorPosition != Input.mousePosition)
        {
            lastCursorPosition = Input.mousePosition;
            Moved?.Invoke(lastCursorPosition);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Pressed?.Invoke(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Held?.Invoke(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Released?.Invoke();
        }

        #endif

    }
}
