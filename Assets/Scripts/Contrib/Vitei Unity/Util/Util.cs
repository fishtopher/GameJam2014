using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Util
{
	static Transform ms_tempObjects;

	public static Transform GetTempItemsParent()
	{
		if(ms_tempObjects == null)
			ms_tempObjects = new GameObject("_tempObjects").transform;
		return ms_tempObjects;
	}

	public static GameObject MakePrimitive(PrimitiveType pt)
	{
		GameObject ngo = GameObject.CreatePrimitive(pt);
		ngo.transform.parent = GetTempItemsParent();
		return ngo;
	}
	
	public static GameObject InstantiatePrefab(GameObject prefab, Vector3 pos)
	{
		GameObject ngo = GameObject.Instantiate(prefab, pos, Quaternion.identity) as GameObject;
		ngo.transform.parent = GetTempItemsParent();
		return ngo;
	}
	
	public static GameObject InstantiatePrefab(GameObject prefab, Vector3 pos, Quaternion rot)
	{
		GameObject ngo = GameObject.Instantiate(prefab, pos, rot) as GameObject;
		ngo.transform.parent = GetTempItemsParent();
		return ngo;
	}


	public static bool ShouldIgnore(GameObject go, ICollection<GameObject> ignoreList)
	{
		bool shouldIgnore = false;
		foreach(GameObject i in ignoreList)
		{
			if(go == i)
			{
				shouldIgnore = true;
				break;
			}
		}
		return shouldIgnore;
	}
	
	public static GameObject GetClosest(Vector3 pos, ICollection<GameObject> objectList, ICollection<GameObject> ignoreList = null)
	{
		GameObject closest = null;
		float closestDistSq = float.MaxValue;
		foreach(GameObject go in objectList)
		{
			if(go == null)
				continue;
			
			bool shouldIgnore = (ignoreList==null) ? false : ShouldIgnore(go, ignoreList);
			if(shouldIgnore)
				continue;
			
			Vector3 v = go.transform.position - pos;
			float d = v.sqrMagnitude;
			if(d < closestDistSq)
			{
				closestDistSq = d;
				closest = go;
			}
		}
		
		return closest;
	}

	public static Transform GetClosest(Vector3 pos, ICollection<Transform> objectList)
	{
		Transform closest = null;
		float closestDistSq = float.MaxValue;
		foreach(Transform go in objectList)
		{
			if(go == null)
				continue;
			
			Vector3 v = go.position - pos;
			float d = v.sqrMagnitude;
			if(d < closestDistSq)
			{
				closestDistSq = d;
				closest = go;
			}
		}
		
		return closest;
	}
	
	public static bool WithinRadius(Vector3 A, Vector3  B, float r)
	{
		return ( (A - B).sqrMagnitude < (r * r) );
	}
	
	public static bool WithinRadius(Component A, Component B, float r)
	{
		return ( (A.transform.position - B.transform.position ).sqrMagnitude < (r * r) );
	}
	
	public static bool WithinRadius(GameObject A, GameObject B, float r)
	{
		return ( (A.transform.position - B.transform.position ).sqrMagnitude < (r * r) );
	}
	
	public static void DebugDrawCircle(Vector3 p, float r, Color col, int res = 16)
    {
		float step = (Mathf.PI*2) / (float)res;
		
		Vector3 p1 = p + new Vector3(Mathf.Sin(0) * r, 0, Mathf.Cos(0) * r);
		for(int i = 0; i < res; i++)
		{	
			float ang = step * (i+1);
			Vector3 p2 = p + new Vector3(Mathf.Sin(ang) * r, 0, Mathf.Cos(ang) * r);
			Debug.DrawLine(p1, p2, col);
			p1 = p2;
    	}
	}
	
	// Knuth shuffle
	public static void Shuffle(ref int[] array)
    {
        for (int i = 0; i < array.Length; i++ )
        {
            int tmp = array[i];
            int j = Random.Range(i, array.Length);
            array[i] = array[j];
            array[j] = tmp;
        }
    }
	
	public static int[] RandomSequenceOfInts(int minInclusive, int maxExclusive)
	{
		int num = maxExclusive - minInclusive;
		int[] array = new int[num];
		for(int i = 0; i < num; i++)
			array[i] = i;
		Shuffle (ref array);
		return array;
	}
}
