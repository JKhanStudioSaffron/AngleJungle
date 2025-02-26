using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour {

    public GameObject InfoCanvas;
    public GameObject ResetCanvas;
    public GameObject ResetConfirmCanvas;
	bool showLoading = false;

    // Use this for initialization
    void Awake() 
    {
        InfoCanvas.SetActive(false);
        ResetCanvas.SetActive(false);
        ResetConfirmCanvas.SetActive(false);
        SaveLoad.Load();
    }

	void Start () 
    {
		showLoading = false;		
        SetUpLastSettings();
	}

    void SetUpLastSettings()
    {
        var selectedLocale = LocalizationSettings.AvailableLocales.Locales[AccessibilitySaveObject.Instance.OptionsData.Language];
        LocalizationSettings.SelectedLocale = selectedLocale;
        TextResize.ResizeText?.Invoke(AccessibilitySaveObject.Instance.OptionsData.FontMultiplier);
    }
    private void OnGUI()
    {
	    if (showLoading) 
        {
			GUIStyle loadingStyle = new GUIStyle ();
			Font myfont = (Font)Resources.Load (Global.FONT_ASAP_MEDIUM, typeof(Font));
			loadingStyle.font = myfont;
			loadingStyle.fontSize = 60;
			loadingStyle.alignment = TextAnchor.MiddleCenter;
			loadingStyle.normal.textColor = Color.white;
			loadingStyle.hover.textColor = Color.white;
			GUI.depth = -1001;																// make the black texture render on top (drawn last)
			GUI.Label (new Rect (Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100), "Loading....", loadingStyle);
		}

    }

    public void Reset(int level)
    {
        SaveLoad.data = new PlayerData (level, 0, false);
        SaveLoad.Save ();
        ShowResetConfirm();
    }

    public void LoadStage()
    {
		MusicManager.Instance.PlayMenuButton ();
		
        //reset everything
		if (SaveLoad.data.LevelProgress < 1) {
			SaveLoad.data.LevelProgress = 1;
			SaveLoad.data.TrophyOrder.Shuffle ();
            Trophys.Instance.ShuffleTrophies();
			SaveLoad.Save ();
			SceneManager.LoadScene (Global.SCENE_MAP);
        }
        else 
        { 
            SceneManager.LoadScene (Global.SCENE_MAP);
        }
    }
	
    IEnumerator LoadStageCo(string stageName)
    {
		showLoading = true;
		float fadeTime = GetComponent<Fading> ().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene (stageName);
	}

    public void HideInfo()
    {
        InfoCanvas.SetActive(false);
		MusicManager.Instance.PlayClickButton ();
    }

    public void ShowInfo()
    {
        InfoCanvas.SetActive(true);
		MusicManager.Instance.PlayMenuButton ();
    }

    public void ShowReset()
    {
        ResetCanvas.SetActive(true);
		MusicManager.Instance.PlayMenuButton ();
    }

    public void HideReset()
    {
        ResetCanvas.SetActive(false);
		MusicManager.Instance.PlayClickButton ();
    }

    private void ShowResetConfirm()
    {
        ResetCanvas.SetActive(false);
        ResetConfirmCanvas.SetActive(true);
		MusicManager.Instance.PlayClickButton ();
    }

    public void HideResetConfirm()
    {
        ResetConfirmCanvas.SetActive(false);
		MusicManager.Instance.PlayClickButton ();
    }

    public void ShowSettings()
    {
        SceneManager.LoadScene(Global.SCENE_SETTINGS);
    }
}
