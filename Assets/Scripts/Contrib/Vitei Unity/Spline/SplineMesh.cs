using UnityEngine;
//using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Spline))]
public class SplineMesh : MonoBehaviour
{
	public float m_resolution = 5;
	float m_resolutionLastFrame;
	public float m_halfWidth = 2;
	float m_halfWidthLastFrame;
	public Vector2 m_uvScale = new Vector2(1, 0.2f);
	Vector2 m_uvScaleLastFrame = new Vector2(1, 0.2f);
	public bool m_autoUpdateInEditor = true;
	public bool m_autoUpdateInGame = false;
	
    void OnRenderObject()
	{
		if(Application.isPlaying)
		{
			if(m_autoUpdateInGame)
				RebuildMesh();
		}
		else if(m_autoUpdateInEditor )
		{
			Spline s = GetComponent<Spline>();
			bool dirty = 
				m_resolutionLastFrame != m_resolution ||
				m_halfWidthLastFrame != m_halfWidth || 
				m_uvScaleLastFrame != m_uvScale ||
				s.IsDirty;

			if(dirty)
			{
				RebuildMesh();
				m_resolutionLastFrame = m_resolution;
				m_halfWidthLastFrame = m_halfWidth;
				m_uvScaleLastFrame = m_uvScale;
			}
		}
	}

    public void RebuildMesh()
	{
		Spline s = GetComponent<Spline>();
      	s.CalculateHiddenPoints();

        float numStepsF = (s.m_points.Count-1) * m_resolution;
		int numSteps = (int)numStepsF;

        int numVertsTotal = (numSteps+1)*2;
        Vector3[] vertices = new Vector3[numVertsTotal];
        Vector3[] normals = new Vector3[numVertsTotal];
        Vector2[] uvs = new Vector2[numVertsTotal];

		float t = 0;
		Vector3 p, tang;
		Vector3 prevPt = s.PointAtNormalisedTime(0)-transform.position;
		float d = 0;
        for (int i = 0; i < numSteps+1; ++i)
        {
			t = (float)i/(float)(numSteps);
			s.PointAndTangentAtNormalisedTime(t, out p, out tang);
			p-=transform.position;
			Vector3 r = Vector3.Cross(Vector3.up, tang);
			d += (prevPt-p).magnitude;

			int v = i*2;
			vertices[v+0] = p + r * -m_halfWidth;
			vertices[v+1] = p + r * m_halfWidth;
			normals[v+0] = Vector3.up;
			normals[v+1] = Vector3.up;
			uvs[v+0]=new Vector2(0.5f - (0.5f*m_uvScale.x), d*m_uvScale.y);
			uvs[v+1]=new Vector2(0.5f + (0.5f*m_uvScale.x), d*m_uvScale.y);

			prevPt = p;
		}

		// Build some triangles
		// 2 - 3
		// | / |
		// 0 - 1
		int[] triangles = new int[numSteps * 6];
		int index = 0;
		for (int i = 0; i < numSteps; ++i)
		{
			int baseVertex = i * 2;
			triangles[index + 0] = baseVertex + 0;
			triangles[index + 1] = baseVertex + 3;
			triangles[index + 2] = baseVertex + 1;

			triangles[index + 3] = baseVertex + 0;
			triangles[index + 4] = baseVertex + 2;
			triangles[index + 5] = baseVertex + 3;

			index += 6;
		}

		// Apply all these things to a mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
		mesh.triangles = triangles;
		//XEUnwrapping.GenerateSecondaryUVSet(mesh);
        mesh.RecalculateBounds();

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter)
        {
            meshFilter.mesh = mesh;
        }
        var mc = GetComponent<MeshCollider>();
        if (mc)
        {
            mc.sharedMesh = mesh;
        }

	}
}
