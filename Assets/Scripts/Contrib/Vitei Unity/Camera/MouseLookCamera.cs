using UnityEngine;
using System.Collections;
using InControl;

// adapted from http://answers.unity3d.com/questions/29741/mouse-look-script.html
/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation
public class MouseLookCamera : MonoBehaviour
{
    public Vector2 m_sensitivity = new Vector2(3.0f, 3.0f);
    public float m_minX = -360.0f;
    public float m_maxX = 360.0f;
    public float m_minY = -60.0f;
    public float m_maxY = 60.0f;
	public Vector2 m_stickExtents = new Vector2(0.5f, 1.0f);
	
    Vector2 m_eulerRot = Vector2.zero;
    Quaternion m_initialRot;
 
    void Start ()
    {
        m_initialRot = transform.localRotation;
    }
	
	void Update ()
	{	
		// Allow looking on stick
		float stickRotX = InControl.InputManager.ActiveDevice.RightStickX * m_maxX * m_stickExtents.x;
		float stickRotY = InControl.InputManager.ActiveDevice.RightStickY * m_maxY * m_stickExtents.y;
		if(stickRotX != 0 || stickRotY != 0)
		{		
			m_eulerRot = Vector2.zero;
		}
		else
		{
			m_eulerRot.x += Input.GetAxis("Mouse X") * m_sensitivity.x;
			m_eulerRot.y += Input.GetAxis("Mouse Y") * m_sensitivity.y;
			m_eulerRot.x = ClampAngle (m_eulerRot.x, m_minX, m_maxX);
			m_eulerRot.y = ClampAngle (m_eulerRot.y, m_minY, m_maxY);
		}
		
		float rotX = ClampAngle (m_eulerRot.x + stickRotX, m_minX, m_maxX);
		float rotY = ClampAngle (m_eulerRot.y + stickRotY, m_minY, m_maxY);
		
		Quaternion q = Quaternion.Euler (-rotY, rotX, 0);
		transform.localRotation = m_initialRot * q;
	}
 
    public static float ClampAngle (float angle, float min, float max)
    {
        while (angle < -360.0f)
            angle += 360.0f;
        while (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp (angle, min, max);
    }
}