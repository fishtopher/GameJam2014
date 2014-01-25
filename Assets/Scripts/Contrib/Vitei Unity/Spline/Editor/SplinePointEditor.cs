using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SplinePoint))] 
public class SplinePointEditor : Editor 
{

	public override void OnInspectorGUI() 
	{
		SplinePoint sp = (SplinePoint)target;
		Spline s = sp.transform.parent.GetComponent<Spline>();
		
		DrawDefaultInspector ();  
		
		
        EditorGUILayout.BeginVertical();
		if(GUILayout.Button("Add Point Before"))
		{
			s.InsertPointBefore(sp);
			
		}
 	    if(GUILayout.Button("Add Point After"))
		{
			s.InsertPointAfter(sp);
		}
        EditorGUILayout.EndVertical();
    }
}
