﻿using UnityEngine;
using System.Collections;
using InControl;

public class LevelEnd : MonoBehaviour {

	void OnEnable () 
	{
		printf.PrintPersistentMessage("OVER");

		SoundManager.Stop("song1");

		// Drug testing!
		if ( Player.Instance.NumCollectedSteroids > 0 ) {
			StartCoroutine( FailEndActions() );
		}
		else {
			Player.Instance.GetComponent<Animator>().Play("win");
			SoundManager.PlaySound("win");
		}
		StartCoroutine( Player.Instance.StopSpeed() );
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(InputManager.ActiveDevice.GetControl(InputControlType.Start).WasPressed ||
		   InputManager.ActiveDevice.GetControl(InputControlType.Action1).WasPressed)
		{
			TheGame.Instance.PlayIntro();
		}
	
	}

	IEnumerator FailEndActions()
	{
		Player.Instance.GetComponent<Animator>().Play("fall");
		SoundManager.PlaySound("fall");

		yield return new WaitForSeconds(1.0f);
		SoundManager.PlaySound("boo");
	}
}
