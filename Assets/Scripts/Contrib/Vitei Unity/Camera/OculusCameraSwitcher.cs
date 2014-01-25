using UnityEngine;
using System.Collections;

[System.Serializable]
public class CameraSet
{
	public GameObject m_root;
	public Transform m_eyeTransform;	// Used by other guys for distance calcs, holding objects etc
}

[System.Serializable]
public class OculusEmuData
{
	public Renderer m_resolutionEmuObject;
	public Material m_pixelsMaterial800;
	public Material m_pixelsMaterial1080;
	public float m_fov = 97.55885f;
}

public class OculusCameraSwitcher : MonoBehaviour 
{
	public enum CameraMode { Auto, Standard, Oculus };
	public CameraMode m_cameraMode = CameraMode.Auto;

	public enum EmulationMode { None, SD, HD };
	public EmulationMode m_emulateOculusScreen = EmulationMode.None;
	       EmulationMode m_prevEmulateOculusScreen = EmulationMode.None;
	public OculusEmuData m_oculusEmulation;
	
	public CameraSet m_standardCamera;
	public CameraSet m_oculusCameras;

	public static CameraSet m_activeCamSet;
	public static Transform EyeTransform { get { return  m_activeCamSet.m_eyeTransform; } }
			
	// Use this for initialization
	void Awake () 
	{
		bool oculusOn = (m_cameraMode == CameraMode.Auto && OVRDevice.SensorCount > 0) || m_cameraMode == CameraMode.Oculus;

		m_standardCamera.m_root.SetActive(!oculusOn);
		m_oculusCameras.m_root.SetActive(oculusOn);

		m_activeCamSet = oculusOn ? m_oculusCameras : m_standardCamera;
	
		if(oculusOn)
		{
			// Oculus mode but no oculus connected? use mouse/stick controls!
			MouseLookCamera mlc = GetComponent<MouseLookCamera>();
			if(mlc)
			{
				mlc.enabled = (OVRDevice.SensorCount <= 0);
			}
		}
		else
		{
			SetupEmulation();
		}
	}

	void Update()
	{
		if(m_prevEmulateOculusScreen != m_emulateOculusScreen)
		{
			SetupEmulation();
			m_prevEmulateOculusScreen = m_emulateOculusScreen;
		}
	}

	void SetupEmulation()
	{
		Camera[] cams = m_standardCamera.m_root.GetComponentsInChildren<Camera>();
		if(m_emulateOculusScreen == EmulationMode.None)
		{
			m_oculusEmulation.m_resolutionEmuObject.gameObject.SetActive(false);
			for(int i = 0; i < cams.Length; i++)
			{
				cams[i].fieldOfView = 60.0f;
			}
		}
		else
		{
			m_oculusEmulation.m_resolutionEmuObject.gameObject.SetActive(true);
			m_oculusEmulation.m_resolutionEmuObject.material = (m_emulateOculusScreen == EmulationMode.SD) ? m_oculusEmulation.m_pixelsMaterial800 : m_oculusEmulation.m_pixelsMaterial1080;
			for(int i = 0; i < cams.Length; i++)
			{
				cams[i].fieldOfView = m_oculusEmulation.m_fov;
			}
		}
	}


}
