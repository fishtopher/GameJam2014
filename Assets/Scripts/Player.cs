using UnityEngine;
using System.Collections;
using InControl;

public class Player : MonoBehaviour 
{
	public static Player Instance;

	private const float Y_POS_MAX = 0.64f;
	private const float Y_POS_MIN = -0.64f;
	private const float MOVE_DISTANCE = 0.64f;
	private const float MOVE_SPEED_VERT = 2.0f;
	
	private bool m_isMovingVert = false;
	public bool CanMoveVert {
		get { return m_isMovingVert; }
		set { m_isMovingVert = value; }
	}

	private bool m_isStunned = false;
	public bool IsStunned {
		get { return m_isStunned; }
		set { m_isStunned = value; }
	}

	private const float RUN_SPEED = 4;
	public float m_runSpeed;
	public float m_score = 0;
	public float Score { 
		get { return m_score; } 
		set 
		{
			m_score = value; 
			m_scoreText.text = string.Format("{0:000}", m_score); 
		}
	}
	public int m_numCollectedSteroids;
	public int NumCollectedSteroids { 
		get { return m_numCollectedSteroids; } 
		set 
		{
			m_numCollectedSteroids = value; 
		}
	}

	public GameObject m_sprite;
	public TextMesh m_scoreText;



	// Use this for initialization
	void Awake () 
	{
		Instance = this;
		m_runSpeed = 0;
		m_numCollectedSteroids = 0;
		StartCoroutine( Wait(0.5f) );
	}

	public void Reset()
	{
		m_score = 0;
		transform.position = Vector3.zero;
		Player.Instance.transform.localScale = Vector3.one; // Reset scale in case we were big when passing the previous level

		m_runSpeed = 0;
		m_numCollectedSteroids = 0;
		StartCoroutine( Wait(1.5f) );
	}

	// Update is called once per frame
	void Update () 
	{
		//Vector3 movement = new Vector3(InputManager.ActiveDevice.LeftStickX * m_runSpeed, 0, 0) * Clock.dt;
		Vector3 movement = Vector3.right * m_runSpeed * Clock.dt;
		transform.position += movement;

		if ( !m_isMovingVert && !m_isStunned ) {
			CheckControlPressed();
		}
	}

	private void CheckControlPressed()
	{
		float currentPos = this.transform.position.y;
		
		if ( Input.GetKeyDown( KeyCode.UpArrow ) && this.transform.position.y < Y_POS_MAX )
		{
			StartCoroutine( MoveUp( currentPos + MOVE_DISTANCE ) );
		}
		else if ( Input.GetKeyDown( KeyCode.DownArrow ) && this.transform.position.y > Y_POS_MIN )
		{
			StartCoroutine( MoveDown( currentPos - MOVE_DISTANCE ) );
		}
	}

	public void CheckControlDown()
	{
		// Continue movement if key is held down
		if ( Input.GetKey( KeyCode.UpArrow ) && this.transform.position.y < Y_POS_MAX )
		{
			StartCoroutine( MoveUp( this.transform.position.y + MOVE_DISTANCE ) );
		}
		else if ( Input.GetKey( KeyCode.DownArrow ) && this.transform.position.y > Y_POS_MIN )
		{
			StartCoroutine( MoveDown( this.transform.position.y - MOVE_DISTANCE ) );
		}
		else
		{
			m_isMovingVert = false;
		}
	}

	private IEnumerator MoveUp( float destination )
	{
		m_isMovingVert = true;

		while ( this.transform.position.y < destination )
		{
			this.transform.position += Vector3.up * MOVE_SPEED_VERT * Time.deltaTime;
			yield return 0;
		}
		
		// Continue movement if key is held down
		if ( !m_isStunned ) {
			CheckControlDown();
		}
	}
	
	private IEnumerator MoveDown( float destination )
	{
		m_isMovingVert = true;

		while ( this.transform.position.y > destination )
		{
			this.transform.position += Vector3.down * MOVE_SPEED_VERT * Time.deltaTime;
			yield return 0;
		}
		
		// Continue movement if key is held down
		if ( !m_isStunned ) {
			CheckControlDown();
		}
	}

	void OnTriggerEnter(Collider col)
	{
		col.gameObject.SendMessage("Collect", this);
	}

	private IEnumerator Wait( float duration ) {
		m_isMovingVert = true;
		yield return new WaitForSeconds(duration);
		m_isMovingVert = false;
		PlayAnimation("run");
		m_runSpeed = RUN_SPEED;
	}

	public IEnumerator StopSpeed() {
		m_isMovingVert = true;
		yield return new WaitForSeconds(0.2f);
		m_runSpeed = 0;
	}

	public void PlayAnimation(string animName)
	{
		Animator playerAnim = m_sprite.GetComponent<Animator>();
		playerAnim.Play(animName);
	}
}
