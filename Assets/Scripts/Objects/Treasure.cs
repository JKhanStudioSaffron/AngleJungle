using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Purpose: Manages treasure chest interactions and rewards upon level completion.
/// Responsibilities:
/// - Controls chest opening animation and effects when the player reaches it.
/// - Checks if the player is completing the level for the first time and updates progress.
/// - Instantiates and animates the level-specific treasure if applicable.
/// - Triggers scene transitions after a short delay.
/// Usage:
/// - Attach to a treasure chest GameObject in the scene.
/// - Ensure references to required objects (GameManager, particle effects, sprites) are assigned.
/// - Automatically manages treasure display and level progression upon player collision.
/// </summary>

public class Treasure : MonoBehaviour {

	public Sprite sp_closedChest;
	public Sprite sp_openChest;
	public GameObject par_Chest;
	public GameObject GM;
	public GameObject TreasureCanvas;
	int levelIndex;
	GameObject treasure;
	Vector3 originTreasureScale;
	float timeTillSceneChange = 4.5f;

    // Use this for initialization
    void Start () 
    {
		GetComponent<SpriteRenderer> ().sprite = sp_closedChest;
		par_Chest.SetActive (false);
		levelIndex = LevelManager.LevelIndex;

		MusicManager.Instance.PlayLevelMusic(levelIndex);

		if(TreasureCanvas != null)
			TreasureCanvas.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == Global.TAG_PLAYER) 
        {
			bool isFirstTimePassThisLevel = false;
			SaveLoad.Load ();

			if (SaveLoad.data.LevelProgress == levelIndex) 
            {
				SaveLoad.data.LevelProgress++;
				SaveLoad.Save ();
				isFirstTimePassThisLevel = true;
			}

			coll.gameObject.GetComponent<Player> ().Get ();
			GetComponent<SpriteRenderer> ().sprite = sp_openChest;
			par_Chest.SetActive (true);

			if (isFirstTimePassThisLevel && Trophys.Instance.Data.Exists(level => level.LevelNoToOpenAt == levelIndex))
            {
				StartCoroutine (CountToShowTreasure ());				
			} 
            else 
            {
				StartCoroutine (CountToClearStage ());
			}
		}
	}

	void TreasureOnScreen()
    {
		//Instantiate Treasure
		treasure = Trophys.Instance.CreateTrophyOfLevel(levelIndex, transform);
		originTreasureScale = treasure.transform.localScale;

		treasure.transform.DOMove(Camera.main.transform.position, timeTillSceneChange);
		treasure.transform.DOScale(originTreasureScale * 2.4f, timeTillSceneChange);
		//TreasureCanvas
		if (TreasureCanvas != null) 
        {
			TreasureCanvas.SetActive (true);
			TreasureCanvas.GetComponentInChildren<Image>().DOFade(0.8f, timeTillSceneChange);
		}
	}

	IEnumerator CountToClearStage()
    {
		yield return new WaitForSeconds (1);
		GM.GetComponent<GameManager> ().StageClear ();
	}

	IEnumerator CountToShowTreasure()
    {
		yield return new WaitForSeconds (0.2f);
		TreasureOnScreen ();
		GM.GetComponent<GameManager> ().ShowTreasure ();
	}
}
