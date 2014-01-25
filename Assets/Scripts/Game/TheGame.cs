using UnityEngine;
using System.Collections;
using InControl;

public class TheGame : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		InputManager.Setup();
		printf.PrintPersistentMessage("Lets go");
	}
	
	// Update is called once per frame
	void Update () 
	{
		InputManager.Update();


		if(InputManager.ActiveDevice.GetControl(InputControlType.Start).WasPressed)
		{
			printf.PrintPersistentMessage("Start Pressed");
		}
	}
}
