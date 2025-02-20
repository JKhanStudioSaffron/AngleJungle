using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerGem : MonoBehaviour 
{
	public GameObject PowerGemParticle;
	public int ActivateNum = 1;

	private Sprite greyPG;
	private Sprite redPG;
	public GameObject[] LightObjects;

	private SpriteRenderer sp;

    HashSet<GameObject> activeLights = new HashSet<GameObject>();
	public static Action<GameObject> CheckLightActivated;

	public List<SlotPositions> SlotPositions;


    private void OnEnable()
    {
		CheckLightActivated += IfMirrorLightOff;
    }

    private void OnDisable()
    {
        CheckLightActivated -= IfMirrorLightOff;
    }


    /// <summary>
    /// Initialize power dots on the power gem
    /// </summary>
    void Start () 
    {
		sp = GetComponent<SpriteRenderer> ();

		SlotPositions CurrentSlot = SlotPositions.Find(slot => slot.Offsets.Length == ActivateNum);

        for (int i = 0; i < ActivateNum; i++)
        {
			LightObjects[i].SetActive(false);
			LightObjects[i].transform.localPosition = CurrentSlot.Offsets[i];
        }
        greyPG = CurrentSlot.GreyObject;
        redPG = CurrentSlot.RedObject;


		sp.sprite = greyPG;
		PowerGemParticle.SetActive (false);
	}
	
	/// <summary>
	/// Set the threshold of activited power dots needed for win
	/// </summary>
	
	void ActivateLightObjects()
	{
		for(int i = 0;i < LightObjects.Length; i++)
		{
            LightObjects[i].SetActive (false);
			if(i < activeLights.Count)
                LightObjects[i].SetActive (true);
		}
	}


    /// <summary>
    /// Set the power gem to activated mode. it 's not necessary mean you win. 
    /// You still need to wait for the particle light to give it enough energy
    /// particle system will call this method on collision to increase power gem energy
    /// Can be interrupted by DeactivateGem
    /// </summary>

    public void ActivateGem(GameObject light)
    {
        activeLights.Add(light); // Register light source

        if (activeLights.Count >= ActivateNum) // Require at least two colliding lights
        {
            PowerGemParticle.SetActive(true);
            sp.sprite = redPG;
			GameManager.PuzzleSolved?.Invoke();
        }
        ActivateLightObjects();
    }

    /// <summary>
    /// Deactivates the gem, interrupt ActivateGem()
    /// </summary>
    /// 
    public void DeactivateGem(GameObject light)
    {
        activeLights.Remove(light); // Remove when collision stops

        if (activeLights.Count < ActivateNum) // If less than 2 lights, deactivate
        {
            PowerGemParticle.SetActive(false);
            sp.sprite = greyPG;
            GameManager.PuzzleUnSolved?.Invoke();
        }

        ActivateLightObjects();
    }

    /// <summary>
    /// Deactivates the gem, if light coming from mirror stops.
    /// </summary>
    /// 
    void IfMirrorLightOff(GameObject light)
	{
		if (activeLights.Contains(light))
		{
			DeactivateGem(light);
		}
	}
}
