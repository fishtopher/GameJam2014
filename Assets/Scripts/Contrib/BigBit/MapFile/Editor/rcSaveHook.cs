using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// Custom save hook for more transparent map file integration
#if true // _fix_Unity4_
public class rcSaveHook : UnityEditor.AssetModificationProcessor
#else
public class rcSaveHook : AssetModificationProcessor
#endif
{
	// Save the map file and associated scene layers for the given level
	static string[] OnWillSaveAssets (string[] paths) 
	{
		// If we have a map file loaded
		MapData mapData = MapData.GetInstance();
		if(mapData != null)
		{
            bool bSavingMapFile = false;
			
			// Look for our map file among the list of assets to be saved
			string mapFile = mapData.levelName + "_Map.unity";
			foreach(string path in paths)
			{
				string filename = Path.GetFileName(path);
                if(filename.Equals(mapFile))
				{
					// Found map file
                    bSavingMapFile = true;
				}
			}
            
            if (bSavingMapFile)
            {
				//V Debug.Log("Saving " + mapFile);
				
				// Hide all the layers and then let unity save the map file
				// This way it doesn't think the scene is dirty
                rcMapLoadSave.PreMapSave(mapData.levelName);

                EditorApplication.update += PostSaveUpdate;	
            }
		}
		
		return paths;
    }
	
	static void PostSaveUpdate()
	{
		MapData mapData = MapData.GetInstance();
		if(mapData != null)
		{
			// Save each visible scene file
			rcMapLoadSave.PostMapSave();
		}
		
		EditorApplication.update -= PostSaveUpdate;	
	}

    /*
    public static bool IsOpenForEdit(string assetPath, out string message)
    {
        // Debug.Log(assetPath + " " + bLockedAsset);

        message = "";
        bool bLockedAsset = assetPath.Contains("ExternalVCS");
        if (bLockedAsset)
            message = "File is locked in SVN";

        return !bLockedAsset;
    }
    */
}
