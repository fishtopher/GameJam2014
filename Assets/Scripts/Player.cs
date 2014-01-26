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

	public ParticleSystem m_boostFX;
	public Hallucinate m_hallucinateFX;
	
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

	public TextMesh m_timeMesh;
	public TextMesh m_bestTimeMesh;
	float m_time;
	float m_bestTime = 999999;
	bool m_flashTime = false;

	bool m_running = false;


	// Use this for initialization
	void Awake () 
	{
		Instance = this;
		m_runSpeed = 0;
		//m_numCollectedSteroids = 0;
		//StartCoroutine( Wait(0.5f) );
	}

	public void Reset()
	{
		m_score = 0;
		transform.position = Vector3.zero;
		m_sprite.transform.localScale = Vector3.one; // Reset scale in case we were big when passing the previous level
		m_sprite.transform.localPosition = Vector3.zero;

		m_boostFX.Stop();
		m_hallucinateFX.Reset();
		
		m_time = 0;
		m_score = 0;
		m_flashTime = false;
		m_bestTimeMesh.gameObject.SetActive(true);
		m_timeMesh.text = "00:00.00";

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

		if(m_running)
			m_time += Clock.dt;
	
		m_timeMesh.text = FormatTime(m_time);
		
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
		m_running = true;

		SoundManager.PlaySound("song1");
	}

	public void StopRunning()
	{
		m_running = false;
	
		if(m_time < m_bestTime)
		{
			m_bestTime = m_time;
			m_bestTimeMesh.text = FormatTime(m_time);
		}

		StartCoroutine( StopSpeed() );
	}

	public static string FormatTime(float timeInsecs)
	{
		string timeString = "";
		float t = timeInsecs;
		float mins = (int)(t / 60.0f);
		timeString = string.Format("{0:00}:", mins);
		float secs = t - (mins * 60);
		timeString += string.Format("{0:00.00}", secs);
		
		return timeString;
	}

	private IEnumerator FlashTime()
	{
		while(m_flashTime)
		{
			m_bestTimeMesh.gameObject.SetActive(false);
			yield return new WaitForSeconds(0.3f);
			m_bestTimeMesh.gameObject.SetActive(true);
			yield return new WaitForSeconds(0.5f);
		}

		m_bestTimeMesh.gameObject.SetActive(true);
	}

	public void StartBoost(float amt)
	{
		m_runSpeed *= amt;
		m_boostFX.Play();
	}
	public void StopBoost(float amt)
	{
		m_runSpeed /= amt;
		m_boostFX.Stop();
	}

	public void Hallucinate()
	{
		m_hallucinateFX.StepUp();
	}


	IEnumerator StopSpeed() {
		m_isMovingVert = true;
		yield return new WaitForSeconds(0.2f);
		m_runSpeed = 0;
	}

	public void PlayAnimation(string animName)
	{
		Animator playerAnim = m_sprite.GetComponent<Animator>();
		playerAnim.Play(animName);
	}

	public void ScaleSprite(float amt)
	{
		m_sprite.transform.localScale = Vector3.one * amt;
		m_sprite.transform.localPosition = new Vector3(0, amt / 8.0f, 0);

	}
}
