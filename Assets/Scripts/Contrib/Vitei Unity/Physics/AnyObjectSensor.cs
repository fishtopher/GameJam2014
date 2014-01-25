using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// A generic sensor, it'll tell you if it's colliding with any objects at all.
// You can set it to inform you when particular objects enter/exit 
public class AnyObjectSensor : MonoBehaviour 
{
	public delegate void TriggerDelegate(Collider col);
	public class ObjectDelegateSet
	{
		public GameObject m_object;
		public TriggerDelegate m_onEnterDelegate;
		public TriggerDelegate m_onExitDelegate;
		public ObjectDelegateSet(GameObject go, TriggerDelegate onEnterDelegate, TriggerDelegate onExitDelegate)
		{
			m_object = go;
			m_onEnterDelegate = onEnterDelegate;
			m_onExitDelegate = onExitDelegate;
		}
	}

	List<ObjectDelegateSet> m_watchList;
	public List<GameObject> m_objs { get; private set; }
	public List<GameObject> m_collisionObjs { get; private set; }

	public bool On { get { return m_objs.Count > 0; } }

	//
	public AnyObjectSensor()
	{
		m_watchList = new List<ObjectDelegateSet>();
		m_objs = new List<GameObject>();
		m_collisionObjs = new List<GameObject>();
	}

	//
	public void WatchFor(GameObject go, TriggerDelegate onEnterDelegate, TriggerDelegate onExitDelegate)
	{
		m_watchList.Add(new ObjectDelegateSet(go, onEnterDelegate, onExitDelegate));
	}

	//
	public void Reset () 
	{
		m_objs.Clear();
	}
	
	//
	void OnTriggerEnter(Collider col)
	{
		if(col.isTrigger)
			return;
		
		m_objs.Add(col.gameObject);
		
		// Callback to anything that's listenting for specific objects
		for(int i = 0; i < m_watchList.Count; i++)
		{
			if(col.gameObject == m_watchList[i].m_object && m_watchList[i].m_onEnterDelegate != null)
			{
				m_watchList[i].m_onEnterDelegate(col);
			}
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		if(col.isTrigger)
			return;
		
		m_objs.Remove(col.gameObject);
		
		// Callback to anything that's listenting for specific objects
		for(int i = 0; i < m_watchList.Count; i++)
		{
			if(col.gameObject == m_watchList[i].m_object && m_watchList[i].m_onExitDelegate != null)
			{
				m_watchList[i].m_onExitDelegate(col);
			}
		}
	}


	
	//
	void OnCollisionEnter(Collision col)
	{
		m_collisionObjs.Add(col.collider.gameObject);
	}
	
	void OnCollisionExit(Collision col)
	{
		m_collisionObjs.Remove(col.collider.gameObject);
	}
}
