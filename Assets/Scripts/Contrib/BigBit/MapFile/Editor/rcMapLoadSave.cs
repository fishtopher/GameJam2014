using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[InitializeOnLoad]
public partial class rcMapLoadSave
{
    static Hashtable hideFlagCache = new Hashtable();
    static bool bVerbose = false;

    static List<Transform> savedLayers = null;
    static List<Transform> savedNonLayers = null;

#pragma warning disable 0414
    static rcMapLoadSave instance = null;
#pragma warning restore 0414

    static rcMapLoadSave()
    {
        // Create singleton on init in order to add/remove update callback
        instance = new rcMapLoadSave();
    }

    rcMapLoadSave()
    {
        //Debug.Log("rcMapLoadSave() ctor");

        // Install update callback so we can reload the map file if the MapData gets destroyed 
        // during play mode as a side-effect of transitioning levels
        EditorApplication.update += ReloadMapFileUpdate;
    }

    ~rcMapLoadSave()
    {
        //Debug.Log("rcMapLoadSave() dtor");

        EditorApplication.update -= ReloadMapFileUpdate;
    }

    // Load the map file for the given level and add in the scene layers
    public static void LoadLevel(String levelName, bool loadNormalLayers = true, bool loadSourceLayers = false, bool loadDontBakeLayers = true, string[] skipLayersByName = null) 
    {
        // Create the lockfile directory if it doesn't exist
        String lockfileDir = MapData.GetVcsRoot() + levelName;
        if (!Directory.Exists(lockfileDir))
        {
            // was: Directory.CreateDirectory(lockfileDir);
            // Using a shell command because it's the easiest way to create a directory that has appropriate 
            //    user priveleges for everyone: File.GetAccessControl(...) fails on OSX
            rcShell.Execute("mkdir", "-p -m 777 " + lockfileDir);
        }

        // Load the layers
        ArrayList mapLayers = LoadLayers(levelName, loadNormalLayers, loadSourceLayers, loadDontBakeLayers, skipLayersByName);

        // Create the map data instance in the scene and set the loaded level and scene layers
        MapData mapData = MapData.CreateInstance();
        mapData.SetLevel(levelName, (MapLayer[])mapLayers.ToArray(typeof(MapLayer)));

        // Initialize and update map view
        // Note: All layers are currently visible.  
        // Only want to include layers in the hierarchy view that are checked out to the current user 
        rcMapView mapView = (rcMapView)EditorWindow.GetWindow(typeof(rcMapView));
        mapView.Refresh();
        mapView.Repaint();
        mapView.autoRepaintOnSceneChange = true;

        // Select the geometry layer so the camera is centered on some visible geometry
        Selection.activeGameObject = GameObject.Find("Layer_Geometry");
    }

    public static String GetLayersPath(String prefix, String levelName)
    {
        String scenePath = prefix + "/Scenes/" + levelName + "/Scene_Layers/";
        return scenePath;
    }

    public static String GetScenePath(String levelName, String suffix)
    {
        String scenePath = Application.dataPath + "/Scenes/" + levelName + "/" + levelName + suffix;
        return scenePath;
    }

    public static String GetProjectRelativeScenePath(String levelName, String suffix)
    {
        String scenePath = "Assets/Scenes/" + levelName + "/" + levelName + suffix;
        return scenePath;
    }

    public static void BakeLevels(String[] levelNames)
    {
        foreach (string level in levelNames)
        {
            BakeLevel(level);
        }
    }

    public static void BakeLevel(String levelName)
    {
        // Load the layers
        const bool loadNormalLayers = true;
        const bool loadSourceLayers = false;
        const bool loadAmbientVFXLayer = false;
        LoadLayers(levelName, loadNormalLayers, loadSourceLayers, loadAmbientVFXLayer, null);

        // Add game object w/ serialized RenderSettings so they are applied when XXX_Baked level
        // is loaded via LoadLevelAdditive()
        rcRenderSettingsFixup.Save();

        // Note: All layers are currently visible.  
        String bakedScene = GetProjectRelativeScenePath(levelName, "_Baked.unity");
		if(bVerbose)
			Debug.Log("Saving baked scene: " + bakedScene);
        EditorApplication.SaveScene(bakedScene);

        // Load baked level
        EditorApplication.OpenScene(bakedScene);
    }

