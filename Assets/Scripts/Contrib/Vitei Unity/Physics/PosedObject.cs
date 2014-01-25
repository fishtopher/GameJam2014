using UnityEngine;
using System.Collections;

public class PosedObject : MonoBehaviour 
{
//	Vector3[] m_localPosition;
//	Quaternion[] m_localRotation;
//	Transform[] m_transforms;
//	
//	// Use this for initialization
//	void Awake () 
//	{
//		m_transforms = GetComponentsInChildren<Transform>();
//		m_localPosition = new Vector3[m_transforms.Length];
//		m_localRotation = new Quaternion[m_transforms.Length];
//		for(int i = 0; i < m_transforms.Length; i++)
//		{
//			m_localPosition[i] = m_transforms[i].localPosition;
//			m_localRotation[i] = m_transforms[i].localRotation;
//		}	
////	}
//	
//	public void CopyPoseTo(Transform objectRoot)
//	{
//		Transform transforms = GetComponentsInChildren<Transform>();
//		for(int i = 0; i < transforms.Length; i++)
//		{
//			Transform t = objectRoot.FindChildRecursive(transforms[i].name);
//			if(t)
//			{
//				t.localPosition = transforms[i].localPosition;
//				t.localRotation = transforms[i].localRotation;
//			}
//		}
//	}
}
