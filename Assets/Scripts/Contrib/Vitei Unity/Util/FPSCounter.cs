using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// From: http://wiki.unity3d.com/index.php?title=FramesPerSecond
public class FPSCounter : MonoBehaviour 
{
 
// Attach this to a GUIText to make a frames/second indicator.
//
// It calculates frames/second over each updateInterval,
// so the display does not keep changing wildly.
//
// It is also fairly accurate at very low FPS counts (<10).
// We do this not by simply counting frames per interval, but
// by accumulating FPS for each frame. This way we end up with
// correct overall FPS even if the interval renders something like
// 5.5 frames.
public bool m_printfFPS = true;
	
public float updateInterval = 0.5F;
public Color goodTextColour = Color.white;
public Color sketchyTextColour = Color.yellow;
public Color badTextColour = Color.red; 
 
private float accum   = 0; // FPS accumulated over the interval
private int   frames  = 0; // Frames drawn over the interval
private float timeleft; // Left time for current interval
	 
public GameObject[] m_warningObjects;
public List<TextMesh> m_warningTextObjects;
public Color goodObjColour = new Color (0,0,0,0);
public Color sketchyObjColour = Color.yellow;
public Color badObjColour = Color.red; 
		
	void Start()
	{	
	    timeleft = updateInterval;  
	
		if(m_warningObjects.Length == 0)
		{
			m_warningObjects = GameObject.FindGameObjectsWithTag ("FPSWarning");	
		}
	
		m_warningTextObjects = new List<TextMesh>();
		for(int i = 0; i < m_warningObjects.Length; i++)
		{
			TextMesh tm = m_warningObjects[i].GetComponent<TextMesh>();
			if(tm!=null)
				m_warningTextObjects.Add (tm);
		}
		
		
	}
	 
	void Update()
	{
	    timeleft -= Time.deltaTime;
	    accum += Time.timeScale/Time.deltaTime;
	    ++frames;
	 
		float fps = accum/frames;
	    
		// Interval ended - update GUI text and start new interval
	    if( timeleft <= 0.0 )
	    {
			if(guiText != null)
			{
			    // display two fractional digits (f2 format)
				string format = System.String.Format("{0:F2} FPS",fps);
				guiText.text = format;
			 
				if(fps < 10)
					guiText.material.color = badTextColour;
				else if(fps < 30)
					guiText.material.color = sketchyTextColour;
				else 
					guiText.material.color = goodTextColour;
			}
			for(int i =0; i < m_warningObjects.Length; i++)
			{
				if(fps < 30)
					m_warningObjects[i].renderer.material.color = badObjColour;
				else if(fps < 59.5f)
					m_warningObjects[i].renderer.material.color = sketchyObjColour;
				else 
					m_warningObjects[i].renderer.material.color = goodObjColour;
			}
			for(int i = 0; i < m_warningTextObjects.Count; i++)
			{
				m_warningTextObjects[i].text = System.String.Format("{0:0}",fps);
			}
			
		
					
	        timeleft = updateInterval;
	        accum = 0.0F;
	        frames = 0;
	    }
		
		if(m_printfFPS)
		    printf.Print(System.String.Format("{0:F2} FPS",fps));
	}
}