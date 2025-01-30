﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrophyManager : MonoBehaviour {

	public GameObject ExplosionEffect;
	public Animator playerAnimator;
	int TrophyNum = 0;
	bool showAnim = false;

	List<GameObject> trophyList = new List<GameObject>();
	GameObject animTrophy;
	public List<Transform> trophyPosList = new List<Transform>();
	public List<GameObject> bagList = new List<GameObject>();
	private GameObject musicManager;
	// Use this for initialization
	
    void Start () 
    {
		int levelProgress = SaveLoad.data.LevelProgress;
		SaveLoad.Load ();
        playerAnimator.SetBool (Global.ANIMATION_HAPPY, true);
        playerAnimator.SetBool (Global.ANIMATION_IDLE, false);
        playerAnimator.SetBool (Global.ANIMATION_CELEBRATE, false);

		if (levelProgress > 30) 
        {
			TrophyNum = 6;
		} 
        else if (levelProgress > 25) 
        {
			TrophyNum = 5;
		} 
        else if (levelProgress > 18) 
        {
			TrophyNum = 4;
		} 
        else if (levelProgress > 13) 
        {
			TrophyNum = 3;
		} 
        else if (levelProgress > 7) 
        {
			TrophyNum = 2;
		}
        else if (levelProgress > 1) 
        {
			TrophyNum = 1;
		} 
        else 
        {
			TrophyNum = 0;
            playerAnimator.SetBool (Global.ANIMATION_IDLE, true);
            playerAnimator.SetBool (Global.ANIMATION_HAPPY, false);
            playerAnimator.SetBool (Global.ANIMATION_CELEBRATE, false);
		}

		//chest and bags
		if(levelProgress > 1)
        {
			bagList [0].SetActive (true);
		}
		if(levelProgress > 3)
        {
			bagList [1].SetActive (true);
		}
		if(levelProgress > 5)
        {
			bagList [2].SetActive (true);
		}
		if(levelProgress > 9)
        {
			bagList [3].SetActive (true);
		}
		if(levelProgress > 11)
        {
			bagList [4].SetActive (true);
		}
		if(levelProgress > 15)
        {
			bagList [5].SetActive (true);
		}
		if(levelProgress > 17)
        {
			bagList [6].SetActive (true);
		}
		if(levelProgress > 20)
        {
			bagList [7].SetActive (true);
		}
		if(levelProgress > 22)
        {
			bagList [8].SetActive (true);
		}
		if(levelProgress > 27)
        {
			bagList [9].SetActive (true);
		}
		if(levelProgress > 29)
        {
			bagList [10].SetActive (true);
		}
            
		if (SaveLoad.data.TrophyGot < TrophyNum) 
        {
			SaveLoad.data.TrophyGot = TrophyNum;
			SaveLoad.Save();
			showAnim = true;
		}

		for (int i = 0; i < TrophyNum; i++) 
        {
			trophyList.Add(Instantiate (GetComponent<Trophys> ().TrophyList [i], trophyPosList[i].position, Quaternion.identity));
		}

		if (showAnim) 
        {
            playerAnimator.SetBool (Global.ANIMATION_HAPPY, false);
            playerAnimator.SetBool (Global.ANIMATION_IDLE, false);
            playerAnimator.SetBool (Global.ANIMATION_CELEBRATE, true);
			animTrophy = trophyList [TrophyNum - 1];
			animTrophy.SetActive (false);
			StartCoroutine (CountToShowAnim());
		}

		if (levelProgress <= 1) 
        {
			MusicManager.Instance.PlayCabinSound ();
		} 
        else
        {
			MusicManager.Instance.PlayCabin2Sound ();
		}

	}

	public void ReturnMap()
    {
		MusicManager.Instance.PlayClickButton ();
        SceneManager.LoadScene (Global.SCENE_MAP);
	}

	IEnumerator CountToShowAnim()
    {
		yield return new WaitForSeconds (0.4f);
		animTrophy.SetActive (true);
		Instantiate (ExplosionEffect, animTrophy.transform.position, Quaternion.identity);
	}
}
