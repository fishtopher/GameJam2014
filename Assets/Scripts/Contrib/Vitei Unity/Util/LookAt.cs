using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour 
{
	public Transform m_target = null;
	public enum Axis {X, Y, Z, Free};
	public Axis axis = Axis.Free; 

	public bool m_lookAtMainCameraIfNoTargetSet = true;

	void Start()
	{
		if(m_lookAtMainCameraIfNoTargetSet)
			m_target = Camera.main.transform;
	}
 
	void  Update ()
	{	
		if(axis == Axis.Free)
		{
			transform.LookAt (m_target.position);
			return;
		}
		
		Vector3 v = m_target.position - transform.position;
		
		if(axis == Axis.X)
		{
	        v.y = 0.0f;
			v.z = 0.0f;
		}
		else if(axis == Axis.Y)
		{
	        v.x = 0.0f;
			v.z = 0.0f;
		}
		else if(axis == Axis.Z)
		{
	        v.x = 0.0f;
			v.y = 0.0f;
		}
		transform.LookAt(m_target.position - v);
	}
}
