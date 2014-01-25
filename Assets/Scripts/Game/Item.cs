using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
	public enum Type { Game, Real };
	Type m_type = Type.Game;
	public Type MyType
	{
		get {return m_type;}
		set 
		{
			m_type = value;
			m_gameWorldSprite.SetActive( m_type == Type.Game );
			m_realWorldSprite.SetActive( m_type != Type.Game );
		}
	}


	public GameObject m_gameWorldSprite;
	public GameObject m_realWorldSprite;

	protected virtual void AwakeVirtual () {}

	// Use this for initialization
	void Awake () 
	{
		m_realWorldSprite.SetActive(false);
		AwakeVirtual();
	}

	void Collect(Player p)
	{
		if(m_type == Type.Game)
			CollectGame(p);
		else
			CollectReal(p);

		m_gameWorldSprite.SetActive(false);
		m_realWorldSprite.SetActive(false);
	}

	protected virtual void CollectGame(Player player) {}
	protected virtual void CollectReal(Player player) {}
}
