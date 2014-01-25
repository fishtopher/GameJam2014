using UnityEngine;
using System.Collections;

public class Debug2D : MonoBehaviour 
{
	public static void Line(float x1, float y1, float x2, float y2)
	{
		Line(x1, y1, x2, y2, Color.green);
	}
	public static void Line(float x1, float y1, float x2, float y2, Color col)
	{
		Vector3 p1 = Camera.main.ScreenToWorldPoint( new Vector3(x1, y1, 5) );
		Vector3 p2 = Camera.main.ScreenToWorldPoint( new Vector3(x2, y2, 5) );

		Debug.DrawLine(p1, p2, col);
	}

	
	public static void Crosshair(float x1, float y1, float scale, Color col)
	{
		Line(x1-scale, y1, x1+scale, y1, col);
		Line(x1, y1-scale, x1, y1+scale, col);
	}
}
