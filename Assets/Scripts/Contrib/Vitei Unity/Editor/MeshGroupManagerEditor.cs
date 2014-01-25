using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MeshGroupManager))] 
public class MeshGroupManagerEditor : Editor 
{
	public override void OnInspectorGUI() 
	{
		MeshGroupManager t = (MeshGroupManager)target;
		DrawDefaultInspector ();  

		
		float labelWidth = Screen.width/2.5f;

		EditorGUILayout.BeginVertical();

		foreach(GameObject go in t.m_meshGroupParent)
		{
			EditorGUILayout.BeginHorizontal();

			GUILayout.Label(go.name, GUILayout.Width(labelWidth));

			if(GUILayout.Button("Show"))
			{
				SetVisibility(go, true);
			}
			if(GUILayout.Button("Hide"))
			{
				SetVisibility(go, false);
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();
	}


	//----------------------------------
	static void SetVisibility(GameObject coh, bool show)
	{
		Renderer[] rs = coh.GetComponentsInChildren<Renderer>();
		for(int i = 0; i < rs.Length; i++)
		{
			rs[i].enabled = show;
		}
	}
}
