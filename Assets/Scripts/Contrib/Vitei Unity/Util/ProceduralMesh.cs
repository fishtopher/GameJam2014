using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ProceduralMesh : MonoBehaviour 
{
	public bool m_clearMeshEveryFrame = true;
	public Camera m_hudCam;
	public float m_zOffset = 1;
	Mesh m_mesh;
	MeshFilter m_meshFilter;
	
	public float m_oneUnit = 1.0f;
	
    public virtual void StartVirtual(){}
	void Awake() 
	{
        m_mesh = new Mesh();
        m_meshFilter = GetComponent<MeshFilter>();
		m_meshFilter.mesh = m_mesh;
		
		if(m_clearMeshEveryFrame)
		{
			StartCoroutine(ClearMeshAtEndOfFrame());
    	}
		
		if(m_hudCam != null)
			m_oneUnit = (m_hudCam.ScreenToWorldPoint(new Vector3(0,0,0)) - m_hudCam.ScreenToWorldPoint(new Vector3(1,0,0))).magnitude;
		
		StartVirtual();
	}
	
    IEnumerator ClearMeshAtEndOfFrame()
	{	
		while(true)
		{	
			yield return new WaitForEndOfFrame();
			Clear();
		};
	}
	
	void OnDisable()
	{}
	
	public void Clear()
	{
		if(m_meshFilter != null)
		{
			DestroyImmediate(m_mesh);
			m_mesh = new Mesh();
	 	    m_meshFilter.mesh = m_mesh;
		}
	}
	
	public enum Axis {X, Y, Z}
	public void AddCircle(Vector3 p, float radius, int resolution, Axis axis = Axis.Z)
	{
		AddCircle(p,radius,resolution,Color.white, axis);
	}
	
	public void AddCircle(Vector3 p, float radius, int resolution, Color col, Axis axis = Axis.Z)
	{
		p.z += m_zOffset;
		radius *= m_oneUnit;
		
		// Some useful numbers
		int numNew = resolution+2;
		float step = (Mathf.PI*2) / (float)resolution;
		
		// Resize the mesh arrays to take new data
		int vl = m_mesh.vertices.Length;
		Vector3[] vs = m_mesh.vertices;
		Array.Resize<Vector3>(ref vs, vs.Length+numNew);
		int cl = m_mesh.colors.Length;
		Color[] cs= m_mesh.colors;
		Array.Resize<Color>(ref cs, cs.Length+numNew);
		int tl = m_mesh.triangles.Length;
		int[] ts = m_mesh.triangles;
		Array.Resize<int>(ref ts, ts.Length+(resolution*3));
		
		// set the vert at the center of the circle
		int center = vl+numNew-1;
		vs[center] = p;
		cs[center] = col;
		
		// gen verts and define tris for all the slices of the fan
		float angS;
		for(int i = 0; i < resolution; i++)
		{	
			angS = step * i;
			if(axis == Axis.X)
				vs[vl+i] = p + new Vector3( 0, Mathf.Sin(angS) * radius, Mathf.Cos(angS) * radius);
			else if(axis == Axis.Y)
				vs[vl+i] = p + new Vector3( Mathf.Sin(angS) * radius, 0, Mathf.Cos(angS) * radius);
			else if(axis == Axis.Z)
				vs[vl+i] = p + new Vector3( Mathf.Sin(angS) * radius, Mathf.Cos(angS) * radius, 0);
			
			
			cs[cl+i] = col;
			
			ts[tl + (i*3) + 0] = center;
			ts[tl + (i*3) + 1] = vl+i;
			ts[tl + (i*3) + 2] = vl+i+1;
		}
		
		//do the last verts (not in the loop because it has to finish one early or it would define an extra triangle)
		angS = step * resolution;
		if(axis == Axis.X)
			vs[vl+resolution] = p + new Vector3( 0, Mathf.Sin(angS) * radius, Mathf.Cos(angS) * radius);
		else if(axis == Axis.Y)
			vs[vl+resolution] = p + new Vector3( Mathf.Sin(angS) * radius, 0, Mathf.Cos(angS) * radius);
		else if(axis == Axis.Z)
			vs[vl+resolution] = p + new Vector3( Mathf.Sin(angS) * radius, Mathf.Cos(angS) * radius, 0);
		cs[cl+resolution] = col;
		
		// reassign the data back to the mesh
		m_mesh.vertices = vs;
		m_mesh.triangles = ts;
		m_mesh.colors = cs;
		m_mesh.RecalculateBounds();
	}
	
	public void AddArc(Vector3 p, float radius, int resolution, float arcLenDegrees, float startOffsetDegrees, Color col, Axis axis = Axis.Z)
	{
	
		p.z += m_zOffset;
		radius *= m_oneUnit;
		
		// Some useful numbers
		int numNew = resolution+2;
		float step = (arcLenDegrees * Mathf.Deg2Rad) / (float)resolution;
		float startOffsetRad = startOffsetDegrees * Mathf.Deg2Rad;
		
		// Resize the mesh arrays to take new data
		int vl = m_mesh.vertices.Length;
		Vector3[] vs = m_mesh.vertices;
		Array.Resize<Vector3>(ref vs, vs.Length+numNew);
		int cl = m_mesh.colors.Length;
		Color[] cs= m_mesh.colors;
		Array.Resize<Color>(ref cs, cs.Length+numNew);
		int tl = m_mesh.triangles.Length;
		int[] ts = m_mesh.triangles;
		Array.Resize<int>(ref ts, ts.Length+(resolution*3));
		
		// set the vert at the center of the circle
		int center = vl+numNew-1;
		vs[center] = p;
		cs[center] = col;
		
		// gen verts and define tris for all the slices of the fan
		float angS;
		for(int i = 0; i < resolution; i++)
		{	
			angS = step * i + startOffsetRad;
			if(axis == Axis.X)
				vs[vl+i] = p + new Vector3( 0, Mathf.Sin(angS) * radius, Mathf.Cos(angS) * radius);
			else if(axis == Axis.Y)
				vs[vl+i] = p + new Vector3( Mathf.Sin(angS) * radius, 0, Mathf.Cos(angS) * radius);
			else if(axis == Axis.Z)
				vs[vl+i] = p + new Vector3( Mathf.Sin(angS) * radius, Mathf.Cos(angS) * radius, 0);
			
			
			cs[cl+i] = col;
			
			ts[tl + (i*3) + 0] = center;
			ts[tl + (i*3) + 1] = vl+i;
			ts[tl + (i*3) + 2] = vl+i+1;
		}
		
		//do the last verts (not in the loop because it has to finish one early or it would define an extra triangle)
		angS = step * resolution + startOffsetRad;
		if(axis == Axis.X)
			vs[vl+resolution] = p + new Vector3( 0, Mathf.Sin(angS) * radius, Mathf.Cos(angS) * radius);
		else if(axis == Axis.Y)
			vs[vl+resolution] = p + new Vector3( Mathf.Sin(angS) * radius, 0, Mathf.Cos(angS) * radius);
		else if(axis == Axis.Z)
			vs[vl+resolution] = p + new Vector3( Mathf.Sin(angS) * radius, Mathf.Cos(angS) * radius, 0);
		cs[cl+resolution] = col;
		
		// reassign the data back to the mesh
		m_mesh.vertices = vs;
		m_mesh.triangles = ts;
		m_mesh.colors = cs;
		m_mesh.RecalculateBounds();
	
	}
	
	
	public void AddLine(Vector3 s, Vector3 e, float w)
	{
		AddLine(s,e,w,Color.white);
	}
	public void AddLine(Vector3 s, Vector3 e, float w, Color col) 
	{
		Color[] cols = new Color[4] {col, col, col, col};
		AddQuad(m_mesh, GetVertsForLine(s, e, w), cols);
	}
	
	public void AddLine(Vector3 s, Vector3 e, float w, Color colStart, Color colEnd) 
	{
		Color[] cols = new Color[4] {colStart, colStart, colEnd, colEnd};
		AddQuad(m_mesh, GetVertsForLine(s, e, w), cols);
	}
	
	Vector3[] GetVertsForLine(Vector3 s, Vector3 e, float w)
	{
		w = w / 2;
		Vector3[] q = new Vector3[4];
		
		//Vector3 n = Vector3.Cross(s, e);
		Vector3 l = Vector3.Cross(Camera.main.transform.forward*-1, e-s);
		l.Normalize();
		
		q[0] = transform.InverseTransformPoint(s + l * w);
		q[1] = transform.InverseTransformPoint(s + l * -w);
		q[2] = transform.InverseTransformPoint(e + l * w);
		q[3] = transform.InverseTransformPoint(e + l * -w);

		return q;
	}
	
	public void AddLine(List<Vector3> points, float width, Color col, float sideOffset = 0)
	{
		AddLine(points, width, col, col, col, col, sideOffset);
	}

	public void AddLine(List<Vector3> points, float width, Color colStart, Color colEnd, float ofssetFromCenter = 0)
	{
		AddLine(points, width, colStart, colStart, colEnd, colEnd, ofssetFromCenter);
	}
	
	public void AddLineWithSidewaysGradient(List<Vector3> points, float width, Color colLeft, Color colRight, float ofssetFromCenter = 0)
	{
		AddLine(points, width, colLeft, colRight, colLeft, colRight, ofssetFromCenter);
	}
	
	
	public void AddLine(List<Vector3> points, float width, Color colStartLeft, Color colStartRight, Color colEndLeft, Color colEndRight, float ofssetFromCenter = 0)
	{	
		if(m_mesh == null)
			return;
		
		if(points.Count < 2)
			return;
		
		// Add a dummy point so that we can calculate the final actual points normal
		Vector3 fpv = points[points.Count-1] - points[points.Count-2];
		points.Add(points[points.Count-1] + fpv);
		
		int numNew = (points.Count-2) * 4;
		int numNewT = (points.Count-2) * 6;
		
		// Resize the mesh arrays to take new data
		int vl = m_mesh.vertices.Length;
		Vector3[] v = m_mesh.vertices;
		Array.Resize<Vector3>(ref v, v.Length+numNew);
		Color[] c= m_mesh.colors;
		Array.Resize<Color>(ref c, c.Length+numNew);
		int tl = m_mesh.triangles.Length;
		int[] t = m_mesh.triangles;
		Array.Resize<int>(ref t, t.Length+numNewT);
		
		float w = width * 0.5f;
		for(int i = 0; i < (points.Count-2); i++)
		{
			Vector3 A = points[i];
			Vector3 B = points[i+1];
			Vector3 C = points[i+2];
			
			Vector3 AB = (B-A).normalized;
			Vector3 perpA = Vector3.Cross(Vector3.up, AB);
			
			Vector3 BC = (C-B).normalized;
			Vector3 perpB = Vector3.Cross(Vector3.up, BC);
			
			v[vl+i*4+0] = A + perpA * w + perpA * ofssetFromCenter * w;
			v[vl+i*4+1] = A + perpA * -w + perpA * ofssetFromCenter * w;
			v[vl+i*4+2] = B + perpB * w + perpB * ofssetFromCenter * w;
			v[vl+i*4+3] = B + perpB * -w + perpB * ofssetFromCenter * w;
			
			c[vl+i*4+0] = colStartRight;
			c[vl+i*4+1] = colStartLeft;
			c[vl+i*4+2] = colEndRight;
			c[vl+i*4+3] = colEndLeft;
			
			t[tl+i*6+0] = vl+i*4+0;
			t[tl+i*6+1] = vl+i*4+1;
			t[tl+i*6+2] = vl+i*4+2;
			
			t[tl+i*6+3] = vl+i*4+1;
			t[tl+i*6+4] = vl+i*4+3;
			t[tl+i*6+5] = vl+i*4+2;
		}
		
		m_mesh.vertices = v;
		m_mesh.triangles = t;
		m_mesh.colors = c;
		m_mesh.RecalculateBounds();
		
		// remove the dummy point we added
		points.RemoveAt(points.Count-1);
	}
	
	public void AddLine(List<Vector3> points, List<Vector3> normals, float width, Color colLeft, Color colRight, float ofssetFromCenter = 0)
	{	
		if(m_mesh == null)
			return;
		
		// Add a dummy point so that we can calculate the final actual points normal
		Vector3 fpv = points[points.Count-1] - points[points.Count-2];
		points.Add(points[points.Count-1] + fpv);
		normals.Add(normals[normals.Count-1]);
		
		int numNew = (points.Count-2) * 4;
		int numNewT = (points.Count-2) * 6;
		
		// Resize the mesh arrays to take new data
		int vl = m_mesh.vertices.Length;
		Vector3[] v = m_mesh.vertices;
		Array.Resize<Vector3>(ref v, v.Length+numNew);
		Color[] c= m_mesh.colors;
		Array.Resize<Color>(ref c, c.Length+numNew);
		int tl = m_mesh.triangles.Length;
		int[] t = m_mesh.triangles;
		Array.Resize<int>(ref t, t.Length+numNewT);
		
		float w = width * 0.5f;
		
		Vector3 A = points[0];
		Vector3 B = points[1];
		Vector3 AB = (B-A).normalized;
		Vector3 perpA = Vector3.Cross(normals[0], AB);
		Vector3 p1 = A + perpA * w + perpA * ofssetFromCenter * w;
		Vector3 p2 = A + perpA * -w + perpA * ofssetFromCenter * w;
		
		for(int i = 0; i < (points.Count-2); i++)
		{
			//Color ccl = Color.Lerp(new Color(1,0,0,1), colLeft, (float)i/(float)(points.Count-2));
			//Color ccr = Color.Lerp(new Color(1,0,0,1), colRight, (float)i/(float)(points.Count-2));
			
				    B = points[i+1];
			Vector3 C = points[i+2];
			
			
			Vector3 BC = (C-B).normalized;
			Vector3 perpB = Vector3.Cross(normals[i+1], BC);
			
			Vector3 p3 = B + perpB * w + perpB * ofssetFromCenter * w;
			Vector3 p4 = B + perpB * -w + perpB * ofssetFromCenter * w;
				
			v[vl+i*4+0] = p1;
			v[vl+i*4+1] = p2;
			v[vl+i*4+2] = p3;
			v[vl+i*4+3] = p4;
			
			p1 = p3;
			p2 = p4;
			
			c[vl+i*4+0] = colRight;
			c[vl+i*4+1] = colLeft;
			c[vl+i*4+2] = colRight;
			c[vl+i*4+3] = colLeft;
			
			t[tl+i*6+0] = vl+i*4+0;
			t[tl+i*6+1] = vl+i*4+1;
			t[tl+i*6+2] = vl+i*4+2;
			
			t[tl+i*6+3] = vl+i*4+1;
			t[tl+i*6+4] = vl+i*4+3;
			t[tl+i*6+5] = vl+i*4+2;
		}
		
		m_mesh.vertices = v;
		m_mesh.triangles = t;
		m_mesh.colors = c;
		m_mesh.RecalculateBounds();
		
		// remove the dummy point we added
		points.RemoveAt(points.Count-1);
	}
	
	
	void AddQuad(Mesh m, Vector3[] quad, Color[] col) 
	{
		int vl = m.vertices.Length;
		Vector3[] vs = m.vertices;
		Array.Resize<Vector3>(ref vs, vs.Length+4);
		vs[vl] = quad[0];
		vs[vl+1] = quad[1];
		vs[vl+2] = quad[2];
		vs[vl+3] = quad[3];
		
		int cl = m.colors.Length;
		Color[] cs= m.colors;
		Array.Resize<Color>(ref cs, cs.Length+4);
		cs[cl] = col[0];
		cs[cl+1] = col[1];
		cs[cl+2] = col[2];
		cs[cl+3] = col[3];
		
		int tl = m.triangles.Length;
		int[] ts = m.triangles;
		Array.Resize<int>(ref ts, ts.Length+6);
		ts[tl] = vl;
		ts[tl+1] = vl+1;
		ts[tl+2] = vl+2;
		ts[tl+3] = vl+1;
		ts[tl+4] = vl+3;
		ts[tl+5] = vl+2;
		
		m.vertices = vs;
		m.triangles = ts;
		m.colors = cs;
		m.RecalculateBounds();
	}
	
	void AddTriangle(Mesh m, Vector3[] tri, Color[] col) 
	{
		int vl = m.vertices.Length;
		Vector3[] vs = m.vertices;
		Array.Resize<Vector3>(ref vs, vs.Length+3);
		vs[vl] = tri[0];
		vs[vl+1] = tri[1];
		vs[vl+2] = tri[2];
		
		int cl = m.colors.Length;
		Color[] cs= m.colors;
		Array.Resize<Color>(ref cs, cs.Length+3);
		cs[cl] = col[0];
		cs[cl+1] = col[1];
		cs[cl+2] = col[2];
		
		int tl = m.triangles.Length;
		int[] ts = m.triangles;
		Array.Resize<int>(ref ts, ts.Length+3);
		ts[tl] = vl;
		ts[tl+1] = vl+1;
		ts[tl+2] = vl+2;
		
		m.vertices = vs;
		m.triangles = ts;
		m.colors = cs;
		m.RecalculateBounds();
	}
}