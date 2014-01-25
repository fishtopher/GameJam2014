using UnityEngine;
using System.Collections;

public class SplineFollower : MonoBehaviour {

	public Spline m_spline;
	public float m_speed = 1;

	public enum LoopType { Clamp, Loop, PingPong, FunctionCall };
	public LoopType m_loop = LoopType.Loop;

	public Vector3 m_offset = Vector3.zero;

	const int Forwards = 1;
	const int Backwards= -1;
	const int Stopped = 0;
	public int m_direction = Forwards;

	public float m_rotationLerpRate = 10;

	public float m_startPosNormalised = 0;
	float m_distAlongSpline = 0.0f;

	// Use this for initialization
	void Start () 
	{
		m_distAlongSpline = m_startPosNormalised*m_spline.Length;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m_direction == Stopped)
			return;

		m_distAlongSpline += m_speed * m_direction * Clock.dt;

		if(m_distAlongSpline > m_spline.Length)
		{
			switch(m_loop)
			{
			case LoopType.Loop:
				while(m_distAlongSpline > m_spline.Length)
					m_distAlongSpline -= m_spline.Length;
				break;
			case LoopType.Clamp:
				m_distAlongSpline = m_spline.Length;
				m_direction = Stopped;
				break;
			case LoopType.PingPong:
				float d = (m_distAlongSpline - m_spline.Length);
				m_distAlongSpline = m_spline.Length - d;
				m_direction *= -1;
					break;
			}
		}
		else if(m_distAlongSpline < 0)
		{
			switch(m_loop)
			{
			case LoopType.Loop:
				while(m_distAlongSpline < 0)
					m_distAlongSpline += m_spline.Length;
				break;
			case LoopType.Clamp:
				m_distAlongSpline = 0;
				m_direction = Stopped;
				break;
			case LoopType.PingPong:
				m_distAlongSpline *= -1;
				m_direction *= -1;
					break;
			}
		}


		Vector3 p;
		Vector3 t;
		m_spline.PointAndTangentAtTime(m_distAlongSpline, out p, out t);
		if(m_direction == Backwards)
			t *= -1;

		Vector3 right = Vector3.Cross(t, Vector3.up);
		Vector3 offset = right * m_offset.x + Vector3.up * m_offset.y + t * m_offset.z;

		transform.position = Vector3.Lerp(transform.position,  p + offset, m_rotationLerpRate * Clock.dt);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(t), m_rotationLerpRate * Clock.dt);
	}
}
