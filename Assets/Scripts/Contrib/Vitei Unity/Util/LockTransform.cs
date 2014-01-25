using UnityEngine;
using System.Collections;

public class LockTransform : MonoBehaviour 
{
	public bool m_lockLocally;
	public bool m_lockPosition;
	public bool m_lockRotation;
	
	Vector3 m_lockedPosition;
	Quaternion m_lockedRotation;
	
	// Use this for initialization
	void Start () 
	{
		if(m_lockPosition)
			LockPosition();
		if(m_lockRotation)
			LockRotation();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_lockPosition)
		{
			if(m_lockLocally)
				transform.localPosition = m_lockedPosition;
			else
				transform.position = m_lockedPosition;
		}
		if(m_lockRotation)
			{
			if(m_lockLocally)
				transform.localRotation = m_lockedRotation;
			else
				transform.rotation = m_lockedRotation;
		}
		
	}
	
	public void LockPosition()
	{
		m_lockedPosition = m_lockLocally ? transform.localPosition: transform.position;
		m_lockPosition = true;
	}
	
	public void LockRotation()
	{
		m_lockedRotation = m_lockLocally ? transform.localRotation : transform.rotation;
		m_lockRotation = true;
	}
}
