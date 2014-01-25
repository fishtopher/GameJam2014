using UnityEngine;

public class rcRenderSettingsFixup : MonoBehaviour 
{
    public bool fog;
    public Color fogColor;
    public float fogDensity;
    public FogMode fogMode;
    public float fogEndDistance;
    public float fogStartDistance;
    public Color ambientLight;
    public float haloStrength;
    public float flareStrength;
    public Material skybox;
		
	void Start () 
    {
	    RenderSettings.fog = fog;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogMode = fogMode;
        RenderSettings.fogEndDistance = fogEndDistance;
        RenderSettings.fogStartDistance = fogStartDistance;

        RenderSettings.ambientLight = ambientLight;
        RenderSettings.haloStrength = haloStrength;
        RenderSettings.flareStrength = flareStrength;
        RenderSettings.skybox = skybox;
	}
	
	public static void Save()
    {
        GameObject renderSettings = new GameObject("RenderSettings");
        rcRenderSettingsFixup savedSettings = renderSettings.AddComponent<rcRenderSettingsFixup>();

        savedSettings.fog = RenderSettings.fog;
        savedSettings.fogColor = RenderSettings.fogColor;
        savedSettings.fogDensity = RenderSettings.fogDensity;
        savedSettings.fogMode = RenderSettings.fogMode;
        savedSettings.fogEndDistance = RenderSettings.fogEndDistance;
        savedSettings.fogStartDistance = RenderSettings.fogStartDistance;

        savedSettings.ambientLight = RenderSettings.ambientLight;
        savedSettings.haloStrength = RenderSettings.haloStrength;
        savedSettings.flareStrength = RenderSettings.flareStrength;
        savedSettings.skybox = RenderSettings.skybox;
	}
}
