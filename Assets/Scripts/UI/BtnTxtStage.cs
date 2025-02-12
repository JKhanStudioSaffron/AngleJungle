using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BtnTxtStage : MonoBehaviour {
	Vector3 originPos;
	Vector3 downPos;
	public Action<bool> ButtonState;

    private void OnEnable()
    {
		ButtonState += CheckButtonState;
    }

    private void OnDisable()
    {
        ButtonState -= CheckButtonState;
    }

    // Use this for initialization
    void Start () {
		originPos = gameObject.transform.position;
		downPos = originPos - new Vector3 (0, 0.03f, 0);
	}


	void CheckButtonState(bool state) { 
		if (!state)
			gameObject.SetActive (false);
		else
			gameObject.SetActive (true);
	}


	public void BtnDown(){
		gameObject.transform.position = downPos;
	}

	public void BtnUp(){
		gameObject.transform.position = originPos;
	}

}
