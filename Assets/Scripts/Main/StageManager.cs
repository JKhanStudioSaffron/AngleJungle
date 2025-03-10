﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StageManager : MonoBehaviour {

	public GameObject[] buttonsList;

	public int currentLevelProgress = 1;

	bool showLoading = false;

	// Use this for initialization
	void Start () 
	{
		MusicManager.Instance.PlayMAPSound ();

		showLoading = false;
		SaveLoad.Load ();
		currentLevelProgress = SaveLoad.data.LevelProgress;
		for(int i=0;i < buttonsList.Length;i++) 
		{
            bool btnInteract = false;
            buttonsList[i].GetComponentInChildren<TMP_Text>().text = LocalizedNumberManager.GetLocalizedNumber(i + 1);

			if (i < currentLevelProgress - 1)
			{
				buttonsList[i].GetComponent<StageButton>().ShowStar();
            }
            if(i <= currentLevelProgress - 1)
			{
                buttonsList[i].GetComponent<Button>().interactable = true;
                btnInteract = true;
            }
			buttonsList[i].GetComponentInChildren<BtnTxtStage>().ButtonState?.Invoke(btnInteract);

			int index = i + 1;
			buttonsList[i].GetComponent<Button>().onClick.AddListener(() => LoadStage(index));
		}
		LevelManager.TotalLevels = buttonsList.Length;
    }

	void LoadStage(int stageIndex)
	{
		MusicManager.Instance.PlayClickButton ();
		StartCoroutine (LoadStageCo(stageIndex));
	}

	public void LoadStart()
	{
		MusicManager.Instance.PlayClickButton ();
		SceneManager.LoadScene (Global.SCENE_START);
	}

    public void LoadTreasure() 
	{
		MusicManager.Instance.PlayCreakClickButton ();
		SceneManager.LoadScene(Global.SCENE_TREASURE);
    }

	IEnumerator LoadStageCo(int stageIndex)
	{
		showLoading = true;
		float fadeTime = GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		LevelManager.LevelIndex = stageIndex;
		SceneManager.LoadScene (Global.SCENE_LEVEL);
	}

	void OnGUI()
	{
		if (showLoading) 
		{
			GUIStyle loadingStyle = new GUIStyle ();
			Font myfont = (Font)Resources.Load ("fonts/Asap-Medium", typeof(Font));
			loadingStyle.font = myfont;
			loadingStyle.fontSize = 60;
			loadingStyle.alignment = TextAnchor.MiddleCenter;
			loadingStyle.normal.textColor = Color.white;
			loadingStyle.hover.textColor = Color.white;
			GUI.depth = -1001;																// make the black texture render on top (drawn last)
			GUI.Label (new Rect (Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100), "Loading....", loadingStyle);
		}
	}
}
