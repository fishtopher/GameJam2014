using UnityEngine;
using System.Collections;

public class Resettable : MonoBehaviour 
{
	delegate void ResettableDelegate();
	static ResettableDelegate ms_resetToStartDelegates = null;
	static ResettableDelegate ms_resetToCheckpointDelegates = null;
	static ResettableDelegate ms_saveCheckpointDelegates = null;
	public static void ResetToStart() { if(ms_resetToStartDelegates != null) { ms_resetToStartDelegates(); } }
	public static void ResetToCheckpoint() { if(ms_resetToCheckpointDelegates != null) { ms_resetToCheckpointDelegates(); } }
	public static void SaveCheckpoint() { if(ms_saveCheckpointDelegates != null) { ms_saveCheckpointDelegates(); } }

	//
	Vector3 m_startPosition;
	Quaternion m_startRotation;

	Vector3 m_checkpointPosition;
	Quaternion m_checkpointRotation;

	Vector3 m_checkpointVelocity;
	Vector3 m_checkpointAngularVelocity;

	// Use this for initialization
	void Awake () 
	{
		m_startPosition = transform.position;
		m_startRotation = transform.rotation;
		m_checkpointPosition = transform.position;
		m_checkpointRotation = transform.rotation;

		ms_resetToStartDelegates += DoResetToStart;
		ms_resetToCheckpointDelegates += DoResetToCheckpoint;
		ms_saveCheckpointDelegates += DoSaveCheckpoint; 
	}

	void OnDestroy()
	{
		ms_resetToStartDelegates -= DoResetToStart;
		ms_resetToCheckpointDelegates -= DoResetToCheckpoint;
		ms_saveCheckpointDelegates -= DoSaveCheckpoint; 
	}
	
	void DoResetToStart()
	{
		transform.position = m_startPosition;
		transform.rotation = m_startRotation;
		
		if(rigidbody != null)
		{
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
		}
	}

	void DoResetToCheckpoint()
	{
		transform.position = m_checkpointPosition;
		transform.rotation = m_checkpointRotation;
		
		if(rigidbody != null)
		{
			rigidbody.velocity = m_checkpointVelocity;
			rigidbody.angularVelocity = m_checkpointAngularVelocity;
		}
	}

	void DoSaveCheckpoint()
	{
		m_checkpointPosition = transform.position;
		m_checkpointRotation = transform.rotation;

		if(rigidbody != null)
		{
			m_checkpointVelocity = rigidbody.velocity;
			m_checkpointAngularVelocity = rigidbody.angularVelocity;
		}
	}

}
