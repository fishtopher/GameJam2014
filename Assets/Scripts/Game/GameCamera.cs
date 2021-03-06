﻿using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour 
{
	public Transform m_target;
	Vector3 m_startPos;
	Vector3 m_offset;

	// Use this for initialization
	void Start () 
	{
		m_startPos = transform.position;
		Vector3 d = transform.position - m_target.position;
		m_offset = new Vector3(d.x, 0, d.z);
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		Vector3 np = m_target.position + m_offset;
		np.y = m_startPos.y;
		transform.position = np;
	}
}
