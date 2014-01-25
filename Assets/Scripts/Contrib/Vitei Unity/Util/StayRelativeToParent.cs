using UnityEngine;
using System.Collections;

public class StayRelativeToParent : MonoBehaviour {

	Vector3 worldOffset;
	Quaternion rotation;
	
	// Use this for initialization
	void Start () {
		worldOffset = transform.position - transform.parent.position;
		rotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = transform.parent.position + worldOffset;
		transform.rotation = rotation;
	}
}
