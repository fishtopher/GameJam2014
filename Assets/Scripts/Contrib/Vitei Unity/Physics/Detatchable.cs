using UnityEngine;
using System.Collections;

public class Detatchable : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		transform.parent = Util.GetTempItemsParent();
	}
}