    public static ArrayList LoadLayers(String levelName, bool loadNormalLayers, bool loadSourceLayers, bool loadDontBakeLayers, string[] skipLayersByName)
    {
        // Open master scene
        String masterScene = GetScenePath(levelName, "_Map.unity");
        EditorApplication.OpenScene(masterScene);

        //Debug.Log("Num Lightmaps in MapFile: " + LightmapSettings.lightmaps.Length);

        ArrayList mapLayers = new ArrayList();

        String levelPath = GetLayersPath(Application.dataPath, levelName);
        string[] files = Directory.GetFiles(levelPath);
        foreach (string file in files)
        {
            //Debug.Log(file);

            bool doLoad = file.EndsWith(".unity");

            // Only load Source layers if wanted
            if (file.Contains("Layer_Source_") && !loadSourceLayers)
                doLoad = false;

            // Only load Normal layers if wanted
            if (!file.Contains("Layer_Source_") && !loadNormalLayers)   // Currently a "normal" layer is any layer that isn't a "source" layer
                doLoad = false;

            // Only load "don't bake" layers if wanted
            if (file.Contains("_DontBake") && !loadDontBakeLayers)
                doLoad = false;

            // Skip layers that we don't want to load
            if (skipLayersByName != null)
            {
                foreach (var name in skipLayersByName)
                {
                    Debug.Log(name + " " + file);

                    if (file.Contains(name))
                        doLoad = false;
                }
            }

            if (doLoad)
            {
                string relativePath = file.Substring(file.IndexOf("/Assets/") + 1);

                if(bVerbose)
                    Debug.Log("Opening scene additively with layer: " + relativePath);
                EditorApplication.OpenSceneAdditive(relativePath);

                // Create a map layer for this scene and add to list
                string filename = Path.GetFileNameWithoutExtension(file);

                // Verify there is layer w/ the correct name
                GameObject root = GameObject.Find(filename);
                if (root == null)
                {
                    Debug.LogError("Could not find root game object named: " + filename);
                }

                // Check if the loaded layer is a proxy to a different one
                rcProxyLayer proxyLayer = root.GetComponent<rcProxyLayer>();
                if (proxyLayer != null && proxyLayer.LevelToProxyTo != string.Empty)
                {
                    // Construct a new file name to use
                    String proxyLevelPath = GetLayersPath(Application.dataPath, proxyLayer.LevelToProxyTo) + filename + ".unity";
                    String proxyRelativePath = proxyLevelPath.Substring(proxyLevelPath.IndexOf("/Assets/") + 1);

                    // Validate file
                    if (File.Exists(proxyLevelPath))
                    {
                        // Delete the old root game object with the proxy layer component on it
                        UnityEngine.Object.DestroyImmediate(root);
                        root = null;

                        if (bVerbose)
                            Debug.Log("Opening scene additively with proxy layer: " + proxyRelativePath);
                        EditorApplication.OpenSceneAdditive(proxyRelativePath);

                        // Verify the new one root game object
                        root = GameObject.Find(filename);
                        if (root == null)
                        {
                            Debug.LogError("Could not find proxy layer root game object named: " + filename);
                        }
                    }
                    else
                    {
                        Debug.LogError("Proxy layer file not found: " + proxyLevelPath);
                    }
                }

                MapLayer layer = new MapLayer(filename, root);
                mapLayers.Add(layer);
            }
        }

        //Debug.Log("Num Lightmaps post load: " + LightmapSettings.lightmaps.Length);

        // Restore static flags after loading
        SaveRestoreStaticFlags(false);

        return mapLayers;
    }

    public static void GetLayers(out List<Transform> layers, out List<Transform> nonLayers)
    {
        layers = new List<Transform>();
        nonLayers = new List<Transform>();

        Transform[] transforms = (Transform[])UnityEngine.Object.FindObjectsOfType(typeof(Transform));

        // NOTE: Find() methods do _NOT_ return inactive objects(!)
        foreach (Transform child in transforms)
        {
            if (child.parent == null)
            {
                if (child.gameObject.name.StartsWith("Layer_"))
                    layers.Add(child);
                else
                    nonLayers.Add(child);
            }
        }
    }

