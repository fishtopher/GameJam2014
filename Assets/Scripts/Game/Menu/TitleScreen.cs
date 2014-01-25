using UnityEngine;
using System.Collections;
using InControl;

public class TitleScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
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
