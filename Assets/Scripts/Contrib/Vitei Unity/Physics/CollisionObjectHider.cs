using UnityEngine;
using System.Collections;

public class CollisionObjectHider : MonoBehaviour 
{
	Renderer[] m_collisionRenderers;
	public bool m_hideAllOnStartup = true;

	// Use this for initialization
	void Start () 
	{
		if(!m_hideAllOnStartup)
			return;

		m_collisionRenderers = GetComponentsInChildren<Renderer>();
		for(int i = 0; i < m_collisionRenderers.Length; i++)
		{
			m_collisionRenderers[i].enabled = false;
		}

		this.enabled = false;
	}
}
