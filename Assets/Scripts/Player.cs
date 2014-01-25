using UnityEngine;
using System.Collections;
using InControl;

public class Player : MonoBehaviour {

	private const float Y_POS_MAX = 0.5f;
	private const float Y_POS_MIN = -0.5f;
	private const float MOVE_DISTANCE = 0.5f;
	private const float MOVE_SPEED_VERT = 2.0f;
	
	private bool isMovingVert = false;

	public float m_runSpeed = 5;
	public float m_lansSwitchSpeed = 5;

	// Use this for initialization
	void Start () {
		StartCoroutine( Wait() );
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 movement = new Vector3(InputManager.ActiveDevice.LeftStickX * m_runSpeed, 0, 0) * Clock.dt;
		transform.position += movement;

		if ( !isMovingVert ) {
			CheckControls();
		}
	}

	private void CheckControls()
	{
		float currentPos = this.transform.position.y;
		
		if ( Input.GetKeyDown( KeyCode.UpArrow ) && this.transform.position.y < Y_POS_MAX )
		{
			isMovingVert = true;
			StartCoroutine( MoveUp( currentPos + MOVE_DISTANCE ) );
		}
		else if ( Input.GetKeyDown( KeyCode.DownArrow ) && this.transform.position.y > Y_POS_MIN )
		{
			isMovingVert = true;
			StartCoroutine( MoveDown( currentPos - MOVE_DISTANCE ) );
		}
	}

	private IEnumerator MoveUp( float destination )
	{
		while ( this.transform.position.y < destination )
		{
			this.transform.position += Vector3.up * MOVE_SPEED_VERT * Time.deltaTime;
			yield return 0;
		}
		
		isMovingVert = false;
		
		// Continue movement if key is held down
		if ( Input.GetKey( KeyCode.UpArrow ) && this.transform.position.y < Y_POS_MAX )
		{
			StartCoroutine( MoveUp( this.transform.position.y + MOVE_DISTANCE ) );
		}
		else
		{
			isMovingVert = false;
		}
	}
	
	private IEnumerator MoveDown( float destination )
	{
		while ( this.transform.position.y > destination )
		{
			this.transform.position += Vector3.down * MOVE_SPEED_VERT * Time.deltaTime;
			yield return 0;
		}
		
		// Continue movement if key is held down
		if ( Input.GetKey( KeyCode.DownArrow ) && this.transform.position.y > Y_POS_MIN )
		{
			StartCoroutine( MoveDown( this.transform.position.y - MOVE_DISTANCE ) );
		}
		else
		{
			isMovingVert = false;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		col.gameObject.SendMessage("Collect");
	}

	// FOR TESTING
	private IEnumerator Wait() {
		yield return new WaitForSeconds(1.0f);
		Animator playerAnim = this.GetComponent<Animator>();

		playerAnim.Play("run");
	}
}
