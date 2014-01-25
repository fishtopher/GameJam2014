using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorUtil : MonoBehaviour {

	[MenuItem ("Vitei/GameObject/Count &#c")]
	static void CountGameObjects()
	{
		Debug.Log( "Selected Objects: " + Selection.gameObjects.Length );
	}
}
