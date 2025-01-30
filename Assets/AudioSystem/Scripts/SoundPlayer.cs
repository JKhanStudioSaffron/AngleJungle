using UnityEngine;

/// <summary>
/// Purpose: Simplifies triggering audio playback for music and button press sounds.
/// Responsibilities:
/// - Plays the main theme music using `AudioManager`.
/// - Plays button press sound effects using `AudioManager`.
/// Usage: Attach to a GameObject in the scene. 
/// - Call `PlayMusic()` to play the main theme.
/// - Call `PlayButtonSound()` to trigger the button press sound.
/// </summary>

public class SoundPlayer : MonoBehaviour
{
    public void PlayMusic()
    {
        AudioManager.Instance.PlayMusic(Clips.MainTheme);
    }

    public void PlayButtonSound()
    {
        AudioManager.Instance.PlayAudio(Clips.ButtonPresses);
    }
}
