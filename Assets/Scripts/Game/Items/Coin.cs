using UnityEngine;
using System.Collections;

public class Coin : Item 
{
	public int gameValue = 100;
	public int realValue = 1;
	public float m_realStopTime = 1.0f;
	float m_realSpeedPrePickup;

	protected override void CollectGame(Player p)
	{
		Player.Instance.Score += gameValue;
	}

	protected override void CollectReal(Player player)
	{
		m_realSpeedPrePickup = player.m_runSpeed;
		StartCoroutine("RealFX", player);
	}

	IEnumerator RealFX(Player p)
	{
		((Player)p).m_runSpeed = 0;
		yield return new WaitForSeconds (m_realStopTime);
		Player.Instance.Score += realValue;
		
		((Player)p).m_runSpeed = m_realSpeedPrePickup;
		
	}
}
