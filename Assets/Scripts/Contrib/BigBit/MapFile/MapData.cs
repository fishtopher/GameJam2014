using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode()]
public class MapData : MonoBehaviour 
{
#if UNITY_STANDALONE_WIN
    public static String vcsRoot = @"\\Horace\NAS3\MapVCS\";
#else
    public static String vcsRoot = "/Volumes/NAS3/MapVCS/";
#endif

    public static String projectName = "";
    public static String userName = Environment.UserName;
	
	public const String gameObjectName = "_MapData";
    public static MapData mapData;
	
	public string levelName;
	public MapLayer[] mapLayers;
	
    public static string savedLevelName;

	// Set the level and layers data from the loaded map file
	public void SetLevel(String level, MapLayer[] layers)
	{
		levelName = level;
		mapLayers = layers;
	}
	
	// Instance creation and access
	public static MapData CreateInstance()
	{
		GameObject go = GameObject.Find(gameObjectName);
		if(go == null)
		{
			go = new GameObject(gameObjectName);
			go.AddComponent<MapData>();
			go.hideFlags = HideFlags.HideAndDontSave; 
			// NOTE: DontSave is effectively same as DontDestroyOnLoad which means 
			// this game object will hang around after a new scene is loaded.

            savedLevelName = null;
		}
		return go.GetComponent<MapData>();
	}
	
	public static MapData GetInstance()
	{
		if(mapData == null)
		{
			GameObject go = GameObject.Find(gameObjectName);
			if(go != null)
			{
				mapData = go.GetComponent<MapData>();
			}
		}
		
		return mapData;
	}
	
	// Path helpers
	public static String GetVcsPath(String layerName)
	{
		String lockfile = vcsRoot + projectName + "/" + GetInstance().levelName + "/" + layerName + ".txt";
		return lockfile;
	}

    public static String GetVcsRoot()
    {
        String lockfileDir = vcsRoot + projectName + "/";
        return lockfileDir;
    }
	
	public static void Log(String prefix, String layerName, String user)
	{
		String msg = prefix + GetInstance().levelName + "/" + layerName + " (" + user + ")";
		
		Debug.Log(msg);
	}

    public static void SetProjectName(String project)
    {
        projectName = project;
    }
	
#if UNITY_EDITOR

    private bool bInPlayMode = false;

    void Update()
    {
        // Need to 'delete' ourselves when the user loads a new non-map file scene
        // ie. File->Open Scene.. or double-clicking on scene in the window
        if (mapLayers != null)
        {
            bool sceneChanged = false;
            foreach (MapLayer mapLayer in mapLayers)
            {
                if (!mapLayer.IsRootValid())
                {
                    sceneChanged = true;
                    break;
                }
            }

            if (sceneChanged)
            {
                // If in play mode we are transitioning between levels in-game so 
                // set sentinel so we know to reload map file when application quits
                if (Application.isPlaying && savedLevelName == null)
                {
                    savedLevelName = levelName;
                }

                Debug.Log("MapData: Detected Scene Change (destroying MapData)");
                UnityEngine.Object.DestroyImmediate(this.gameObject);
            }
        }

        if (Application.isPlaying)
        {
            if (!bInPlayMode)
            {
                Debug.Log("Started Play Mode");

                // Initialize sentinel to track if map data gets was unloaded whilst in play mode
                savedLevelName = null;
            }
            bInPlayMode = true;
        }
        else
        {
            if (bInPlayMode)
            {
                Debug.Log("Stopped Play Mode");
            }
            bInPlayMode = false;
        }
    }

#endif

    /*
	void Awake()
	{
        Debug.Log("Awake");
	}
	
    
	void Start()
	{
        Debug.Log("Start");
	}
    
	void OnEnable()
	{
		Debug.Log("OnEnable");
	}
	
	void OnDisable()
	{
		Debug.Log("OnDisable");
	}
    
    void OnLevelWasLoaded(int level) 
	{
        Debug.Log("OnLevelWasLoaded");
    }
    */ 
}

