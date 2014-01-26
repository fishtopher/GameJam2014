using UnityEngine;
using System.Collections;

public class DualWorldGFX : MonoBehaviour 
{
	Item.Type m_type = Item.Type.Game;
	public Item.Type MyType
	{
		get {return m_type;}
		set 
		{
			m_type = value;
			m_gameWorldVisuals.SetActive( m_type == Item.Type.Game );
			m_realWorldVisuals.SetActive( m_type != Item.Type.Game );
		}
	}


	public GameObject m_gameWorldVisuals;
	public GameObject m_realWorldVisuals;

	// Use this for initialization
	void Awake () 
	{
		Item.Type t = m_type;
		m_type = t;
	}
}
