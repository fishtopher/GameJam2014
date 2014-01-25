using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class rcInit
{
    static rcInit()
    {
        // Set the project name to qualify the Map file VCS path for different projects
        MapData.SetProjectName(UnityEditor.PlayerSettings.productName.Replace(" ", "_"));

        string currentProject = EditorPrefs.GetString("rcCurrentProject");
        if (currentProject != Application.dataPath)
        {
            Debug.Log("Opened new project: " + Application.dataPath);
            EditorPrefs.SetString("rcCurrentProject", Application.dataPath);

            EditorApplication.update += RunOnce;
            
            // Print out current settings at startup
            //Debug.Log("rcInit: Application.Platform = " + Application.platform);
            //Debug.Log("rcInit: EditorUserBuildSettings.selectedBuildTargetGroup = " + EditorUserBuildSettings.selectedBuildTargetGroup);
            //Debug.Log("rcInit: EditorUserBuildSettings.selectedStandaloneTarget = " + EditorUserBuildSettings.selectedStandaloneTarget);
            //Debug.Log("rcInit: EditorUserBuildSettings.activeBuildTarget = " + EditorUserBuildSettings.activeBuildTarget);
        }
    }

    static void RunOnce()
    {
        EditorApplication.update -= RunOnce;

        // Loop over editor windows looking and blow away any 'Fallback' windows
        EditorWindow[] editorWindows = Resources.FindObjectsOfTypeAll(typeof(EditorWindow)) as EditorWindow[];
        foreach (EditorWindow editorWindow in editorWindows)
        {
            //Debug.Log(editorWindow.name + " " + editorWindow.GetType());
            if (editorWindow.GetType().ToString() == "UnityEditor.FallbackEditorWindow")
            {
                editorWindow.Close();
            }
        }
    }
}