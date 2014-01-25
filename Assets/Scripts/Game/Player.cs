using UnityEngine;
using System.Collections;
using InControl;

public class Player : MonoBehaviour {

	public float m_runSpeed = 5;

	// Use this for initialization
	void Start () {
		StartCoroutine( Wait() );
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 movement = new Vector3(InputManager.ActiveDevice.LeftStickX * m_runSpeed, 0, 0) * Clock.dt;
		transform.position += movement;
	}

	void OnTriggerEnter(Collider col)
	{
		col.gameObject.SendMessage("Collect", this);
	}

	IEnumerator Wait() {
		yield return new WaitForSeconds(1.0f);
		Animator playerAnim = this.GetComponent<Animator>();

		playerAnim.Play("run");
	} 
}
