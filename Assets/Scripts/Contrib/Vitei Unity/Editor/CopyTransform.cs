using UnityEngine;
using UnityEditor;
using System.Collections;
 
public class TransformCopier : ScriptableObject
{
	private static Vector3 position;
	private static Quaternion rotation;
	private static Vector3 scale;
	private static string myName; 
 
    [MenuItem ("Vitei/Transform/Copy Transform &c")]
	public static void DoRecord()
    {
       position = Selection.activeTransform.position;
       rotation = Selection.activeTransform.rotation;
       //scale = Selection.activeTransform.localScale;
       //myName = Selection.activeTransform.name;       
    }
 
	[MenuItem ("Vitei/Transform/Paste Transform &v")]
	public  static void DoApply()
    {
        Selection.activeTransform.position = position;
        Selection.activeTransform.rotation = rotation;
        //Selection.activeTransform.localScale = scale;      
    }

	
	[MenuItem ("Vitei/Transform/Average Positions &a")]
	public static void AveragePositions()
	{
		Vector3 ap = Vector3.zero;
		for(int i = 0; i < Selection.gameObjects.Length; i++)
		{
			ap += Selection.gameObjects[i].transform.position;
		}
		ap /= (float)Selection.gameObjects.Length;
		
		for(int i = 0; i < Selection.gameObjects.Length; i++)
		{
			Selection.gameObjects[i].transform.position = ap;
		}
	}
}