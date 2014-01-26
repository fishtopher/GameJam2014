using UnityEngine;
using System.Collections;

public class Speedboost : Item 
{
	float m_speedBoost = 3;
	float m_duration = 0.5f;
	private float m_realStopTime = 0.5f;
	float m_realSpeedPrePickup;

	protected override void CollectGame(Player player)
	{
		Player.Instance.PlayAnimation("fly");
		StartCoroutine("GameFX", player);

		GameObject collectText = GameObject.Instantiate( m_ItemGetTextMesh, Player.Instance.transform.position, Quaternion.identity ) as GameObject;
		collectText.GetComponent<ItemGetText>().Initialize( "SPEED_BOOST!", Color.white );
	}

	IEnumerator GameFX(Player p)
	{
		((Player)p).m_runSpeed *= m_speedBoost;
		yield return new WaitForSeconds (m_duration);
		((Player)p).m_runSpeed /= m_speedBoost;
		Player.Instance.PlayAnimation("run");
	}
	
	protected override void CollectReal(Player player)
	{
		m_realSpeedPrePickup = player.m_runSpeed;
		StartCoroutine("RealFX", player);

		GameObject collectText = GameObject.Instantiate( m_ItemGetTextMesh, Player.Instance.transform.position, Quaternion.identity ) as GameObject;
		collectText.GetComponent<ItemGetText>().Initialize( "STEROIDS +1!", Color.white );
		
		Player.Instance.NumCollectedSteroids++;
	}

	IEnumerator RealFX(Player p)
	{
		((Player)p).m_runSpeed = 0;
		Player.Instance.PlayAnimation("pickup");
		
		((Player)p).IsStunned = true;
		yield return new WaitForSeconds (m_realStopTime);
		((Player)p).IsStunned = false;
		((Player)p).CheckControlDown();

		((Player)p).m_runSpeed = m_realSpeedPrePickup * 1.5f;

		Player.Instance.PlayAnimation("run");
	}
}