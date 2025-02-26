using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Purpose: Manages background music and sound effects for different game scenes and events.
/// Responsibilities:
/// - Ensures a singleton instance for global access.
/// - Handles background music playback and transitions with fade-out effects.
/// - Plays specific sounds based on level progression and user interactions.
/// - Prevents overlapping audio by stopping previous sounds before playing new ones.
/// Usage:
/// - Call PlayLevelMusic(levelIndex) to play music for a specific level.
/// - Use PlayBGM(), PlayMAPSound(), PlayCabinSound(), etc., to trigger corresponding sounds.
/// - Call PlayClickButton() or PlayMenuButton() for UI interactions.
/// - Background music transitions are handled via fade-out coroutines.
/// </summary>

public class MusicManager : MonoBehaviour {
	
	public static MusicManager Instance;
    private void Awake()
    {
        if(Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(gameObject);
    }

    public AudioSource BGM_as;
	public AudioSource MAP_as;
	public AudioSource CABIN_as;
	public AudioSource CABIN_as2;
	public AudioSource S1_as;
	public AudioSource S2_as;
	public AudioSource S3_as;
	public AudioSource S4_as;
	public AudioSource S5_as;
	public AudioSource MenuClick_as;
	public AudioSource Click_as;
	public AudioSource CreakClick_as;
	private Coroutine curCo;
	private AudioSource curAs;

	public List<MusicForLevels> MusicForLevels;

	public void PlayLevelMusic(int levelIndex)
	{
		UnityEvent funct = MusicForLevels.FirstOrDefault(msc => msc.LevelIndices?.OtherLevelIndices?.Contains(levelIndex) == true)?.CallEvent;

        if (funct == null)
            funct = MusicForLevels.Find(index => (index.LevelIndices.startLevelIndex <= levelIndex && index.LevelIndices.endingLevelIndex >= levelIndex)).CallEvent;

		funct?.Invoke();
	}

	public void StopBGM()
	{
		curCo = StartCoroutine(fadeOut(BGM_as));
	}

	public void PlayBGM()
	{
		if (curAs == BGM_as)
			return;
		else
            if (curCo != null) StopCoroutine(curCo);

		curCo = StartCoroutine(fadeOut(curAs));
		curAs = BGM_as;
		curAs.volume = 1f;
		curAs.Play();
	}

	public void PlayMAPSound()
	{
		if (curAs == MAP_as)
			return;
        else
            if (curCo != null) StopCoroutine(curCo);

        curCo = StartCoroutine(fadeOut(curAs));
		curAs = MAP_as;
		curAs.volume = 1f;
		curAs.Play();
	}

	public void PlayCabinSound()
	{
		if (curAs == CABIN_as)
			return;
        else
            if(curCo != null) StopCoroutine(curCo);

        curCo = StartCoroutine(fadeOut(curAs));
		curAs = CABIN_as;
		curAs.volume = 1f;
		curAs.Play();
	}

	public void PlayCabin2Sound()
	{
		if (curAs == CABIN_as2)
			return;
        else
            if (curCo != null) StopCoroutine(curCo);

        curCo = StartCoroutine(fadeOut(curAs));
		curAs = CABIN_as2;
		curAs.volume = 1f;
		curAs.Play();
	}

	public void PlayS1Sound()
	{
		if (curAs == S1_as)
			return;
        else
            if (curCo != null) StopCoroutine(curCo);

        curCo = StartCoroutine(fadeOut(curAs));
		curAs = S1_as;
		curAs.volume = 1f;
		curAs.Play();
	}

	public void PlayS2Sound()
	{
		if (curAs == S2_as)
			return;
        else
           if (curCo != null) StopCoroutine(curCo);
		
		curCo = StartCoroutine(fadeOut(curAs));
		curAs = S2_as;
		curAs.volume = 1f;
		curAs.Play();
	}

	public void PlayS3Sound()
	{
		if (curAs == S3_as)
			return;
        else
            if (curCo != null) StopCoroutine(curCo);

		curCo = StartCoroutine(fadeOut(curAs));
		curAs = S3_as;
		curAs.volume = 1f;
		curAs.Play();
	}

	public void PlayS4Sound()
	{
		if (curAs == S4_as)
			return;
        else
            if (curCo != null) StopCoroutine(curCo);

		curCo = StartCoroutine(fadeOut(curAs));
		curAs = S4_as;
		curAs.volume = 1f;
		curAs.Play();
	}

	public void PlayS5Sound()
	{
		if (curAs == S5_as)
			return;
        else
           if (curCo != null) StopCoroutine(curCo);

		curCo = StartCoroutine(fadeOut(curAs));
		curAs = S5_as;
		curAs.volume = 1f;
		curAs.Play();
	}

	public void PlayMenuButton()
	{
		MenuClick_as.Stop ();
		MenuClick_as.Play ();
	}
		
	public void PlayClickButton()
	{
		Click_as.Stop ();
		Click_as.Play ();
	}

	public void PlayCreakClickButton()
	{
		CreakClick_as.Stop ();
		CreakClick_as.Play ();
	}

	IEnumerator fadeOut(AudioSource auSource)
	{
		if (auSource != null) 
		{
			float t = 1f;

			while (t > 0.0f) 
			{
				t -= Time.deltaTime * 0.8f;
				auSource.volume = t;
				yield return new WaitForSeconds (0);
				if (curAs == auSource)
					yield break;
			}

			if (curAs == auSource)
				yield break;
			
			auSource.volume = 0.0f;
			auSource.Stop ();
		}
	}
}
