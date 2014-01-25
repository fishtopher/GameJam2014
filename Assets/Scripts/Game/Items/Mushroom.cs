using UnityEngine;
using System.Collections;

public class Mushroom : Item 
{
	protected override void CollectGame(Player player)
	{
		Player.Instance.transform.localScale = Vector3.one * 2;
	}
	
	protected override void CollectReal(Player player)
	{
		
	}
}
