using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BaseRandomPair
{
	public Vector3 m_base = default(Vector3);
	Vector3 m_baseLast = default(Vector3);
	public Vector3 m_random = default(Vector3);
	Vector3 m_randomLast = default(Vector3);
	
	public bool Dirty { get { return !m_base.Equals(m_baseLast) || !m_random.Equals(m_randomLast); } }
	
	public BaseRandomPair() {}
	public BaseRandomPair(Vector3 initialBase, Vector3 initialRandom) 
	{
		m_base = m_baseLast = initialBase;  
		m_random = m_randomLast = initialRandom;
	}
	
	public void RefreshLasts()
	{
		m_baseLast = m_base;
		m_randomLast = m_random;
	}
}

// Annoying: Unity won't show templated serializable classes in the inspector, so having to manually make different versions for each type:/ 
[System.Serializable]
public class BaseRandomPairFloat
{
	public float m_base = default(float);
	float m_baseLast = default(float);
	public float m_random = default(float);
	float m_randomLast = default(float);
	
	public bool Dirty { get { return !m_base.Equals(m_baseLast) || !m_random.Equals(m_randomLast); } }
	
	public BaseRandomPairFloat() {}
	public BaseRandomPairFloat(float initialBase, float initialRandom) 
	{
		m_base = m_baseLast = initialBase;  
		m_random = m_randomLast = initialRandom;
	}
	
	public void RefreshLasts()
	{
		m_baseLast = m_base;
		m_randomLast = m_random;
	}
}

[ExecuteInEditMode]
[RequireComponent(typeof(Spline))]
public partial class SplinePlopper : MonoBehaviour
{
	public GameObject[] m_prefab;
	GameObject[] m_lastPrefab;
	int m_availablePrefabs = 0;
	public float m_spacing = 10;
	float m_lastSpacing = 10;

	public BaseRandomPair m_position = new BaseRandomPair();
	public BaseRandomPair m_rotation = new BaseRandomPair();
	public BaseRandomPairFloat m_scale = new BaseRandomPairFloat(1, 0);

	Spline m_spline;
	List<GameObject> m_objects = new List<GameObject>();
	GameObject m_objectContainer;

	bool m_dirty = false;

	void Start()
	{
		SanityCheck();
		UpdateLastPrefabs();
	}

	bool HavePrefabsChanged()
	{
		if(m_prefab == null)
			return true;

		if(m_prefab != null && m_lastPrefab == null)
			return true;

		if(m_prefab.Length != m_lastPrefab.Length)
			return true;

		for(int i = 0; i < m_prefab.Length; i++)
		{
			if(m_prefab[i] != m_lastPrefab[i])
				return true;
		}

		return false;
	}

	void UpdateLastPrefabs()
	{
		if(m_prefab == null)
			return;

		m_availablePrefabs = 0;
		m_lastPrefab = new GameObject[m_prefab.Length];
		for(int i = 0; i < m_prefab.Length; i++)
		{
			m_lastPrefab[i] = m_prefab[i];
			if(m_prefab[i] != null)
				m_availablePrefabs++;
		}
	}

	// Late update so that we can react to changes in the spline
	void LateUpdate () 
	{
		m_dirty = false;

		m_spline = GetComponent<Spline>();
		if(m_spline == null)
			m_spline = gameObject.AddComponent(typeof(Spline)) as Spline;
		
		if(m_spline == null)
			return;

		if(HavePrefabsChanged())
		{
			ClearObjects();
			UpdateLastPrefabs();
			m_dirty = true;
		}

		if(m_dirty ||
		   m_spline.IsDirty ||
		   m_spacing != m_lastSpacing || 
		   m_position.Dirty ||
		   m_rotation.Dirty ||
		   m_scale.Dirty
		   )
		{
			UpdateObjects();
		}

		m_lastSpacing = m_spacing;
	}

	void UpdateObjects()
	{
		if( Mathf.Abs(m_spacing) > 0.1 )
		{
			int canFit = (int)(m_spline.Length / m_spacing)+1;
			if(m_availablePrefabs > 0)
			{
				while(canFit > m_objects.Count)
					AddPrefab ();
			}
			while(canFit < m_objects.Count)
				RemovePrefab();
			
			AlignObjects();
			
			m_position.RefreshLasts();
			m_rotation.RefreshLasts();
			m_scale.RefreshLasts();
		}
	}

	void AlignObjects()
	{
		if(m_spline == null)
			return;

		for(int i = 0; i < m_objects.Count; i++)
		{
			GameObject obj = m_objects[i];
			Vector3 p, t;
			m_spline.PointAndTangentAtTime(i * m_spacing, out p, out t);
			obj.transform.rotation = Quaternion.LookRotation(t, Vector3.up);
			obj.transform.position = p;
			obj.transform.Translate(m_position.m_base + m_position.m_random.Randomized());
			obj.transform.rotation = Quaternion.LookRotation(t, Vector3.up) * Quaternion.Euler(m_rotation.m_base);
			obj.transform.localRotation *= Quaternion.Euler( m_rotation.m_random.Randomized() );
			obj.transform.localScale = Vector3.one * (m_scale.m_base + Random.Range(0, m_scale.m_random));
		}
	}

	public void Randomize()
	{
		ClearObjects();
		UpdateObjects();
	}

	void ClearObjects()
	{
		m_objects.Clear();
		GameObject.DestroyImmediate(m_objectContainer);
		m_objectContainer = null;
	}

	void SanityCheck()
	{
		// Make sure we have somewhere to keep all the objects tidy
		if(m_objectContainer == null)
		{
			Transform t = transform.Find("PloppedObjects");
			if(t)
			{
				m_objectContainer = t.gameObject;

				if(m_objects.Count == 0)
				{
					for(int i = 0; i < m_objectContainer.transform.childCount; i++)
					{
						m_objects.Add( m_objectContainer.transform.GetChild(i).gameObject );
					}
				}
					    
			}

			if(m_objectContainer == null)
			{
				m_objects.Clear ();
				m_objectContainer = new GameObject("PloppedObjects");
				m_objectContainer.transform.parent = transform;
				m_objectContainer.transform.localPosition = Vector3.zero;
			}
		} 
	}



	// Removes the last rprefab
	void RemovePrefab()
	{
		if(m_objects.Count == 0)
			return;

		int last = m_objects.Count-1;
		GameObject obj = m_objects[last];
		m_objects.RemoveAt(last);
		GameObject.DestroyImmediate(obj);
	}
}
