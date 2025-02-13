using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGem : MonoBehaviour 
{

	public GameObject PowerGemParticle;
	public int ActivateNum = 1;

	private Sprite greyPG;
	private Sprite redPG;
	public GameObject L1;
	public GameObject L2;
	public GameObject L3;
	public GameObject L4;

	public Sprite greyPG1;
	public Sprite greyPG2;
	public Sprite greyPG3;
	public Sprite greyPG4;

	public Sprite redPG1;
	public Sprite redPG2;
	public Sprite redPG3;
	public Sprite redPG4;

	private SpriteRenderer sp;

    HashSet<GameObject> activeLights = new HashSet<GameObject>();
	List<GameObject> lightObj = new List<GameObject>();
	public static Action<GameObject> CheckLightActivated;

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

        // Set initial sate
		L1.SetActive (false);
		L2.SetActive (false);
		L3.SetActive (false);
		L4.SetActive (false);
		
        // Set position of dots programatically
        switch (ActivateNum) 
		{
			case 1:
				greyPG = greyPG1;
				redPG = redPG1;
				L1.transform.localPosition = new Vector2 (-0.15f, -2.26f);
				lightObj.Add (L1);
				break;

			case 2:
				greyPG = greyPG2;
				redPG = redPG2;
				L1.transform.localPosition = new Vector2 (-0.924f, -2.188f);
				L2.transform.localPosition = new Vector2 (0.627f, -2.158f);
				lightObj.Add(L1);
				lightObj.Add(L2);
				break;

			case 3:
				greyPG = greyPG3;
				redPG = redPG3;
				L1.transform.localPosition = new Vector2 (-1.429f, -2.164f);
				L2.transform.localPosition = new Vector2 (-0.06f, -2.298f);
				L3.transform.localPosition = new Vector2 (1.272f, -2.17f);
				lightObj.Add(L1);
				lightObj.Add(L2);
				lightObj.Add(L3);
				break;

			case 4:
				greyPG = greyPG4;
				redPG = redPG4;
				L1.transform.localPosition = new Vector2 (-1.903f, -1.981f);
				L2.transform.localPosition = new Vector2 (-0.869f, -2.231f);
				L3.transform.localPosition = new Vector2 (0.481f, -2.261f);
				L4.transform.localPosition = new Vector2 (1.77f, -1.994f);
				lightObj.Add(L1);
				lightObj.Add(L2);
				lightObj.Add(L3);
				lightObj.Add(L4);
				break;
		}

		sp.sprite = greyPG;
		PowerGemParticle.SetActive (false);
	}
	
	/// <summary>
	/// Set the threshold of activited power dots needed for win
	/// </summary>
	
	void ActivateLightObjects()
	{
		for(int i = 0;i < lightObj.Count; i++)
		{
			lightObj[i].SetActive (false);
			if(i < activeLights.Count)
				lightObj[i].SetActive (true);
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
