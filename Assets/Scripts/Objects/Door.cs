using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public GameObject Flame;
	public GameObject EyeMask;
	public GameObject DoorParticle;

	// Use this for initialization
	void Start () 
	{
		Flame.SetActive (true);
		EyeMask.SetActive (false);
	}

	/// <summary>
	/// Opens The Door.
	/// </summary>
	public void DoorOpened()
	{
		Flame.SetActive (false);
		EyeMask.SetActive (true);
		DoorParticle.SetActive(true);
	}

}
