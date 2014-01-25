using UnityEngine;
using System.Collections;

public class ScaleVariation : MonoBehaviour 
{
	public float m_maxVariationNormalised = 0.1f;

	// Use this for initialization
	void Start () 
	{
		float scaleVal = Random.Range(1-m_maxVariationNormalised, 1+m_maxVariationNormalised);
		transform.localScale = transform.localScale * scaleVal;
		enabled = false;
	}
	
}
