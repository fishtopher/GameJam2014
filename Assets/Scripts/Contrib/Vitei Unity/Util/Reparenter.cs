using UnityEngine;
using System.Collections;

public class Reparenter : MonoBehaviour 
{
	public GameObject[] m_reparentToMe;
	
	// Use this for initialization
	void Start () 
	{
		for(int i = 0; i < m_reparentToMe.Length; i++)
		{
			m_reparentToMe[i].transform.parent = transform;
		}
	}
	
}
