using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLabel : MonoBehaviour {
	
    public bool UpperCase = false;
	
    // Use this for initialization
	void Start () 
    {
		int levelIndex = LevelManager.LevelIndex;
		
		string localizedIndex = LocalizedNumberManager.GetLocalizedNumber(levelIndex);

		string levelIndexStr = LocalizedNumberManager.Level + " " + localizedIndex;

		if (UpperCase)
			levelIndexStr = levelIndexStr.ToUpper();

		GetComponent<TMP_Text>().text = levelIndexStr;
	}
}
