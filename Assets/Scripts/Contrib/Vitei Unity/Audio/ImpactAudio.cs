using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class ImpactAudio : MonoBehaviour 
{
	AudioSource m_audioSource;
	public AudioClip[] m_audioClips;

	// range.Min = volume 0, range.max = volume 1
	public VMath.FloatRange m_speedRange;
	public VMath.FloatRange m_angVelRange;

	public float m_pitchVariation = 0;

	// Use this for initialization
	void Start () 
	{
		m_audioSource = GetComponent<AudioSource>();
	}

	
	void OnCollisionEnter(Collision col)
	{
		if (m_audioSource && !m_audioSource.isPlaying && m_audioClips.Length > 0)
		{
//			printf.PrintPersistentMessage("LV: " +  rigidbody.velocity.magnitude );
//			printf.PrintPersistentMessage(" AV: " +  rigidbody.angularVelocity.magnitude );

			float vol = Mathf.Max(m_speedRange.Normalize( rigidbody.velocity.magnitude ), m_angVelRange.Normalize( rigidbody.angularVelocity.magnitude ));
	
			m_audioSource.PlayOneShot(m_audioClips[Random.Range(0, m_audioClips.Length)]);
			m_audioSource.pitch = 1 + Random.Range(-m_pitchVariation, m_pitchVariation);
			m_audioSource.volume = vol * AudioControl.ms_fxVolume;
		}
	}
}