    public static void PreMapSave(string levelName)
    {
        hideFlagCache.Clear();

        savedLayers = null;
        savedNonLayers = null;
        GetLayers(out savedLayers, out savedNonLayers);

        // Hide the layers
        if(bVerbose)
            Debug.Log("PreSave: Hiding layers");
        foreach (Transform child in savedLayers)
        {
            SaveHideFlags(child.gameObject);
            SetHideFlags(child.gameObject, HideFlags.HideAndDontSave);
        }

        // Hide the non layers
        if (bVerbose)
            Debug.Log("PostSave: Hiding non-layers");
        foreach (Transform child in savedNonLayers)
        {
            SaveHideFlags(child.gameObject);
            SetHideFlags(child.gameObject, HideFlags.HideAndDontSave);
        }

        LightmapData[] emptyLightMaps = new LightmapData[0];

        // Now save the sub-scenes
        if (bVerbose)
            Debug.Log("PostSave: saving visible layers");
        foreach (Transform child in savedLayers)
        {
            // Only save visible layers
            HideFlags hideFlags = (HideFlags)hideFlagCache[child.gameObject];
            if (hideFlags == 0)
            {
                // Restore this layer node
                RestoreHideFlags(child.gameObject);

                String scenePath = GetLayersPath("Assets", levelName);
                scenePath += child.gameObject.name + ".unity";

                // Save static flags
                SaveStaticFlags(child.gameObject);
                
                // Save off map-file based lightmaps and clear the lightmaps[] so it 
                // doesn't get saved in the layers scene file
                LightmapData[] lightmaps = LightmapSettings.lightmaps;
                LightmapSettings.lightmaps = emptyLightMaps;

                if(bVerbose)
                    Debug.Log("Saving sub-scene: " + scenePath);
                EditorApplication.SaveScene(scenePath);

                // Restore lightmap[]
                LightmapSettings.lightmaps = lightmaps;

                // Restore static flags
                RestoreStaticFlags(child.gameObject);

                // Rehide this layer node
                SetHideFlags(child.gameObject, HideFlags.HideAndDontSave);
            }
        }

        // All Layers hidden again
        // Restore the non-layers and let unity perform the save
        if (bVerbose)
            Debug.Log("PostSave: restoring layers/non-layers");
        foreach (Transform child in savedNonLayers)
        {
            RestoreHideFlags(child.gameObject);
        }
    }

    public static void PostMapSave()
    {
        foreach (Transform child in savedLayers)
        {
            RestoreHideFlags(child.gameObject);
        }
    }

    // Recursively set the hide flags for the given game object 
    public static void SetHideFlags(GameObject gameObj, HideFlags hideFlags)
    {
        if (hideFlags != gameObj.hideFlags)
        {
            gameObj.hideFlags = hideFlags;
            EditorUtility.SetDirty(gameObj);
        }

        foreach (Transform child in gameObj.transform)
        {
            SetHideFlags(child.gameObject, hideFlags);
        }
    }

    // Recursively cache the hide flags for the given game object
    static void SaveHideFlags(GameObject gameObj)
    {
        SaveRestoreHideFlags(gameObj, true);
    }

    // Recursively restore the hide flags for the given game object
    static void RestoreHideFlags(GameObject gameObj)
    {
        SaveRestoreHideFlags(gameObj, false);
    }

    // Worker
    static void SaveRestoreHideFlags(GameObject gameObj, bool save)
    {
        if (save)
        {
            hideFlagCache.Add(gameObj, gameObj.hideFlags);
        }
        else
        {
            gameObj.hideFlags = (HideFlags)hideFlagCache[gameObj];
            //EditorUtility.SetDirty(gameObj);
        }

        foreach (Transform child in gameObj.transform)
        {
            SaveRestoreHideFlags(child.gameObject, save);
        }
    }

    public static void SaveRestoreStaticFlags(bool bSave)
    {
        Transform[] transforms = (Transform[])UnityEngine.Object.FindObjectsOfType(typeof(Transform));

        foreach (Transform child in transforms)
        {
            if (child.parent == null)
            {
                if (child.gameObject.name.StartsWith("Layer_"))
                {
                    if(bSave)
                        SaveStaticFlags(child.gameObject);
                    else
                        RestoreStaticFlags(child.gameObject);
                }
            }
        }
    }

    static void SaveStaticFlags(GameObject gameObj)
    {
        SaveRestoreStaticFlags(gameObj, true);
    }

    static void RestoreStaticFlags(GameObject gameObj)
    {
        SaveRestoreStaticFlags(gameObj, false);
    }

    static void SaveRestoreStaticFlags(GameObject gameObj, bool save)
    {
        const string staticSuffix = "__STATIC__";

        if (save)
        {
            if(gameObj.isStatic)
            {
                //Debug.Log("Saving static object: " + gameObj.name + " " + gameObj.isStatic);
                gameObj.name += staticSuffix + GameObjectUtility.GetStaticEditorFlags(gameObj).ToString();
                gameObj.isStatic = false;
            }
        }
        else
        {
            if (gameObj.name.Contains(staticSuffix))
            {
                string flagString = gameObj.name.Substring(gameObj.name.IndexOf(staticSuffix) + staticSuffix.Length);
                if(string.IsNullOrEmpty(flagString))
                {
                    // If we don't have a flags value, we're probably loading an old version of a file and so just set the entire object as static
                    gameObj.isStatic = true;
                }
                else
                {
                    // If we have a flags value, then we've got some flags to set!
                    StaticEditorFlags flags = (StaticEditorFlags) Enum.Parse(typeof(StaticEditorFlags), flagString);
                    GameObjectUtility.SetStaticEditorFlags(gameObj, flags);
                }
                gameObj.name = gameObj.name.Substring(0, gameObj.name.IndexOf(staticSuffix));
                //Debug.Log("Restoring static object: " + gameObj.name + " " + gameObj.isStatic);
            }

            //EditorUtility.SetDirty(gameObj);
        }

        foreach (Transform child in gameObj.transform)
        {
            SaveRestoreStaticFlags(child.gameObject, save);
        }
    }

