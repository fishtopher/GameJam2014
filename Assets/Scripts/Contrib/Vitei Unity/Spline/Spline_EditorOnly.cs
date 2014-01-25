using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

public partial class Spline : MonoBehaviour 
{
	public enum DrawMode { WhenSelected, Always, Never }
	public DrawMode m_drawDebug = DrawMode.WhenSelected;
	static Material ms_debugMat;
	public float m_debugRenderResolution = 5;
	public Color m_debugColour = Color.cyan;
	public Color m_debugColourUnselected = new Color(0,0.475f,1,0.5f);
	
	void PostInsertPoint(GameObject insertedPoint)
	{
		Selection.activeGameObject = insertedPoint;
	}

	// -------------------------------------
	// Drawing
	//
	public void DebugDrawLineList()
	{
		Color c = new Color(1, 1, 1, 1.1f);
		if(m_points.Count > 1)
		{
			for(int i = 0; i < m_points.Count-1; i++)
			{
				Debug.DrawLine(m_points[i].transform.position, m_points[i+1].transform.position, c);
			}
		}
	}
	
	public void DebugDrawHermite(Color col)
	{
		float t = 0;
		float step = 1.0f / ((m_points.Count-1) * m_debugRenderResolution);
		Vector3 p1 = PointAtNormalisedTime(t);
		while(t < 1)
		{
			t += step;
			Vector3 p2 = PointAtNormalisedTime(t);
			Debug.DrawLine(p1, p2, col);
			p1 = p2;
		}
		
		//		Debug.DrawLine(m_points[0].transform.position, m_pointBeforeStart, Color.blue);
		//		Debug.DrawLine(m_points[m_points.Count-1].transform.position, m_pointAfterEnd, Color.blue);
	}

	//
	void OnDrawGizmos()
	{
		if(m_drawDebug == DrawMode.Never)
			return;

		bool isSelected = false;
		if(Selection.Contains(gameObject))
		{
			isSelected = true;
		}
		else
		{
			for(int i = 0; i < m_points.Count; i++)
			{
				if(Selection.Contains(m_points[i].gameObject))
				{
					isSelected = true;
					break;
				}
			}
		}
		
		if(isSelected)
		{
			DebugDrawHermite(m_debugColour);
		}
		else if(m_drawDebug == DrawMode.Always)
		{
			DebugDrawHermite(m_debugColourUnselected);
		}
	}
}

#else

public partial class SplinePlopper : MonoBehaviour 
{
	
	void PostInsertPoint(GameObject insertedPoint) { }
}


#endif
