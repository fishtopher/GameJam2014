using UnityEngine;
using System.Collections;

public class PlayerTransformMatcher : MonoBehaviour
{
	// Use this for initialization
	void Start () 
	{
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		transform.parent = go.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}
}
