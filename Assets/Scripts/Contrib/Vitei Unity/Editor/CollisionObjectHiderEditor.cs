using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CollisionObjectHider))] 
public class CollisionObjectHiderEditor : Editor 
{
	public override void OnInspectorGUI() 
	{
		CollisionObjectHider t = (CollisionObjectHider)target;
		DrawDefaultInspector ();  

		EditorGUILayout.BeginVertical();
		if(GUILayout.Button("Show All"))
		{
			SetVisibility(t.gameObject, true);
		}
		if(GUILayout.Button("Hide All"))
		{
			SetVisibility(t.gameObject, false);
		}
		EditorGUILayout.EndVertical();
	}

	static void SetVisibility(GameObject coh, bool show)
	{
		Renderer[] rs = coh.GetComponentsInChildren<Renderer>();
		for(int i = 0; i < rs.Length; i++)
		{
			rs[i].enabled = show;
		}
	}
	
	[MenuItem("Vitei/Collision Objects/Show All")]
	static void ShowCollisionObjectsInCollisionLayer()
	{
		GameObject colLayer = GameObject.Find("Layer_Collision");
		SetVisibility(colLayer, true);
	}
	
	[MenuItem("Vitei/Collision Objects/Hide All")]
	static void HideCollisionObjectsInCollisionLayer()
	{
		GameObject colLayer = GameObject.Find("Layer_Collision");
		SetVisibility(colLayer, false);
	}

	static readonly string[] AlwaysShow = { "block out C export", "Passengers", "BlackCab" };
	static readonly string[] AlwaysHide = { "MapVolume", "Collision Internal", "CarCollision" };
	static void SetPermanents()
	{
		Transform[] allObjs = GameObject.FindObjectsOfType<Transform>();
		for(int i = 0; i < allObjs.Length; i++)
		{
			for(int j = 0; j < AlwaysShow.Length; j++)
			{
				if(allObjs[i].name == AlwaysShow[j])
				{
					Renderer[] childRenderers = allObjs[i].GetComponentsInChildren<Renderer>();
					for(int k = 0; k < childRenderers.Length; k++)
					{
						childRenderers[k].enabled = true;
					}
				}
			}
			
			for(int j = 0; j < AlwaysHide.Length; j++)
			{
				if(allObjs[i].name == AlwaysHide[j])
				{
					Renderer[] childRenderers = allObjs[i].GetComponentsInChildren<Renderer>();
					for(int k = 0; k < childRenderers.Length; k++)
					{
						childRenderers[k].enabled = false;
					}
				}
			}
		}

		GameObject[] taggedHidden = GameObject.FindGameObjectsWithTag("HiddenCollision");
		for(int j = 0; j < taggedHidden.Length; j++)
		{
			if(taggedHidden[j].renderer)
				taggedHidden[j].renderer.enabled = false;
		}
	}

	[MenuItem("Vitei/Collision Objects/Toggle/Collision Only")]
	static void ShowCollisionOnly()
	{
		Renderer[] allRenderers = GameObject.FindObjectsOfType<Renderer>();
		for(int i = 0; i < allRenderers.Length; i++)
		{
			allRenderers[i].enabled = 
				(allRenderers[i].name == "Collision") ||
				(allRenderers[i].transform.parent != null && allRenderers[i].transform.parent.name == "Collision");
		}
		ShowCollisionObjectsInCollisionLayer();
		SetPermanents();
	}

	[MenuItem("Vitei/Collision Objects/Toggle/Geometry Only")]
	static void ShowGeometryOnly()
	{
		Renderer[] allRenderers = GameObject.FindObjectsOfType<Renderer>();
		for(int i = 0; i < allRenderers.Length; i++)
		{
			allRenderers[i].enabled = !(
				(allRenderers[i].name == "Collision") ||
				(allRenderers[i].transform.parent != null && allRenderers[i].transform.parent.name == "Collision"));
		}
		HideCollisionObjectsInCollisionLayer();
		SetPermanents();
	}
}
