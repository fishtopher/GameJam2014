using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
	public enum Type { Game, Real };
	[HideInInspector]
	public Type m_type = Type.Game;

	public GameObject m_gameWorldSprite;
	public GameObject m_realWorldSprite;

	protected virtual void StartVirtual () {}

	// Use this for initialization
	void Start () 
	{
		m_realWorldSprite.SetActive(false);
		StartVirtual();
	}

	void Collect(Player p)
	{
		if(m_type == Type.Game)
			CollectGame(p);
		else
			CollectReal(p);
	}

	protected virtual void CollectGame(Player player) {}
	protected virtual void CollectReal(Player player) {}
}
