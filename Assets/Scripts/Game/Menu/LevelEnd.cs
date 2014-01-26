using UnityEngine;
using System.Collections;
using InControl;

public class LevelEnd : MonoBehaviour {

	void OnEnable () 
	{
		printf.PrintPersistentMessage("OVER");

		// Drug testing!
		if ( Player.Instance.NumCollectedSteroids > 0 ) {
			Player.Instance.PlayAnimation("fall");
		}
		else {
			Player.Instance.PlayAnimation("win");
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
}
