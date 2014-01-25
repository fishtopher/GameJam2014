// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.
using UnityEngine;
using System.Collections;

public class SmoothLookatCam : MonoBehaviour
{

	public Transform target;
	public float damping = 6.0f;
	public bool smooth = true;

	void  LateUpdate ()
	{
		if (target) {
			if (smooth) {
				// Look at and dampen the rotation
				Quaternion rotation = Quaternion.LookRotation (target.position - transform.position);
				transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * damping);
			} else {
				// Just lookat
				transform.LookAt (target);
			}
		}
	}

	void  Start ()
	{
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
	}
}