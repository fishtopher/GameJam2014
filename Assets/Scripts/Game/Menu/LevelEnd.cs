using UnityEngine;
using System.Collections;
using InControl;

public class LevelEnd : MonoBehaviour 
{
	public int m_realWorldAfter = 2;

	public Level m_level;
	public TextMesh m_headline;
	public TextMesh m_message;
	public TextMesh m_continuePrompt;
	int m_timesSeen = 0;

	void OnEnable () 
	{
		m_timesSeen++;

		m_message.text = "";
		
		if(m_timesSeen == m_realWorldAfter)
		{
			m_message.text = "BUT\n\nLIFE IS NOT A VIDEO GAME";
			m_level.m_universe = Item.Type.Real;
		}
		else if(m_timesSeen > (m_realWorldAfter+2))
		{
			m_message.text = "LIFE IS THIS FOREVER";
		}


		// Drug testing!
		if ( Player.Instance.NumCollectedSteroids > 0 ) 
		{
			Player.Instance.PlayAnimation("fall");
			m_headline.text = "LOSER";
			m_message.text = "WINNERS DON'T DO DRUGS";

		}
		else 
		{
			Player.Instance.PlayAnimation("win");
			m_headline.text = "WINNER";
		}

		Player.Instance.StopRunning();
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
