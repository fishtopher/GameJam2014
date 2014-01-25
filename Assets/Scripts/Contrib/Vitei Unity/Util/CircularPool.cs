using UnityEngine;
using System.Collections;

// This gives you the next object in the list, looping round when it has to.  you will end up overwriting things no-doubt.
public class CircularPrefabPool
{
	GameObject m_prefab;
	int m_capacity;

	GameObject[] m_objects;
	int m_nextObject;

	public CircularPrefabPool(int capacity, GameObject prefab)
	{
		m_capacity = capacity;
		m_prefab = prefab;
		m_nextObject = -1;
		m_objects = new GameObject[m_capacity];
		for(int i = 0; i < m_capacity; i++)
		{
			m_objects[i] = Util.InstantiatePrefab(m_prefab, Vector3.zero);
			m_objects[i].SetActive(false);
		}
	}

	public GameObject GetNext()
	{
		m_nextObject++;
		if(m_nextObject >= m_capacity)
			m_nextObject = 0;

		return m_objects[m_nextObject];
	}
}
