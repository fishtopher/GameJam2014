using UnityEngine;
using System.Collections;

public class Appearer : MonoBehaviour {
	
	public enum Tween{Linear, Sin, Bounce};
	public Tween m_tween = Tween.Linear;
	public float m_duration = 1.0f;
	
	StateMachine m_sm;
	public enum State { Appear, Visible, Disappear, Hidden };
	public State m_initialState = State.Appear;
	public float m_timer = 0;
	public float m_initialTimer = 0;
	float m_progress = 0;
	Vector3 m_targetScale;
	
	public void Appear(float delay = 0) { m_sm.State = (int)State.Appear; m_timer = delay; }
	public void Disappear(float delay = 0) { m_sm.State = (int)State.Disappear; m_timer = m_duration + delay; }
	public void Hide() { m_sm.State = (int)State.Hidden; m_timer = 0; m_progress = 0; transform.localScale = m_targetScale * 0; }
	public bool Visible { get { return m_sm.State == (int)State.Appear || m_sm.State == (int)State.Visible; } }
	
	
	// Use this for initialization
	void Awake () 
	{
		m_initialTimer = m_timer;

		m_targetScale = transform.localScale;
		
		m_sm = new StateMachine(typeof(State));
		m_sm.AddState((int)State.Appear, UpdateAppear);
		m_sm.AddState((int)State.Visible, UpdateVisible);
		m_sm.AddState((int)State.Disappear, UpdateDisappear);
		m_sm.AddState((int)State.Hidden, UpdateHidden);
		m_sm.State = (int)m_initialState;	
	}

	void OnEnable()
	{
		m_timer = m_initialTimer;
		transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
		m_initialState = State.Appear;
	}

	// Update is called once per frame
	void Update () 
	{
		m_sm.Update();
	
		float scaler;
		if(m_tween == Tween.Bounce)
			scaler = VMath.lerpSinBounce(0, 1, m_progress);
		else if(m_tween == Tween.Sin)
			scaler = VMath.lerpSin(0, 1, m_progress);
		else
			scaler = m_progress;
	
		transform.localScale = m_targetScale * scaler;
	}
	
	void UpdateAppear()
	{
		m_timer += Clock.dt;
		float t = Mathf.Clamp(m_timer, 0, m_duration);
		m_progress = t / m_duration;
		
		if(m_timer >= m_duration)
			m_sm.State = (int)State.Visible;
	}
	
	void UpdateVisible()
	{
		
	}
	
	void UpdateDisappear()
	{
		m_timer  -= Clock.dt;
		float t = Mathf.Clamp(m_timer, 0, m_duration);
		m_progress = t / m_duration;
		
		if(m_timer <= 0)
			m_sm.State = (int)State.Hidden;
	}
	
	void UpdateHidden()
	{
		
	}
}
