using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SplinePoint : MonoBehaviour
{
	[HideInInspector]
	public bool removeFromSpline = true;

	void OnDestroy()
	{
	  if (removeFromSpline && (Application.isPlaying==false)&&(Application.isEditor==true)&&(Application.isLoadingLevel==false))
      {
			if(m_mySpline)
			{
				m_mySpline.RemovePoint(this);
			}
		}
    }
	
	public bool IsDirty { get { return m_dirty; } }
	bool m_dirty;
	Vector3 m_lastPos;
	Quaternion m_lastRotation;
	Spline m_mySpline;

	// Use this for initialization
	void Start () 
	{
		m_lastPos = transform.position;
		m_lastRotation = transform.rotation;
		m_mySpline = transform.parent.GetComponent<Spline>();
	}

#if UNITY_EDITOR

	// Update is called once per frame
	void Update () 
	{
		//!
		// I'd really like if this entire function wasn't called in game, but that's not really possible:/
		//	#if UNITY_EDITOR       :stops it getting into builds.
		//  if(Debug.isDebugBuild) :stops it running when game is ran from editor
		if(Debug.isDebugBuild)
			return;

		m_dirty |= (
			m_lastPos != transform.position || 
			m_lastRotation != transform.rotation);

		if(m_dirty)
		{
			m_mySpline = transform.parent.GetComponent<Spline>();
			m_mySpline.SanityCheck();
			m_lastPos = transform.position;
			m_lastRotation = transform.rotation;
		}
	}

	void OnPostRender()
	{
		m_dirty = false;
	}

	public void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "SplineIcon.png", false);
    }
#endif
}
