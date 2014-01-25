using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private const float Y_POS_MAX = 0.5f;
	private const float Y_POS_MIN = -0.5f;
	private const float MOVE_DISTANCE = 0.5f;
	private const float MOVE_SPEED = 2.0f;

	private bool isMoving = false;

	// Use this for initialization
	void Start () {
		StartCoroutine( Wait() );

		//movePosition = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if ( !isMoving ) {
			CheckControls();
		}
	}

	private void CheckControls()
	{
		float currentPos = this.transform.position.y;

		if ( Input.GetKeyDown( KeyCode.UpArrow ) && this.transform.position.y < Y_POS_MAX )
		{
			isMoving = true;
			StartCoroutine( MoveUp( currentPos + MOVE_DISTANCE ) );
		}
		else if ( Input.GetKeyDown( KeyCode.DownArrow ) && this.transform.position.y > Y_POS_MIN )
		{
			isMoving = true;
			StartCoroutine( MoveDown( currentPos - MOVE_DISTANCE ) );
		}
	}

	private IEnumerator MoveUp( float destination )
	{
		while ( this.transform.position.y < destination )
		{
			this.transform.position += Vector3.up * MOVE_SPEED * Time.deltaTime;
			yield return 0;
		}

		isMoving = false;

		// Continue movement if key is held down
		if ( Input.GetKey( KeyCode.UpArrow ) && this.transform.position.y < Y_POS_MAX )
		{
			StartCoroutine( MoveUp( this.transform.position.y + MOVE_DISTANCE ) );
		}
		else
		{
			isMoving = false;
		}
	}

	private IEnumerator MoveDown( float destination )
	{
		while ( this.transform.position.y > destination )
		{
			this.transform.position += Vector3.down * MOVE_SPEED * Time.deltaTime;
			yield return 0;
		}

		// Continue movement if key is held down
		if ( Input.GetKey( KeyCode.DownArrow ) && this.transform.position.y > Y_POS_MIN )
		{
			StartCoroutine( MoveDown( this.transform.position.y - MOVE_DISTANCE ) );
		}
		else
		{
			isMoving = false;
		}
	}

	// FOR TESTING
	private IEnumerator Wait()
	{
		yield return new WaitForSeconds(1.0f);
		Animator playerAnim = this.GetComponent<Animator>();

		playerAnim.Play("run");
	}
}
