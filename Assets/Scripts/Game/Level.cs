using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level : MonoBehaviour 
{
	public Item.Type m_universe = Item.Type.Game;

	public float m_laneWidth = 0.7f;
	public float m_laneHeight = 0.7f;
	public float m_laneSpacing = 0.32f;
	public int m_numLanes = 3;
	public int m_numItems = 100;
	public float m_firstItem = 4;
	public VMath.FloatRange m_itemSpacing = new VMath.FloatRange(1, 2);

	public GameObject[] m_itemPrefabs;
	public GameObject m_bgStartPrefab;
	public GameObject[] m_bgPrefabs;
	public GameObject m_bgEndPrefab;

	public GameObject m_endScene;

	public GameObject[] m_items;
	public List<GameObject> m_bgTiles;

	public Renderer m_sky;
	public Renderer m_grass;
	public Color m_skyGameColour = new Color(41/255.0f, 147/255.0f, 221/255.0f);
	public Color m_grassGameColour = new Color (52/255.0f, 206/255.0f, 72/255.0f);
	public Color m_skyRealColour = new Color(41/255.0f, 147/255.0f, 221/255.0f);
	public Color m_grassRealColour = new Color (52/255.0f, 206/255.0f, 72/255.0f);

	//
	float m_endPos;

	/*public float LaneYPos(int laneId)
	{
		float yMin = -(m_laneWidth + m_laneSpacing) * ((float)m_numLanes-1) * 0.5f;
		return yMin + laneId * (m_laneWidth + m_laneSpacing);
	}*/


	int timesFinished;


	//
	public void Reset()
	{
		DestroyLevel();
		GenerateLevel();

		Player p = GameObject.FindObjectOfType<Player>() as Player;

		if(p)
		{
			p.Reset();
		}
	}

	// Use this for initialization
	void Start () 
	{
		Reset();
	}

	void DestroyLevel()
	{
		if(m_items != null)
		{
			for(int i = 0; i < m_items.Length; i++)
			{
				GameObject.Destroy( m_items[i] );
			}
			m_items = null;
		}

		if(m_bgTiles != null)
		{
			for(int i = m_bgTiles.Count-1; i >= 0; i--)
			{
				GameObject go = m_bgTiles[i];
				GameObject.Destroy( go );
			}
			m_bgTiles.Clear();
			m_bgTiles = null;
		}
	}

	void GenerateLevel()
	{
		if(m_universe == Item.Type.Game)
		{
			m_grass.material.color = m_grassGameColour;
			m_sky.material.color = m_skyGameColour;
		}
		else
		{
			m_grass.material.color = m_grassRealColour;
			m_sky.material.color = m_skyRealColour;
		}


		GameObject go;
		float itemX = m_firstItem * m_laneWidth;
		float itemZ = 0.01f;
		m_items = new GameObject[m_numItems];
		for(int i = 0; i < m_numItems; i++)
		{
			itemX += m_itemSpacing.RandomValue;
			//float ypos = LaneYPos(Random.Range(0,m_numLanes));
			float ypos = Random.Range(0,m_numLanes) * m_laneHeight + m_laneHeight * -( Mathf.Ceil( (float) m_numLanes / 2 ) - 1);

			Vector3	p = new Vector3(itemX, ypos, itemZ);
			m_items[i] = Util.InstantiatePrefab( m_itemPrefabs[ Random.Range(0, m_itemPrefabs.Length) ], p);
			Item item = m_items[i].GetComponent<Item>();
			item.MyType = m_universe;
		}
		
		m_bgTiles = new List<GameObject>();

		float bgY = m_laneHeight * -( Mathf.Ceil( (float) m_numLanes / 2 ) - 1);

		for ( int i = 0; i < m_numLanes; ++i )
		{
			float bgX = 0;
			go = Util.InstantiatePrefab( m_bgStartPrefab,  new Vector3(0, bgY, 2)); 
			go.GetComponent<DualWorldGFX>().MyType = m_universe;
			m_bgTiles.Add(go);

			bgX = go.transform.localScale.x;

			while(bgX < (itemX + 5))
			{
				go = Util.InstantiatePrefab( m_bgPrefabs[ Random.Range(0, m_bgPrefabs.Length) ],  new Vector3(bgX, bgY, 2)); 
				m_bgTiles.Add(go);
				go.GetComponent<DualWorldGFX>().MyType = m_universe;
				bgX += go.transform.localScale.x;
			}

			go = Util.InstantiatePrefab( m_bgEndPrefab,  new Vector3(bgX, bgY, 2));
			m_bgTiles.Add(go);
			go.GetComponent<DualWorldGFX>().MyType = m_universe;
			m_endPos = bgX;

			bgY += m_laneHeight;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if(Player.Instance.transform.position.x > m_endPos)
		{
			m_endScene.SetActive(true);
		}
	}
}
