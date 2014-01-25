using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

class rcMapView : EditorWindow
{
    // Info to display on root objects
    public struct RootObjInfo
    {
        public string name;
        public string hideFlags;
    }

    // Root object info data members
    public List<RootObjInfo> rootObjectsInfo = new List<RootObjInfo>();
    public bool refreshRootObjectInfo = true;
    public Vector2 scrollPosition;
    public bool showFoldout;
    public bool playMode;


    public rcMapView()
    {
        autoRepaintOnSceneChange = true;
		title = "Scene Layers";
    }

    public void Refresh()
    {
        MapData mapData = MapData.GetInstance();
        if (mapData == null)
            return;

        foreach (MapLayer mapLayer in mapData.mapLayers)
        {
            mapLayer.Refresh();

            GameObject mapLayerRoot = mapLayer.GetRoot();

            // Show layers that are checked out to us
            if (mapLayer.checkedOut && mapLayer.user == MapData.userName)
            {
                if (mapLayerRoot.hideFlags == HideFlags.HideInHierarchy)
                    rcMapLoadSave.SetHideFlags(mapLayerRoot, 0);
            }

            // Hide layers that aren't checked-out or are checked out to others
            if (!mapLayer.checkedOut || (mapLayer.checkedOut && mapLayer.user != MapData.userName))
            {
                if (mapLayerRoot != null && mapLayerRoot.hideFlags == 0)
                    rcMapLoadSave.SetHideFlags(mapLayerRoot, HideFlags.HideInHierarchy);
            }
        }
    }

    void OnGUI()
    {
        if (GUILayout.Button("Show All Locked Layers"))
            rcMapLoadSave.ShowAllLockedFiles();

        MapData mapData = MapData.GetInstance();
        if (mapData == null)
        {
            GUILayout.Label("No Map File loaded");
            return;
        }

        //if (GUILayout.Button("Save " + mapData.levelName))
        //    rcMapLoadSave.SaveLevel(mapData.levelName);

        if (GUILayout.Button("Refresh"))
            Refresh();

        foreach (MapLayer mapLayer in mapData.mapLayers)
        {
            GameObject mapLayerRoot = mapLayer.GetRoot();

            if ((mapLayer.checkedOut && mapLayer.user != MapData.userName) || Application.isPlaying)
                GUI.enabled = false;

            EditorGUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.MaxHeight(16) });

            bool checkedOut = EditorGUILayout.Toggle(mapLayer.layerName, mapLayer.checkedOut);
            if (checkedOut && !mapLayer.checkedOut)
            {
                String result = mapLayer.CheckOut();
                if (result != null)
                {
                    ShowFileError("Check-out Error", mapLayer.layerName, result);
                }

                // Show layers that are checked out to us
                if (mapLayer.checkedOut)
                {
                    if (mapLayerRoot.hideFlags == HideFlags.HideInHierarchy)
                        rcMapLoadSave.SetHideFlags(mapLayerRoot, 0);
                }
            }
            else if (!checkedOut && mapLayer.checkedOut)
            {
                String result = mapLayer.CheckIn();
                if (result != null)
                {
                    ShowFileError("Check-in Error", mapLayer.layerName, result);
                }

                // Hide layers that aren't checked-out
                if (!mapLayer.checkedOut)
                {
                    if (mapLayerRoot.hideFlags == 0)
                        rcMapLoadSave.SetHideFlags(mapLayerRoot, HideFlags.HideInHierarchy);
                }
            }

            if (mapLayer.checkedOut)
                GUILayout.Label("(" + mapLayer.user + ")");

            EditorGUILayout.EndHorizontal();

            GUI.enabled = true;
        }

        ShowRootObjectInfo();
    }

    public void ShowFileError(String title, String layerName, String result)
    {
        String msg = "Could not release lock on " + layerName + "!\n\n";
        msg += "Error: " + result + "\n\n";
		msg += "Verify you can connect to " + MapData.GetVcsRoot();
        EditorUtility.DisplayDialog(title, msg, "OK");
    }

    public void Update()
	{
        // Refresh root object info list when toggling play mode and force window to repaint
        if(playMode != Application.isPlaying)
        {
            refreshRootObjectInfo = true;
            playMode = Application.isPlaying;

            Repaint();
        }
    }
	
    public void OnEnable()
    {
        //Debug.Log("MapView.OnEnable()");
    }

    public void OnDisable()
    {
        //Debug.Log("MapView.OnDisable()");
    }

    public void OnDestroy()
    {
        //Debug.Log("MapView.OnDestroy()");
    }

    public void OnHierarchyChange()
    {
        if (!Application.isPlaying)
        {
            //Debug.Log("MapView.OnHierarchyChange()");
            refreshRootObjectInfo = true;
        }
    }

    public void UpdateRootObjectInfo()
    {
        // Get list of _all_ game objects (whether active or not).  NOTE: this API returns internal
        // unity objects so we filter out prefab instances as we really only want to display objects
        // in the scene hierachy. Also don't want to call this every frame as it's _slow_ so only
        // invoke on hierarchy change or when toggling play mode.
        GameObject[] gameObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

        rootObjectsInfo.Clear();
        foreach (var gameObj in gameObjects)
        {
            // Exclude prefabs
            PrefabType prefabType = PrefabUtility.GetPrefabType(gameObj);
            if (gameObj.transform.parent == null && prefabType == PrefabType.None)
            {
                // Save off name and hide flags to helper struct
                RootObjInfo rootObjInfo = new RootObjInfo();
                rootObjInfo.name = gameObj.name;
                rootObjInfo.hideFlags = gameObj.hideFlags.ToString();
                rootObjectsInfo.Add(rootObjInfo);
            }
        }
    }

    public void ShowRootObjectInfo()
    {
        if (refreshRootObjectInfo)
        {
            // Rebuild list of root game objects
            UpdateRootObjectInfo();
            refreshRootObjectInfo = false;
        }

        // Add a collapsible scrolling list of labels
        EditorGUILayout.Separator();
        showFoldout = EditorGUILayout.Foldout(showFoldout, "Root Objects");
        if (showFoldout)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            foreach (var rootObjInfo in rootObjectsInfo)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label(rootObjInfo.name, GUILayout.MaxWidth(200));
                GUILayout.Label(rootObjInfo.hideFlags, GUILayout.MinWidth(200));

                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();

            // Frame the scrolling list bounds so it stands out against the background
            if (Event.current.type == EventType.Repaint)
            {
                Rect bounds = GUILayoutUtility.GetLastRect();
                GUI.Box(bounds, GUIContent.none);
            }
        }
    }
}
