using UnityEngine;
using System.Collections;

// Make sure this dude is first in "Script Excecution Order"
public class Clock : MonoBehaviour 
{
	public static float timeScale 
	{
		get { return Time.timeScale; }
		set { Time.timeScale = value; }
	}

	public static float unscaledDt = 0;
	public static float dt { get { return unscaledDt * Time.timeScale; } }

	float m_lastTime = 0;

	void Awake()
	{
		m_lastTime = Time.realtimeSinceStartup;
		unscaledDt = 0;
	}

	void Update()
	{
		float theDt = (Time.realtimeSinceStartup - m_lastTime);
		unscaledDt = theDt > 0 ? theDt : 0;
		m_lastTime = Time.realtimeSinceStartup;
	}
	
	
}
