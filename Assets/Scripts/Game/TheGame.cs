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
	public GameObject m_creditsScene;

	// Use this for initialization
	void Awake () 
	{
		Instance = this;
		InputManager.Setup();

		// Just in case...
		m_titleScene.SetActive(true);
		m_introScene.SetActive(false);
		m_gameScene.SetActive(false);
		m_endScene.SetActive(false);
		m_creditsScene.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		InputManager.Update();
	}
	
	public void PlayIntro()
	{
		m_titleScene.SetActive(false);
		m_introScene.SetActive(true);
		m_gameScene.SetActive(false);
		m_endScene.SetActive(false);
		m_creditsScene.SetActive(false);
	}

	public void PlayCredits()
	{
		m_titleScene.SetActive(false);
		m_introScene.SetActive(false);
		m_gameScene.SetActive(false);
		m_endScene.SetActive(false);
		m_creditsScene.SetActive(true);
		
	}

	public void StartGame()
	{
		m_titleScene.SetActive(false);
		//m_introScene.SetActive(false);
		m_endScene.SetActive(false);
		m_gameScene.SetActive(true);
		m_creditsScene.SetActive(false);
		m_level.Reset();
		
	}
}
