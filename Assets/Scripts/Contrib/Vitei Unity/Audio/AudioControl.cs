using UnityEngine;
using System.Collections;

public class AudioControl : MonoBehaviour 
{
	public static float ms_musicVolume = 1;
	public static float ms_fxVolume = 1;
	public static float ms_phoneVolume = 1;
	
	public float m_fxVolume = 1;
	public float m_musicVolume = 1;
	public float m_phoneVolume = 1;

	void Update()
	{
		ms_fxVolume = m_fxVolume;
		ms_musicVolume = m_musicVolume;
		ms_phoneVolume = m_phoneVolume;
	}

}
