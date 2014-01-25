using UnityEngine;
using System.Collections;

public class ObjectSensor : MonoBehaviour 
{
	public GameObject m_target;
	public void SetTarget(GameObject value) 
	{
		if(m_target!= value)
		{
			m_target = value;
			On = false;
		}
	}

	public bool On { get; private set; }
	
	public Color m_onColour = new Color(0, 1, 0, 0.333f);
	public Color m_offColour = new Color(1, 0, 0, 0.333f);
	public Renderer m_visualiser;
	
	public delegate void TriggerDelegate(Collider col);
	[HideInInspector]
	public TriggerDelegate m_onAnyTriggerEnter;
	public TriggerDelegate m_onAnyTriggerExit;
	public TriggerDelegate m_onTargetTriggerEnter;
	public TriggerDelegate m_onTargetTriggerExit;
	
	void Start()
	{
		if(m_visualiser == null)
			m_visualiser = renderer;
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject == m_target)
		{ 
			On = true;
			
			if(m_visualiser)
				m_visualiser.material.color = m_onColour;
		
			if(m_onTargetTriggerEnter != null)
				m_onTargetTriggerEnter(col);
		}
		
		if(m_onAnyTriggerEnter != null)
			m_onAnyTriggerEnter(col);
	}
	
	void OnTriggerExit(Collider col)
	{
		if(col.gameObject == m_target)
		{ 
			On = false;
			
			if(m_visualiser)
				m_visualiser.material.color = m_offColour;
		
			if(m_onTargetTriggerExit != null)
				m_onTargetTriggerExit(col);
		}
		
		if(m_onAnyTriggerExit != null)
			m_onAnyTriggerExit(col);
	}
}
