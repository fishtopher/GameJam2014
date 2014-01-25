using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class printf : MonoBehaviour
{
    static ArrayList m_lines = new ArrayList(24);
    Texture2D m_whiteTex;
    GUIStyle m_guiStyle;
    Font m_monoFont;
	static TextMesh m_linesMesh;
	static TextMesh m_messagesMesh;
	
    public bool messagesAtTop = false;

    static GameObject ms_instanceObject = null;
    static void Instantiate()
    {
        if (ms_instanceObject == null)
        {
            ms_instanceObject = new GameObject("printf");
            ms_instanceObject.AddComponent<printf>();
			
			GameObject go = GameObject.Find("PrintfText");
        	if(go != null)
			{
				m_linesMesh = go.GetComponent<TextMesh>();
			}
			GameObject pm = GameObject.Find("PrintfMessages");
        	if(pm != null)
			{
				m_messagesMesh = pm.GetComponent<TextMesh>();
			}
		}
    }

    // Use this for initialization
    void Start()
    {
//        Debug.Log(">> printf is attatched to " + transform.GetPath());
        if (ms_instanceObject == null)
        {
            ms_instanceObject = gameObject;
        }

        m_whiteTex = new Texture2D(4, 4);

        for (int y = 0; y < m_whiteTex.height; ++y)
        {
            for (int x = 0; x < m_whiteTex.width; ++x)
            {
                m_whiteTex.SetPixel(x, y, Color.white);
            }
        }
        m_whiteTex.Apply();

        m_guiStyle = new GUIStyle();
        m_monoFont = (Font)Resources.Load("Fonts/Consolas");
        if (m_monoFont != null)
        {
            m_guiStyle.font = m_monoFont;
        }
		
		StartCoroutine(ClearAtEndOfFrame());
    }
	
	
    IEnumerator ClearAtEndOfFrame()
	{	
		while(true)
		{	
			yield return new WaitForEndOfFrame();
            m_lines.Clear();
			m_positionalMessages.Clear();
		};
	}

    public static void Print(string s)
    {
        Instantiate();
		string[] lines = s.Split('\n');
		for(int i = 0; i < lines.Length; i++)
	        m_lines.Add(lines[i]);
    }
	
	
	
	//--------------------------------------------------------
    class PersistentMessage
    {
        public string str;
        public float timer;
    }
    static List<PersistentMessage> messages = new List<PersistentMessage>();

    public static void PrintPersistentMessageNoTimeStamp(string s, float time = 8.0f)
    {
        Instantiate();
		
		string[] lines = s.Split('\n');
		for(int i = 0; i < lines.Length; i++)
		{
	        PersistentMessage p = new PersistentMessage();
	        p.str = lines[i];
	        p.timer = time;
	        messages.Add(p);
		}
    }

    static int msgNum = 0;
    public static void PrintPersistentMessage(string s, float time = 8.0f)
    {
        Instantiate();
		
		string[] lines = s.Split('\n');
		for(int i = 0; i < lines.Length; i++)
		{
	        PersistentMessage p = new PersistentMessage();
	        //p.str = DateTime.Now.ToString("H:mm:ss") + ": " + s;
	        p.str = string.Format("{0:0000}: {1}", (msgNum++), lines[i]);
	        p.timer = time;
	        messages.Add(p);
		}
    }
	
	
	//--------------------------------------------------------
	class PositionalMessage
	{
		public string str;
		public Vector2 pos;
	}
	
    static List<PositionalMessage> m_positionalMessages = new List<PositionalMessage>();
	public static void Print(String s, Vector2 p)
	{
        Instantiate();
        PositionalMessage pm = new PositionalMessage();
        pm.str = s;
		
		p.y = Screen.height - p.y;
        pm.pos = p;
        m_positionalMessages.Add(pm);
	}
	
	
	
	//--------------------------------------------------------
	// Update is called once per frame
    void Update()
    {
        for (int i = messages.Count - 1; i >= 0; i--)
        {
            messages[i].timer -= Time.deltaTime;
            if (messages[i].timer < 0)
                messages.RemoveAt(i);
        }
    }

    void OnGUI()
    {
        float w = 256;
        float h = 18;
        float x = 8;
        float y = 8;
        float ls = h;

        Color bgc = new Color(0, 0, 0, 0.55f);
        Color fgc = new Color(1, 1, 1, 0.95f);
        Color fgcm = new Color(1, 1, 0, 0.95f);
        m_guiStyle.normal.textColor = fgc;

        Rect rb = new Rect(x - 2, y - 2, w, h - 1);
        Rect rf = new Rect(x, y, w, h);
		
		string allLines = "";
		string allMessages = "";
		
        if (messagesAtTop)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                GUIContent str = new GUIContent((string)messages[i].str);
				allMessages += (messages[i].str + "\n");

                // Work out how big to make the background 
                Vector2 sz = m_guiStyle.CalcSize(str);
                rb.width = sz.x + 6;
                rf.width = sz.x + 6;

                // Draw Background
                GUI.color = bgc;
                GUI.DrawTexture(rb, m_whiteTex);

                // Draw Text
                GUI.color = fgcm;
                GUI.Label(rf, str, m_guiStyle);

                // Move to next line
                rb.y += ls;
                rf.y += ls;
            }
        }
        else
        {
            rb = new Rect(x - 2, Screen.height - y - 2, w, h - 1);
            rf = new Rect(x, Screen.height - y, w, h);

            for (int i = 0; i < messages.Count; i++)
            {
                GUIContent str = new GUIContent((string)messages[i].str);
				allMessages += (messages[i].str + "\n");

                // Work out how big to make the background 
                Vector2 sz = m_guiStyle.CalcSize(str);
                rb.width = sz.x + 6;
                rf.width = sz.x + 6;

                rb.y -= sz.y;
                rf.y -= sz.y;

                // Draw Background
                GUI.color = bgc;
                GUI.DrawTexture(rb, m_whiteTex);

                // Draw Text
                GUI.color = fgcm;
                GUI.Label(rf, str, m_guiStyle);
            }

            rb = new Rect(x - 2, y - 2, w, h - 1);
            rf = new Rect(x, y, w, h);
        }
		
		if(m_messagesMesh != null)
		{
			m_messagesMesh.text = allMessages;
		}


        for (int i = 0; i < m_lines.Count; i++)
        {
            GUIContent str = new GUIContent((string)m_lines[i]);
			allLines += (m_lines[i] + "\n");

            // Work out how big to make the background 
            Vector2 sz = m_guiStyle.CalcSize(str);
            rb.width = sz.x + 6;
            rf.width = sz.x + 6;

            // Draw Background
            GUI.color = bgc;
            GUI.DrawTexture(rb, m_whiteTex);

            // Draw Text
            GUI.color = fgc;
            GUI.Label(rf, str, m_guiStyle);

            // Move to next line
            rb.y += ls;
            rf.y += ls;
        }
		
		for (int i = 0; i < m_positionalMessages.Count; i++)
        {
            GUIContent str = new GUIContent(m_positionalMessages[i].str);
			
			rb.x = m_positionalMessages[i].pos.x-2;
			rb.y = m_positionalMessages[i].pos.y-1;
			rf.x = m_positionalMessages[i].pos.x;
			rf.y = m_positionalMessages[i].pos.y;

            // Work out how big to make the background 
            Vector2 sz = m_guiStyle.CalcSize(str);
            rb.width = sz.x + 6;
            rf.width = sz.x + 6;

            // Draw Background
            GUI.color = bgc;
            GUI.DrawTexture(rb, m_whiteTex);

            // Draw Text
            GUI.color = fgc;
            GUI.Label(rf, str, m_guiStyle);
        }
		
		if(m_linesMesh != null)
		{
			m_linesMesh.text = allLines;
		}
    }
}
