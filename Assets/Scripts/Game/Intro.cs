using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

	public void PlayAnim()
	{
		printf.PrintPersistentMessage("letsgo");
		animation.Play("GetReadyAnim");
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!animation.isPlaying)
			gameObject.SetActive(false);
	}
	
	public void ScreenIsCovered()
	{
		
	}

	public void PlaySound()
	{
		SoundManager.PlaySound("countdown");
	}

	public void PlayStartSound()
	{
		SoundManager.PlaySound("start");
	}

	public void ScreenIsUncovered()
	{
		TheGame.Instance.StartGame();	
	}
}
