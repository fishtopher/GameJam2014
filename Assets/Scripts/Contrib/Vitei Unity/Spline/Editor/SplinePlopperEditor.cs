using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SplinePlopper))]
public class SplinePlopperEditor : Editor 
{	
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		
		EditorGUILayout.BeginVertical();
		if(GUILayout.Button("Randomize"))
		{
			SplinePlopper sp = (SplinePlopper)target;
			sp.Randomize();
		}
		EditorGUILayout.EndVertical();
	}
}