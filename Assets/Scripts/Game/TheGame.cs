using UnityEngine;
using System.Collections;
using InControl;

public class TheGame : MonoBehaviour {

	public static TheGame Instance;

	public Level m_level;

	//Transitions
	public GameObject m_titleScene;
	public GameObject m_introScene;
	public GameObject m_gameScene;
	public GameObject m_endScene;

	// Use this for initialization
	void Awake () 
	{
		Instance = this;
		InputManager.Setup();
	}
	
	// Update is called once per frame
	void Update () 
	{
		InputManager.Update();
	}
	
	public void PlayIntro()
	{
		printf.PrintPersistentMessage("playintro");
		
		m_introScene.SetActive(true);
		m_gameScene.SetActive(false);

	}

	public void StartGame()
	{
		m_titleScene.SetActive(false);
		m_endScene.SetActive(false);
		m_gameScene.SetActive(true);
		m_level.Reset();
		
	}
}
