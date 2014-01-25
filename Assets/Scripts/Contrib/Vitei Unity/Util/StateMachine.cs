using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Super simple state machine.
// I'm not certain if this is actually going to be good, so am going to try it out a bit.
// It's a bit ugly in some ways e.g. it's state is an int to refer to an external enum :/
//  It would be best if it could be class StateMachine<MyStateEnum>, but it can't, see: http://msmvps.com/blogs/jon_skeet/archive/2009/09/10/generic-constraints-for-enums-and-delegates.aspx
public class StateMachine
{
	public delegate void StateDelegate();
	StateDelegate[] m_enterFns;
	StateDelegate[] m_updateFns;
	StateDelegate[] m_exitFns;
	
	int m_currentState;
	int m_nextState;
	public int State { get {return m_nextState; } set { m_nextState = value; } }

	public float StateTimer {get; private set;}
	
	public StateMachine( System.Type stateEnum )
	{
		int numStates = System.Enum.GetNames( stateEnum ).Length;
		m_enterFns  = new StateDelegate[numStates];
		m_updateFns = new StateDelegate[numStates];
		m_exitFns   = new StateDelegate[numStates];
		State = 0;

		StateTimer = 0;
	}
	
	public void AddState(int state, StateDelegate updateDelegate, StateDelegate enterDelegate = null, StateDelegate exitDelegate = null)
	{
		m_enterFns[state] = enterDelegate;
		m_updateFns[state] = updateDelegate;
		m_exitFns[state] = exitDelegate;
	}
	
	// Update is called once per frame
	public void Update ()
	{
		StateTimer+= Time.deltaTime;

		// Excecute any transition functions that exist
		if(m_nextState != m_currentState)
		{
			if(m_exitFns[m_currentState] != null)
				m_exitFns[m_currentState]();
			
			m_currentState = m_nextState;
			StateTimer = 0;
			
			if(m_enterFns[m_currentState] != null)
				m_enterFns[m_currentState]();
		}
		
		m_updateFns[m_currentState]();
	}
}
