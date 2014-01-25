using UnityEngine;
using UnityEditor;

public class rcProjectMenus_AUTOGENERATED_
{

	[MenuItem("Scene Layers/Scenes/Newland/Load", false, 1)]
	static void Load_Newland() { rcMapLoadSave.LoadLevel("Newland"); }

	[MenuItem("Scene Layers/Scenes/Newland/Bake", false, 1)]
	static void Bake_Newland() { rcMapLoadSave.BakeLevel("Newland"); }

	[MenuItem("Scene Layers/Scenes/TitleScene/Load", false, 1)]
	static void Load_TitleScene() { rcMapLoadSave.LoadLevel("TitleScene"); }

	[MenuItem("Scene Layers/Scenes/TitleScene/Bake", false, 1)]
	static void Bake_TitleScene() { rcMapLoadSave.BakeLevel("TitleScene"); }
}