using UnityEngine;
using System.Collections;

public class ToggleActiveTrigger : MonoBehaviour {
	
	public GameObject[] m_gameObjects;
	bool[] m_defaultActive;
	
	// Use this for initialization
	void Start () 
	{
		m_defaultActive = new bool[m_gameObjects.Length];
		for(int i = 0; i < m_gameObjects.Length; i++)
		{
			m_defaultActive[i] = m_gameObjects[i].activeSelf;
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		for(int i = 0; i < m_gameObjects.Length; i++)
		{
			m_gameObjects[i].SetActive(!m_defaultActive[i]);
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		for(int i = 0; i < m_gameObjects.Length; i++)
		{
			m_gameObjects[i].SetActive(m_defaultActive[i]);
		}
	}
}
