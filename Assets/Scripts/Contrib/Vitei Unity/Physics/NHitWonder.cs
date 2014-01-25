using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// After N collisions, this object will switch to another layer.
// e.g. after the car hits a box once, move it to a no-car-collision 
//	layer to stop the car bouncing over it as the wheel physics are 
//	a bit sketchy.
public class NHitWonder : MonoBehaviour 
{
	public bool m_sleepAtStart = false;
	public int m_hitsBeforeSwitch = 1;
	public LayerMask m_switchToLayerOnHit;
	int m_layerId;
	Rigidbody m_rigidBody;


	// Use this for initialization
	void Start ()
	{
		m_rigidBody = rigidbody;
		
		int layer = m_switchToLayerOnHit.value;
		while(layer > 0)
		{
		    layer = layer >> 1;
		    m_layerId++;
		}
		
		if(m_sleepAtStart)
		{
			m_rigidBody.Sleep();
		}
	}
	
	void OnCollisionEnter(Collision col)
	{
		if(m_hitsBeforeSwitch > 0)
		{
			m_hitsBeforeSwitch--;
			if(m_hitsBeforeSwitch == 0)
			{
				gameObject.layer = m_layerId;
			}
		}
	}
}
