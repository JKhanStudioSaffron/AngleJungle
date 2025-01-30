using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Purpose: Handles AudioData i.e: Clip and its properties and Aduio mixer properties
/// Responsibilities: Searches and send the respective AudioClipHolder
/// Usage: Create this from Assets > Create menu and fill the data.
/// </summary>

[CreateAssetMenu(fileName = "AudioClips", menuName = "AudioClipsHolder", order = 1)]
public class AudioClipHolder : ScriptableObject
{
    public List<Audios> Audios;

    public Audios GetAudio(Clips audioName)
    {
        return Audios.Find(clipName => clipName.Audio == audioName);
    }

}
