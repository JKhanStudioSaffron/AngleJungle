using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
	public GameObject mainCamera;
	public GameObject canvasInGame;
	public GameObject canvasPause;
	public GameObject canvasStageClear;
	public GameObject canvasLastStageClear;
	public GameObject Player;
	public GameObject GoButton;
	public GameObject door;


	private GameObject[] protractors;

	public AudioSource goButton_as;
	public AudioSource music_win;
	public AudioSource complete_level;

	private Coroutine countToWin;

	private bool isStartToWinCalled = false;
	private bool isProtractorOn = false;
	private bool isPauseMenuOn = false;
	bool isLevelFinished = false;

	public static Action PuzzleSolved, PuzzleUnSolved;


	// Use this for initialization
	void Awake()
	{
		// Start timing level for analytics
		AnalyticsSingleton.Instance.levelStart = Time.time;
		AnalyticsSingleton.Instance.levelName = "LEVEL_" + LevelManager.LevelIndex.ToString();// SceneManager.GetActiveScene().name;

		isPauseMenuOn = false;
		isLevelFinished = false;
		StartCoroutine(AntiCheater());
		mainCamera.GetComponent<RapidBlurEffect>().enabled = false;
		canvasInGame.SetActive(true);
		canvasPause.SetActive(false);
		canvasStageClear.SetActive(false);
        canvasLastStageClear.SetActive(false);
		GoButton.SetActive(false);
		Global.isDragging = false;
		Global.isPaused = false;
		protractors = GameObject.FindGameObjectsWithTag(Global.TAG_PROTRACTOR);

	}

	private void OnEnable()
	{
		InputManager.Instance.Pressed += GoToNextLevel;
		PuzzleSolved += DoorPuzzleSolved;
		PuzzleUnSolved += DoorPuzzleUnsolved;
	}

	private void OnDisable()
	{
		InputManager.Instance.Pressed -= GoToNextLevel;
		PuzzleSolved -= DoorPuzzleSolved;
		PuzzleUnSolved -= DoorPuzzleUnsolved;
	}

	void GoToNextLevel(Vector3 position)
	{
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(position), Vector2.zero);

		if (hit && hit.collider != null && hit.collider.tag == Global.TAG_GO_BUTTON && !isPauseMenuOn)
		{
			goButton_as.Play();
			Win();
		}
	}

	/// <summary>
	/// Shows the pause menu.
	/// </summary>
	public void ShowPauseMenu()
	{
		mainCamera.GetComponent<RapidBlurEffect>().enabled = true;
		canvasPause.SetActive(true);
		canvasInGame.SetActive(false);
		isPauseMenuOn = true;
	}

	/// <summary>
	/// All buttons clicks are using this sound
	/// </summary>
	public void PlayButtonClickSound()
	{
		MusicManager.Instance.PlayClickButton();
	}

	/// <summary>
	/// Show congrat UI when you complete this level
	/// </summary>
	public void ShowStageClear()
	{
		music_win.Play();
		mainCamera.GetComponent<RapidBlurEffect>().enabled = true;
		if(LevelManager.LevelIndex == LevelManager.TotalLevels)
			canvasLastStageClear.SetActive(true);
		else
			canvasStageClear.SetActive(true);

		canvasInGame.SetActive(false);
		canvasPause.SetActive(false);
		Global.isPaused = true;
		Invoke(nameof(RefreshText), 0.5f);
	}

	void RefreshText()
	{
		RTLTextFixer.RefreshText?.Invoke();
		TextResize.ResizeText?.Invoke(AccessibilitySaveObject.Instance.OptionsData.FontMultiplier);
	}

	/// <summary>
	/// Shows the treasure if this level is trophy level
	/// </summary>
	public void ShowTreasure()
	{
		music_win.Play();
		StartCoroutine(GoToTreasureRoom());
	}

	/// <summary>
	/// Gos to treasure room if this level is trophy level.
	/// </summary>
	/// <returns>The to treasure room.</returns>
	IEnumerator GoToTreasureRoom()
	{
		yield return new WaitForSeconds(3.6f);
		SceneManager.LoadScene(Global.SCENE_TREASURE);
	}

	/// <summary>
	/// We use this to prevent making angles without placement
	/// </summary>
	IEnumerator AntiCheater()
	{
		while (true)
		{
			yield return new WaitForSeconds(1);
			if (Global.antiCheater > 0)
			{
				Global.antiCheater--;
			}
		}
	}

	/// <summary>
	/// Hides the pause menu.
	/// </summary>
	public void HidePauseMenu()
	{
		mainCamera.GetComponent<RapidBlurEffect>().enabled = false;
		canvasPause.SetActive(false);
		canvasInGame.SetActive(true);
		isPauseMenuOn = false;
	}

	/// <summary>
	/// Puzzle solved
	/// </summary>
	public void DoorPuzzleSolved()
	{
		if (!isStartToWinCalled)
		{
			// when the puzzle solved, we call the coroutine to win, but it's not really the winning.
			// it can still be interupted by DoorClosed() method.
			countToWin = StartCoroutine(CountToWin());
			isStartToWinCalled = true;
			//disable hand click tutorial
			GameObject handClickTutorial = GameObject.FindGameObjectWithTag(Global.TAG_HAND_TUTORIAL);

			if (handClickTutorial != null)
				handClickTutorial.SetActive(false);
		}
	}

	/// <summary>
	/// Puzzle solved status reverted, interuppt DoorOpen()
	/// </summary>
	public void DoorPuzzleUnsolved()
	{
		if (countToWin != null)
			StopCoroutine(countToWin);

		isStartToWinCalled = false;
		if(GoButton != null)
			GoButton.SetActive(false);
	}

	/// <summary>
	/// Restart the level
	/// </summary>
	public void ReloadCurrentScene()
	{
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

	/// <summary>
	/// Loads the next level.
	/// </summary>
	public void LoadNextScene()
	{
		LevelManager.LevelIndex++;
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.buildIndex);
	}

	/// <summary>
	/// Loads the map scene.
	/// </summary>
	public void LoadHomeScene()
	{
		SceneManager.LoadScene(Global.SCENE_MAP);
	}

	/// <summary>
	/// Loads the trophy room scene.
	/// </summary>
	public void LoadTreasureScene()
	{
		SceneManager.LoadScene(Global.SCENE_TREASURE);
	}

	/// <summary>6
	/// Show congrat UI when you complete this level
	/// </summary>
	public void StageClear()
	{
		ShowStageClear();
	}

	/// <summary>
	/// Real winning
	/// The little character started walking towards the chest box
	/// </summary>
	void Win()
	{
		GoButton.SetActive(false);
		canvasInGame.SetActive(false);
		canvasPause.SetActive(false);
		mainCamera.GetComponent<RapidBlurEffect>().enabled = false;
		Global.isPaused = true;
		Player.GetComponent<Player>().Win();
	}

	/// <summary>
	/// Counts to Win().
	/// </summary>
	/// <returns>The to window.</returns>
	IEnumerator CountToWin()
	{
		yield return new WaitForSeconds(2.4f);
		GoButton.SetActive(true);
		complete_level.Play();
		Global.isPaused = true;
		isLevelFinished = true;
		door.GetComponent<Door>().DoorOpened();
		Player.GetComponent<Player>().FeelHappy();

		// Calculate level time and send to analytics
		AnalyticsSingleton.Instance.levelEnd = Time.time;

		Mirror[] mirrors = ((Mirror[])GameObject.FindObjectsOfType(typeof(Mirror)));
		AnalyticsSingleton.Instance.gemEndState.BuildGemEndState(mirrors.ToList());
		AnalyticsSingleton.Instance.CalculateLevelTime();
		AnalyticsSingleton.Instance.DispatchData();
	}

	/// <summary>
	/// Toggle on all protractors
	/// </summary>
	public void ToggleAllProtractors()
	{
		if (isProtractorOn)
		{
			foreach (GameObject pro in protractors)
			{
				pro.SetActive(false);
			}
		}
		else
		{
			foreach (GameObject pro in protractors)
			{
				pro.SetActive(true);
			}
		}

		isProtractorOn = !isProtractorOn;
	}
}
