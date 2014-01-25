using UnityEngine;
using System.Collections;

public class Sleeper : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		if(rigidbody)
			rigidbody.Sleep();

		enabled = false;
	}
}
