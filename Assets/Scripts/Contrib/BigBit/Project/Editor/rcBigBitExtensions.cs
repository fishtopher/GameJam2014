using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class rcBigBitExtensions
{
	
    //---------------------------------------------------------
	
	[MenuItem("Scene Layers/Bake All Levels")]
    public static void BuildAppDebugiOS()
    {
        // Check if loaded project has any baked levels in the scene list (ie. XXX_Baked.unity)
        // Extract level names from scene list and "bake" before kicking off build
        string[] bakedLevels = GetBakedLevelList();
        rcMapLoadSave.BakeLevels(bakedLevels);
    }

    public static string[] GetBakedLevelList()
    {
        List<string> levelsToBuild = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            //Debug.Log(scene.path);

            if (scene.enabled)
            {
                // Extract name of baked levels
                string filename = Path.GetFileNameWithoutExtension(scene.path);
                if (filename.EndsWith("_Baked"))
                {
                    filename = filename.Substring(0, filename.IndexOf("_Baked"));
                    levelsToBuild.Add(filename);

                    Debug.Log("Found baked level: " + filename);
                }
            }
        }

        return levelsToBuild.ToArray();
    }	
	
    //---------------------------------------------------------

    [MenuItem("Scene Layers/Debug/Toggle MapData")]
    static void ToggleMapData()
    {
        GameObject mapData = GameObject.Find(MapData.gameObjectName);
        if (mapData != null)
        {
            if (mapData.hideFlags == HideFlags.HideAndDontSave)
                mapData.hideFlags = HideFlags.DontSave;
            else if (mapData.hideFlags == HideFlags.DontSave)
                mapData.hideFlags = HideFlags.HideAndDontSave;

            // Bit shit but setting it dirty doesn't force hierarchy window to update. 
            EditorUtility.SetDirty(mapData);
            //EditorApplication.RepaintHierarchyWindow();

            // HACK: Force update by toggling active flag.
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 // _fix_Unity4_
            mapData.SetActive(false);
            mapData.SetActive(true);
#else
            mapData.active = false;
            mapData.active = true;
#endif
        }
    }

    [MenuItem("Scene Layers/Debug/Toggle MapData", true)]
    static bool ToggleMapDataValidate()
    {
        return MapData.GetInstance() != null;
    }

    //---------------------------------------------------------

    [MenuItem("Scene Layers/Debug/Show All Layers %#a")]
    static void ShowAll()
    {
        // NOTE: Find() methods do _NOT_ return inactive objects(!)
        GameObject[] gameObjects = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
        ShowAll(gameObjects, "Layer_");
    }

    [MenuItem("Scene Layers/Debug/Show Gameplay Layer")]
    static void ShowGameplayLayer()
    {
        // NOTE: Find() methods do _NOT_ return inactive objects(!)
        GameObject[] gameObjects = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
        ShowAll(gameObjects, "Layer_Gameplay");
    }

    [MenuItem("Scene Layers/Debug/Show All GameObjects &#a")]
    static void ShowAllGameObjects()
    {
        GameObject[] gameObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
        ShowAll(gameObjects, null);
    }

    // Helper for showing game objects
    static void ShowAll(GameObject[] gameObjects, string rootPrefix)
    {
        foreach (var gameObj in gameObjects)
        {
            if (gameObj.hideFlags != 0 && (rootPrefix == null || gameObj.transform.root.name.StartsWith(rootPrefix)))
            {
                gameObj.hideFlags = 0;
                EditorUtility.SetDirty(gameObj);
            }
        }
    }
	
	//---------------------------------------------------------
	
    [MenuItem("Window/Scene Layers")]
    static void ShowMapView()
    {
        EditorWindow.GetWindow(typeof(rcMapView));
    }
}
