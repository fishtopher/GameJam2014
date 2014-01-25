using UnityEngine;
using System.Collections;

public class GroundAlignment : MonoBehaviour 
{
	public bool m_stickToGround = true;
	
	public enum Alignment {MatchGround, Flat, None};
	public Alignment m_alignToGround = Alignment.MatchGround;
	public float m_rayLength = 20;
	Vector3 m_lastPosition;
	
	void Start()
	{
		m_lastPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{
		StickToGround();
	}
	
	// Make sure we stay on the ground when we go up/down slopes
	void StickToGround()
	{
		if(transform.position != m_lastPosition)
		{
			RaycastHit hitInfo;
			if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo, m_rayLength, 1 << LayerMask.NameToLayer("Ground")))
			{
				transform.position = hitInfo.point;
			}
			
			if(m_alignToGround != Alignment.None)
				AlignToGround(hitInfo);
		}
		m_lastPosition = transform.position;
	}
	
	void AlignToGround(RaycastHit hitInfo)
	{
		if(m_alignToGround == Alignment.Flat)
		{
			Vector3 f = Vector3.Cross(transform.right, Vector3.up);
			transform.rotation = Quaternion.LookRotation(f, Vector3.up);
		}
		else if(m_alignToGround == Alignment.MatchGround)
		{
			if(hitInfo.normal.sqrMagnitude != 0)
			{
				Vector3 f = Vector3.Cross(transform.right, hitInfo.normal);
				transform.rotation = Quaternion.LookRotation(f, hitInfo.normal);
			}
		}
	}
	
}
