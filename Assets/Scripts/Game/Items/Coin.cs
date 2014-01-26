using UnityEngine;
using System.Collections;

public class Coin : Item 
{
	public int gameValue = 100;
	public int realValue = 1;
	private float m_realStopTime = 0.5f;
	float m_realSpeedPrePickup;

	protected override void CollectGame(Player p)
	{
		Player.Instance.Score += gameValue;

		SoundManager.PlaySound("coin");

		GameObject collectText = GameObject.Instantiate( m_ItemGetTextMesh, Player.Instance.transform.position, Quaternion.identity ) as GameObject;
		collectText.GetComponent<ItemGetText>().Initialize( "+" + gameValue.ToString(), Color.white );
	}

	protected override void CollectReal(Player player)
	{
		m_realSpeedPrePickup = player.m_runSpeed;
		StartCoroutine("RealFX", player);

		SoundManager.PlaySound("coin");

		GameObject collectText = GameObject.Instantiate( m_ItemGetTextMesh, Player.Instance.transform.position, Quaternion.identity ) as GameObject;
		collectText.GetComponent<ItemGetText>().Initialize( "Coins +" + gameValue.ToString(), Color.white );
	}

	IEnumerator RealFX(Player p)
	{
		((Player)p).m_runSpeed = 0;
		Player.Instance.PlayAnimation("pickup");

		((Player)p).IsStunned = true;
		yield return new WaitForSeconds (m_realStopTime);
		((Player)p).IsStunned = false;
		((Player)p).CheckControlDown();
		Player.Instance.Score += realValue;
		
		((Player)p).m_runSpeed = m_realSpeedPrePickup;
		Player.Instance.PlayAnimation("run");
	}
}
