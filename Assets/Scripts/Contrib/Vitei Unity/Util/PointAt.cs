using UnityEngine;
using System.Collections;

public class PointAt : MonoBehaviour {
	
	public Transform m_target;
	
	// Update is called once per frame
	void Update () 
	{	
		transform.localScale = Vector3.one;
		Quaternion l = Quaternion.LookRotation(transform.position - transform.position);
		Quaternion n = Quaternion.Euler(0, l.eulerAngles.y, 0);
		transform.rotation = n;
		
	}
}
