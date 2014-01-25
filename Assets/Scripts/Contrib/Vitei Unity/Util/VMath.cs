using UnityEngine;
using System.Collections;

public class VMath 
{
	[System.Serializable]
	public class FloatRange
	{
		public float Min = 0;
		public float Max = 1;
		public float Range { get { return Max - Min; } }
		public float RandomValue { get { return Random.Range(Min, Max); } }
		public float Normalize(float input)
		{
			float inRange = input - Min;
			float output = inRange / Range;
			return output;
		}

		public float GetFromNormalized(float input0to1)
		{
			return Mathf.Lerp(Min, Max, input0to1);
		}

		public FloatRange(float min, float max)
		{
			Min = min;
			Max = max;
		}
	}

	// Sin in the range 0 to +1 instead of -1 to +1
	public static float Sin01(float angRad)
    {
        return Mathf.Sin(angRad) * 0.5f + 0.5f;
    }
	
	// Cos in the range 0 to +1 instead of -1 to +1
    public static float Cos01(float angRad)
    {
        return Mathf.Cos(angRad) * 0.5f + 0.5f;
    }
	
	
	public static float lerpSin(float from, float to, float fraction)
	{
		const float Xextent = Mathf.PI;
		
		float adjustedFraction = Xextent * fraction - Mathf.PI / 2.0f;
		float value = (Mathf.Sin(adjustedFraction) + 1.0f) / 2.0f;
		
		float range = to - from;
		
		return (value * range) + from;
	}
	
	public static float lerpSinBounce(float from, float to, float fraction)
	{
		const float Xextent = Mathf.PI / .8f;
		float Yextent = (Mathf.Sin(Xextent - Mathf.PI / 2.0f) + 1.0f) / 2.0f;
		
		float adjustedFraction = Xextent * fraction - Mathf.PI / 2.0f;
		float value = (Mathf.Sin(adjustedFraction) + 1.0f) / 2.0f;
		value /= Yextent;
		
		float range = to - from;
		
		return (value * range) + from;
	}
	
	public bool WithinRadius(Vector3 A, Vector3 B, float r)
	{
		return ( (A - B).sqrMagnitude < (r * r) );
	}
	
	
	public struct Circle
	{
		public Circle(Vector2 center, float radius) { c = center; r = radius; }
		public Vector2 c;
		public float r;
	}
	
	// See:
	// http://paulbourke.net/geometry/circlesphere/
	// http://justbasic.wikispaces.com/Check+for+collision+of+two+circles,+get+intersection+points
	public static bool CircleCircleIntersection(Circle A, Circle B, out Vector2 p1, out Vector2 p2)
	{
		p1 = Vector2.zero;
		p2 = Vector2.zero;
		Vector2 sep = B.c - A.c;
		float d = sep.magnitude;
		
		
		// Too far apart for intersection
		if(d > A.r + B.r)
			return false;
		
		// One is completely inside the other
		if(d < Mathf.Abs(A.r - B.r))
			return false;
		
		// p0 is where sep intersects with the the line p1p2
		// a is dist from A.c to p0
		float a = ((A.r*A.r) - (B.r*B.r) + (d*d)) / (2 * d);
		Vector2 p0 = A.c + (sep * a/d);
		
		//Get the dist from pt0 to either p1 or p2 (it's the same for both)
   		float h = Mathf.Sqrt((A.r*A.r) - (a*a));
		
	    Vector2 r = new Vector2(
				(0-sep.y) * (h/d), 
				sep.x * (h/d) );
			
		p1.x = p0.x + r.x;
	    p2.x = p0.x - r.x;
	    p1.y = p0.y + r.y;
	    p2.y = p0.y - r.y;
		
		return true;
	}
	
	Vector3 ClosestPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{	
	    Vector3 AP = point - lineStart;
	    Vector3 AB = (lineEnd - lineStart).normalized;
	
	    float d = Vector3.Distance(lineStart, lineEnd);
	    float t = Vector3.Dot(AB, AP);
	
	    if (t <= 0) 
	        return lineStart;
	
	    if (t >= d) 
	        return lineEnd;
	
	    Vector3 closestPt = lineStart + AB * t;
	    return closestPt;
	}
}
