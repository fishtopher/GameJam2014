using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour 
{
	public float m_laneWidth = 0.64f;
	public float m_laneSpacing = 0.32f;
	public int m_numLanes = 3;
	public int m_numItems = 100;
	public float m_firstItem = 4;
	public VMath.FloatRange m_itemSpacing = new VMath.FloatRange(1, 2);

	public GameObject[] m_itemPrefabs;


	// Use this for initialization
	void Start () 
	{
		float xpos = m_firstItem;
		float yMin = -(m_laneWidth + m_laneSpacing) * ((float)m_numLanes-1) * 0.5f;
		for(int i = 0; i < m_numItems; i++)
		{
			xpos += m_itemSpacing.RandomValue;
			float ypos = yMin + Random.Range(0,m_numLanes) * (m_laneWidth + m_laneSpacing);
			Vector3	p = new Vector3(xpos, ypos, 0);
			GameObject go = Util.InstantiatePrefab( m_itemPrefabs[ Random.Range(0, m_itemPrefabs.Length) ], p); 
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
