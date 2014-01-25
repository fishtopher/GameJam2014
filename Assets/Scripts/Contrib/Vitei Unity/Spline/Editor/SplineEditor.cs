using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Spline))]
public class SplineEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		EditorGUILayout.BeginVertical();
		if(GUILayout.Button("Fixup"))
		{
			Spline sp = (Spline)target;
			sp.SanityCheck();
		}
		EditorGUILayout.EndVertical();
    }
}
