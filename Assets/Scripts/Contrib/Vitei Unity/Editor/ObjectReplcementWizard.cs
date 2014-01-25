using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ObjectReplcementWizard: ScriptableWizard
{
	public bool m_limitToTree = false;
	public GameObject m_onlyReplaceChildrenOf;
	public GameObject m_originalObject;
	public GameObject m_newPrefab;
	
	public Vector3 m_rotationOffset = Vector3.zero;
	public Vector3 m_rotationOffsetLast = Vector3.zero;
	
	bool m_previewing = false;
	
	List<GameObject> m_newObjects;
	List<Quaternion> m_objLocalStartRots;
	
	string fileName;
	void OnGUI () 
	{
		if(!m_previewing)
		{
			m_limitToTree = EditorGUILayout.Toggle("Limit scope", m_limitToTree);
			
			
			if(m_limitToTree)
			{
				m_onlyReplaceChildrenOf = EditorGUILayout.ObjectField("    To children of", m_onlyReplaceChildrenOf, typeof(GameObject), true) as GameObject;
				EditorGUILayout.Separator();
			}
			m_originalObject = EditorGUILayout.ObjectField("Replace instances of", m_originalObject, typeof(GameObject), true) as GameObject;
			m_newPrefab = EditorGUILayout.ObjectField("Wih", m_newPrefab, typeof(GameObject), false) as GameObject;
		}
		else
		{
			EditorGUILayout.LabelField("Tweak rotation");
		}
		m_rotationOffset = EditorGUILayout.Vector3Field("Rotation offset", m_rotationOffset);
		
		if(m_rotationOffset != m_rotationOffsetLast && m_newObjects != null)
		{
			Quaternion q = Quaternion.Euler(m_rotationOffset);
			for(int i = 0; i < m_newObjects.Count; i++)
			{
				m_newObjects[i].transform.rotation = m_objLocalStartRots[i] * q;
			}
		}
		
		m_rotationOffsetLast = m_rotationOffset;
		
		EditorGUILayout.Separator();
		if(m_previewing)
		{	
			if(GUILayout.Button("Finished"))
			{
				Close();
			}
		}
		else
		{
			GUI.enabled = (m_originalObject != null && m_newPrefab != null);
			
			if(GUILayout.Button("Replace"))
			{
				OnWizardCreate();
				m_previewing = true;
			}
			
			GUI.enabled = true;
		}
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
	}
	
	
	[MenuItem ("Vitei/Replace/Prefab Instances")]
	static void CreateWizard () 
	{
		ObjectReplcementWizard wiz = ScriptableWizard.DisplayWizard<ObjectReplcementWizard>("Replace Prefab Instances", "Replace", "Cancel");
		
		if(Selection.activeGameObject != null)
		{
			wiz.m_originalObject = Selection.activeGameObject;
			if(Selection.activeGameObject.transform.parent)
			{
				wiz.m_onlyReplaceChildrenOf = Selection.activeGameObject.transform.parent.gameObject;
			}
		}
	}
	
	void OnWizardCreate () 
	{
		if(m_onlyReplaceChildrenOf == false)
			m_onlyReplaceChildrenOf = null;
		
		List<GameObject> currents = FindAllPrefabInstances(m_onlyReplaceChildrenOf, m_originalObject);
		m_newObjects = new List<GameObject>(currents.Count);
		m_objLocalStartRots = new List<Quaternion>(currents.Count);
		
		for(int i = 0; i < currents.Count; i++)
		{
			GameObject ngo = PrefabUtility.InstantiatePrefab(m_newPrefab) as GameObject;
			m_newObjects.Add(ngo);
			
			ngo.transform.parent= currents[i].transform.parent;
			ngo.transform.position = currents[i].transform.position;
			m_objLocalStartRots.Add(currents[i].transform.localRotation);
			ngo.transform.rotation = currents[i].transform.rotation * Quaternion.Euler(m_rotationOffset);
			ngo.transform.localScale = currents[i].transform.localScale;
			
			Undo.DestroyObjectImmediate(currents[i]);
			Undo.RegisterCreatedObjectUndo(ngo, "Replace");
		}
	}  
	
	void OnWizardOtherButton () 
	{
	}
	
	List<GameObject> FindAllPrefabInstances(GameObject parent, GameObject prefabToMatch)
	{
		UnityEngine.Object myPrefab = PrefabUtility.GetPrefabParent(prefabToMatch);
		List<GameObject> result = new List<GameObject>();
		
		GameObject[] allObjects;
		if(parent == null)
		{
			allObjects = (GameObject[])FindObjectsOfType(typeof(GameObject));
		}
		else
		{
			Transform[] ts = parent.GetComponentsInChildren<Transform>();
			allObjects = new GameObject[ts.Length];
			for(int i = 0; i < ts.Length; i++)
			{
				allObjects[i] = ts[i].gameObject;
			}
		}
		foreach(GameObject go in allObjects)
		{
			UnityEngine.Object go_prefab = PrefabUtility.GetPrefabParent(go);
			if (myPrefab == go_prefab)
				result.Add(go);
		}
		return result;
	}
}
