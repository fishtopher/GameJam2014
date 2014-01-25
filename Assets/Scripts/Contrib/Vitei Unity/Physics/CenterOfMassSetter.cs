using UnityEngine;
using System.Collections;

public class CenterOfMassSetter : MonoBehaviour
{
	
	public Transform m_centerOfMass;
	
	// Use this for initialization
	void Awake () 
	{
		if(rigidbody)
			rigidbody.centerOfMass = m_centerOfMass.localPosition;
		
		// Turn us off so we dont do any updates etc.
		this.enabled = false;
	}
	
}
