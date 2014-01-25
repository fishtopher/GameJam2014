using UnityEngine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour {
	
	SmoothFollowCam m_smoothFollowCam;
	SmoothLookatCam m_smoothLookatCam;
	enum Mode {Follow, Look, Overhead};
	Mode m_mode = Mode.Follow;
	
	Vector3 startPos;
	
	void Awake()
	{
		startPos = transform.position;
	}
	
	// Use this for initialization
	void Start () 
	{
		m_smoothFollowCam = GetComponentInChildren<SmoothFollowCam>();
		m_smoothLookatCam = GetComponentInChildren<SmoothLookatCam>();
		
		m_smoothFollowCam.enabled = (m_mode == Mode.Follow);
		m_smoothLookatCam.enabled = (m_mode == Mode.Look);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire3") )
		{
			m_mode++;
			if(m_mode > Mode.Overhead)
					m_mode = Mode.Follow;
			
			printf.PrintPersistentMessage(m_mode.ToString());
			
			m_smoothFollowCam.enabled = (m_mode == Mode.Follow);
			m_smoothLookatCam.enabled = (m_mode != Mode.Follow);
			
		}
		
		
			if(m_mode == Mode.Overhead)
				transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime);
	}
}
