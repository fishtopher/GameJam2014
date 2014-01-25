using UnityEngine;
using System.Collections;

public class Placeholder : MonoBehaviour {

	public GameObject m_prefab;

	// Use this for initialization
	void Awake () 
	{
		GameObject ngo = Util.InstantiatePrefab(m_prefab, transform.position, transform.rotation);
		ngo.transform.parent = transform.parent;
		gameObject.SetActive(false);
	}
}
