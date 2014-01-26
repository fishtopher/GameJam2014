using UnityEngine;
using System.Collections;

public class Hallucinate : MonoBehaviour {

	Vignetting m_vignette;
	VortexEffect[] m_vortex;

	public float m_decayRate = 0.33f;

	public float m_vortexAngleStep = 75;
	float m_vortexAngle = 0;
	float m_vortexAngleTarget = 0;

	public float m_abberationStep = 100;
	float m_abberation = 200;
	float m_abberationTarget = 0;

	float m_timer;

	void Awake()
	{
		m_vignette = GetComponent<Vignetting>();
		m_vortex = GetComponentsInChildren<VortexEffect>();
	}

	// Use this for initialization
	public void Reset () 
	{
		m_timer = Random.Range(0, 5000.0f);
		m_vortexAngle = 0;
		m_vortexAngleTarget = 0;
		m_abberation = 0;
		m_abberationTarget = 0;
	}

	public void StepUp()
	{
		m_vortexAngleTarget += m_vortexAngleStep;
		m_abberationTarget += m_abberationStep;
	}
	
	// Update is called once per frame
	void Update () 
	{
		m_timer += Clock.dt * 0.333f;
		m_vortexAngle = Mathf.Lerp(m_vortexAngle, m_vortexAngleTarget, Clock.dt);

		for(int i = 0; i < m_vortex.Length; i++)
		{
			m_vortex[i].angle = Mathf.Sin(m_timer * i) * m_vortexAngle; 
			m_vortex[i].center =
				(new Vector2(Mathf.Sin(3 * m_timer * i) * Mathf.Cos(5 * m_timer * i), Mathf.Sin(7 * m_timer * i) * Mathf.Cos(2 * m_timer * i)) * 0.5f ) + Vector2.one * 0.5f;
		}


		m_abberation = Mathf.Lerp(m_abberation, m_abberationTarget, Clock.dt);

		m_vignette.chromaticAberration = Mathf.Cos(m_timer) * m_abberation;
		m_vignette.blurSpread = (m_abberation/m_abberationStep);
		m_vignette.intensity  = (m_abberation/m_abberationStep) * 3;

		m_vortexAngleTarget = Mathf.Lerp(m_vortexAngleTarget, 0, Clock.dt * m_decayRate);
		m_abberationTarget = Mathf.Lerp(m_abberationTarget, 0, Clock.dt * m_decayRate);
		
	}
}
