using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Purpose: Manages trophy display and animations upon level completion.
/// Responsibilities:
/// - Controls player animation states based on trophy acquisition.
/// - Instantiates and activates trophies at designated positions.
/// - Handles trophy unlocking logic and updates saved progress.
/// - Triggers explosion effects for newly obtained trophies.
/// - Plays appropriate background music based on level progression.
/// Usage:
/// - Attach to a GameObject in the scene.
/// - Assign trophy positions, explosion effects, and player animator.
/// - Call ReturnMap() to navigate back to the map scene.
/// </summary>


public class TrophyManager : MonoBehaviour {

	public GameObject ExplosionEffect;
	public Animator playerAnimator;
	int TrophyNum = 0;
	bool showAnim = false;

	GameObject animTrophy;

	public List<Transform> TrophyPosList = new List<Transform>();

	public List<LevelRewardData> BagsData;

	// Use this for initialization	
    void Start () 
    {
		int levelProgress = SaveLoad.data.LevelProgress;
		SaveLoad.Load ();
        playerAnimator.SetBool (Global.ANIMATION_HAPPY, true);
        playerAnimator.SetBool (Global.ANIMATION_IDLE, false);
        playerAnimator.SetBool (Global.ANIMATION_CELEBRATE, false);

        Trophys.Instance.CreateTrophy(levelProgress, TrophyPosList);

        foreach (var item in BagsData.Where(bag => levelProgress > bag.LevelNoToOpenAt).Select(bag => bag.Reward))
        {
            item.SetActive(true);
        }

        TrophyNum = Trophys.Instance.TrophyList.Count - 1;

		if (SaveLoad.data.TrophyGot < TrophyNum) 
        {
			SaveLoad.data.TrophyGot = TrophyNum;
			SaveLoad.Save();
			showAnim = true;
		}

		if (showAnim) 
        {
            playerAnimator.SetBool (Global.ANIMATION_HAPPY, false);
            playerAnimator.SetBool (Global.ANIMATION_IDLE, false);
            playerAnimator.SetBool (Global.ANIMATION_CELEBRATE, true);
			animTrophy = Trophys.Instance.TrophyList[TrophyNum];
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
