using UnityEngine;
using System.Collections;

public class AnimationScaler : MonoBehaviour 
{
	public float m_scale = 1.0f;

	// Use this for initialization
	void Awake () 
	{
		foreach(AnimationState s in animation)
		{
			animation[s.name].speed *= m_scale;
		}
		enabled = false;
	}
}
