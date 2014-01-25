using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
	public GameObject m_gameWorldSprite;
	public GameObject m_realWorldSprite;

	// Use this for initialization
	void Start () {
		m_realWorldSprite.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
