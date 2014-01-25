using UnityEngine;
//using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Spline))]
public class SplineMirroree : MonoBehaviour
{
	public Spline m_sourceSpline;
	public float m_offset = 10;
	public float m_offsetY = 0;

	Spline m_mySpline;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(m_sourceSpline == null)
			return;

		m_mySpline = GetComponent<Spline>();
		if(m_mySpline == null)
		{
			m_mySpline = gameObject.AddComponent(typeof(Spline)) as Spline;
		}
		if(m_mySpline == null)
			return;

		while(m_mySpline.m_numRealPoints < m_sourceSpline.m_numRealPoints)
			m_mySpline.InsertPointAfter(m_mySpline.m_numRealPoints-1);
		while(m_mySpline.m_numRealPoints > m_sourceSpline.m_numRealPoints)
			m_mySpline.RemovePoint(m_mySpline.m_points[m_mySpline.m_numRealPoints-1]);

		for(int i = 0; i < m_mySpline.m_points.Count; i++)
		{
			Transform sourcePtTr = m_sourceSpline.m_points[i].transform;
			m_mySpline.m_points[i].transform.position = sourcePtTr.position
				+ sourcePtTr.right * m_offset * sourcePtTr.localScale.x
				+ sourcePtTr.up * m_offsetY * sourcePtTr.localScale.y;
		}

		m_mySpline.m_closed = m_sourceSpline.m_closed;


		m_mySpline.SanityCheck();
	}
}