    public static void RepairStaticMeshes()
    {
        Transform[] transforms = (Transform[])UnityEngine.Object.FindObjectsOfType(typeof(Transform));

        foreach (Transform child in transforms)
        {
            if (IsRootPrefab(child.gameObject))
            {
                if(ContainsCombinedMesh(child.gameObject))
                {
                    bool bIsStatic = child.gameObject.isStatic;

                    bool bResult = PrefabUtility.ResetToPrefabState(child.gameObject);

                    Debug.Log("Repairing combined meshes in root object: " + child.name + " (" + bResult + ")");

                    child.gameObject.isStatic = bIsStatic;
                    //EditorUtility.SetDirty(child.gameObject);
                }
            }
        }
    }

    static bool IsRootPrefab(GameObject gameObj)
    {
        //GameObject prefabRoot = EditorUtility.FindPrefabRoot(gameObj);
        //GameObject prefabParent = PrefabUtility.GetPrefabParent(gameObj) as GameObject;
        //PrefabType prefabType = PrefabUtility.GetPrefabType(gameObj);
        
        UnityEngine.Object prefabRoot = PrefabUtility.GetPrefabParent(gameObj);
        UnityEngine.Object parentPrefabRoot = gameObj.transform.parent != null ? PrefabUtility.GetPrefabParent(gameObj.transform.parent.gameObject) : null;

        //if (parentPrefabRoot != null && parentPrefabRoot.name.StartsWith("HW_BarrelDrop"))
        //    Debug.Log(prefabRoot + " " + prefabType + " " + parentPrefabRoot + PrefabUtility.GetPrefabType(parentPrefabRoot));

        bool bIsPrefab = prefabRoot != null;
        bool bIsParentPrefab = parentPrefabRoot != null;
        
        if(bIsParentPrefab) // Need to revert children of model prefabs (not root prefab in this case!)
            bIsParentPrefab = PrefabUtility.GetPrefabType(parentPrefabRoot) != PrefabType.ModelPrefab;

        return bIsPrefab && !bIsParentPrefab;
    }

    static bool ContainsCombinedMesh(GameObject gameObj)
    {
        bool bFoundCombinedMesh = false;

        MeshFilter[] meshFilters = gameObj.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter meshFilter in meshFilters)
        {
            // NOTE: meshFilter.name is still the original name
            // even though the sharedMesh has been combined
            if (meshFilter.gameObject.isStatic &&
                meshFilter.sharedMesh.name.StartsWith("Combined Mesh (root: scene)"))
            {
                bFoundCombinedMesh = true;
                break;
            }
        }

        return bFoundCombinedMesh;
    }

    static void ReloadMapFileUpdate()
    {
        MapData mapData = MapData.GetInstance();
        if (!Application.isPlaying && mapData == null && MapData.savedLevelName != null)
        {
            string msg = "MapData was unloaded due to level transition\n\n";
            msg += "Reloading \"" + MapData.savedLevelName + "_Map\" file";
            EditorUtility.DisplayDialog("MapFile Warning:", msg, "OK");
            rcMapLoadSave.LoadLevel(MapData.savedLevelName);
            MapData.savedLevelName = null;
        }
    }

    static public void ShowAllLockedFiles()
    {
        string[] files = Directory.GetFiles(MapData.GetVcsRoot(), "*", SearchOption.AllDirectories);

        string vcsRoot = MapData.GetVcsRoot();

        var fileInfo =
            from file in files
            select new { FileName = file.Replace(vcsRoot, "").Replace(".txt", ""), UserName = System.IO.File.ReadAllText(file).Replace("\n","") };

        fileInfo = fileInfo.OrderBy(x => x.UserName);

        string fileList = "LOCKED LAYERS:\n";
        foreach (var info in fileInfo)
        {
            fileList += info.UserName + " : " + info.FileName + "\n";
        }

        Debug.LogWarning(fileList);
    }
}
