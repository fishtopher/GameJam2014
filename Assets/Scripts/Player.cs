using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine( Wait() );
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Wait() {
		yield return new WaitForSeconds(1.0f);
		Animator playerAnim = this.GetComponent<Animator>();

		playerAnim.Play("run");
	}
}
