using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(TextMesh))]
public class TextMatcher : MonoBehaviour {

	public TextMesh m_source;

	void Update () 
	{
		if(m_source)
		{
			TextMesh tm = GetComponent<TextMesh>();
			if(tm && tm.text != m_source.text)
			{
				tm.text = m_source.text;
			}
		}
	}
}
