using UnityEngine;
using System.Collections;

public class Speedboost : Item 
{
	float m_speedBoost = 3;
	float m_duration = 0.5f;

	protected override void CollectGame(Player player)
	{
		Player.Instance.GetComponent<Animator>().Play("fly");
		StartCoroutine("GameFX", player);

		GameObject collectText = GameObject.Instantiate( m_ItemGetTextMesh, Player.Instance.transform.position, Quaternion.identity ) as GameObject;
		collectText.GetComponent<ItemGetText>().Initialize( "SPEED_BOOST!", Color.white );
	}

	IEnumerator GameFX(Player p)
	{
		((Player)p).m_runSpeed *= m_speedBoost;
		yield return new WaitForSeconds (m_duration);
		((Player)p).m_runSpeed /= m_speedBoost;
		Player.Instance.GetComponent<Animator>().Play("run");
	}
	
	protected override void CollectReal(Player player)
	{
		GameObject collectText = GameObject.Instantiate( m_ItemGetTextMesh, Player.Instance.transform.position, Quaternion.identity ) as GameObject;
		collectText.GetComponent<ItemGetText>().Initialize( "STEROIDS +1!", Color.white );

		Player.Instance.NumCollectedSteroids++;

		StartCoroutine("GameFX", player);
	}
}