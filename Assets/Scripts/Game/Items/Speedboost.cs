using UnityEngine;
using System.Collections;

public class Speedboost : Item 
{
	float m_speedBoost = 3;
	float m_duration = 0.5f;

	protected override void CollectGame(Player player)
	{
		StartCoroutine("GameFX", player);
	}

	IEnumerator GameFX(Player p)
	{
		((Player)p).m_runSpeed *= m_speedBoost;
		yield return new WaitForSeconds (m_duration);
		((Player)p).m_runSpeed /= m_speedBoost;

	}
	
	protected override void CollectReal(Player player)
	{
		
	}
}