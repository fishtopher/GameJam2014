using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Grouping : ScriptableObject
{
	[MenuItem ("Vitei/Group/Group %g")]
	static void GroupObjectsAtCentroid()
	{
		GroupObjects(Location.Centroid);
	}
	
	[MenuItem ("Vitei/Group/Group From Parent Origin %#g")]
	static void GroupObjectsFromOrigin()
	{
		GroupObjects(Location.Origin);
	}
	
	static List<Transform> GetAllParents(Transform t)
	{
		List<Transform> parents = new List<Transform>();
		while(t.parent != null)
		{
			parents.Add(t.parent);
			t = t.parent;
		}
		return parents;	
	}
	
	enum Location {Origin, Centroid}
	static void GroupObjects(Location location)
	{
		int nobs = Selection.gameObjects.Length;
		if(nobs == 0)
			return;

		//Undo.RegisterSceneUndo("Group Objects");
		Undo.RecordObjects(Selection.gameObjects, "Group Objects");
		
		// Work out the avg pos and if all these objects share a parent anywhere
		Vector3 center = Vector3.zero;
		List<Transform> viableParents = GetAllParents(Selection.gameObjects[0].transform);
		for(int i = 0; i < nobs; i++)
		{	
			center += Selection.gameObjects[i].transform.position;
			List<Transform> myParents = GetAllParents(Selection.gameObjects[i].transform);
			for(int j = viableParents.Count-1; j >= 0; j--)
			{
				if(!myParents.Contains(viableParents[j]))
				{
					viableParents.RemoveAt(j);
				}
			}
		}
		Transform commonParent = viableParents.Count > 0 ? viableParents[0] : null;
		
		// Make a new parent object and set it up appopriately
		GameObject groupObj = new GameObject("New Group");
    	Undo.RegisterCreatedObjectUndo(groupObj, "Group Objects");
		
		Undo.RecordObjects(Selection.gameObjects, "Group Objects");
		if(commonParent != null)
		{
			groupObj.transform.parent = commonParent;
		}
		
		if(location == Location.Centroid)
		{
			center /= (float)nobs;
			groupObj.transform.position = center;
		}
		else if(location == Location.Origin)
		{
			groupObj.transform.localPosition = Vector3.zero;
		}
		
		// Attach the objects to the group
		for(int i = 0; i < nobs; i++)
		{
			Selection.gameObjects[i].transform.parent = groupObj.transform;
		}
		
		// Set the new object to be the seected one.
		Selection.activeGameObject = groupObj;
	}
	
}
