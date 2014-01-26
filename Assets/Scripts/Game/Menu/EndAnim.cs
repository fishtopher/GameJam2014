using UnityEngine;
using System.Collections;

public class EndAnim : MonoBehaviour 
{
	public void PlaySound()
	{
		SoundManager.PlaySound("countdown");
	}
}
