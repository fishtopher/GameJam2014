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
		
		if(InputManager.ActiveDevice.GetControl(InputControlType.Action1).WasPressed)	//space
		{
			printf.PrintPersistentMessage("Action1 Pressed");
		}
		
		if(InputManager.ActiveDevice.GetControl(InputControlType.Action2).WasPressed)	//z
		{
			printf.PrintPersistentMessage("Action2 Pressed");
		}
		
		if(InputManager.ActiveDevice.GetControl(InputControlType.Action3).WasPressed)	//x
		{
			printf.PrintPersistentMessage("Action3 Pressed");
		}
		
		if(InputManager.ActiveDevice.GetControl(InputControlType.Action4).WasPressed)	//c
		{
			printf.PrintPersistentMessage("Action4 Pressed");
		}
	}
}
