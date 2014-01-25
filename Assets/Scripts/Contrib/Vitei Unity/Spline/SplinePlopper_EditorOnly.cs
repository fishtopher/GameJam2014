using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

public partial class SplinePlopper : MonoBehaviour 
{
	// Adds a prefab to the end of the list
	void AddPrefab()
	{
		if(m_availablePrefabs <= 0)
			return;
		
		SanityCheck();

		GameObject p = m_prefab[ Random.Range(0, m_prefab.Length) ];
		if(p)
		{
			GameObject ngo = PrefabUtility.InstantiatePrefab( p ) as GameObject;
			ngo.transform.parent = m_objectContainer.transform;
			ngo.name = ngo.name + " " + m_objects.Count;
			m_objects.Add(ngo);
		}
	}
}

#else

public partial class SplinePlopper : MonoBehaviour 
{
	// Adds a prefab to the end of the list
	void AddPrefab()
	{
		
	}
}


#endif