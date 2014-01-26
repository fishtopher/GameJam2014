using UnityEngine;
using System.Collections;

public class Mushroom : Item 
{
	private float m_realStopTime = 1.5f;
	float m_realSpeedPrePickup;

	protected override void CollectGame(Player player)
	{
		Player.Instance.transform.localScale = Vector3.one * 2;

		SoundManager.PlaySound("powerup");

		GameObject collectText = GameObject.Instantiate( m_ItemGetTextMesh, Player.Instance.transform.position, Quaternion.identity ) as GameObject;
		collectText.GetComponent<ItemGetText>().Initialize( "SUPER POWER!", Color.white );
	}
	
	protected override void CollectReal(Player player)
	{
		GameObject collectText = GameObject.Instantiate( m_ItemGetTextMesh, Player.Instance.transform.position, Quaternion.identity ) as GameObject;
		collectText.GetComponent<ItemGetText>().Initialize( "HALLUCINATION!", Color.white );

		SoundManager.PlaySound("stun");

		m_realSpeedPrePickup = player.m_runSpeed;
		StartCoroutine("RealFX", player);
	}
	
	IEnumerator RealFX(Player p)
	{
		((Player)p).m_runSpeed = 0;
		Player.Instance.GetComponent<Animator>().Play("stun");

		((Player)p).IsStunned = true;
		yield return new WaitForSeconds (m_realStopTime);
		((Player)p).IsStunned = false;
		((Player)p).CheckControlDown();

		((Player)p).m_runSpeed = m_realSpeedPrePickup;
		Player.Instance.GetComponent<Animator>().Play("run");
	}
}
