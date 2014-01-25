using UnityEditor;
using UnityEngine;
using System.Collections;

public class RandomWizard : ScriptableWizard
{
	[MenuItem ("Vitei/Randomise Transform")]
    static void CreateWizard () 
	{
        ScriptableWizard.DisplayWizard<RandomWizard>("Randomise", "Done", "Revert");
		
		m_startLocalPositions = new Vector3[Selection.gameObjects.Length];
		m_startLocalRotations = new Quaternion[Selection.gameObjects.Length];
		m_startLocalScales = new Vector3[Selection.gameObjects.Length];
		for(int i = 0; i < Selection.gameObjects.Length; i++)
		{
			m_startLocalPositions[i] = Selection.gameObjects[i].transform.localPosition;
			m_startLocalRotations[i] = Selection.gameObjects[i].transform.localRotation;
			m_startLocalScales[i] = Selection.gameObjects[i].transform.localScale;
		}
		
		Undo.RecordObjects(Selection.gameObjects, "Randomize Transform");
    }
	
    public Vector3 m_maxPositionOffset = Vector3.zero;
    public Vector3 m_maxRotationOffset = Vector3.zero;
    public Vector3 m_maxScaleOffset = Vector3.zero;
	public float m_scalarScaler = 0;
	
	private static Vector3[] m_startLocalPositions;
	private static Quaternion[] m_startLocalRotations;
	private static Vector3[] m_startLocalScales;
	
	void OnWizardCreate () 
	{
		m_startLocalPositions = null;
		m_startLocalRotations = null;
		m_startLocalScales = null;
    }  
	
    void OnWizardUpdate () 
	{	
		if(m_startLocalPositions == null || m_startLocalRotations == null || m_startLocalScales == null)
			return;
		
		for(int i = 0; i < Selection.gameObjects.Length; i++)
		{
			Selection.gameObjects[i].transform.localPosition = m_startLocalPositions[i];
			Selection.gameObjects[i].transform.Translate(
				m_maxPositionOffset.x * Random.Range(-1.0f, 1.0f),
				m_maxPositionOffset.y * Random.Range(-1.0f, 1.0f),
				m_maxPositionOffset.z * Random.Range(-1.0f, 1.0f)
				); 
			
			Selection.gameObjects[i].transform.localRotation = m_startLocalRotations[i];
			Selection.gameObjects[i].transform.Rotate(
				m_maxRotationOffset.x * Random.Range(-1.0f, 1.0f),
				m_maxRotationOffset.y * Random.Range(-1.0f, 1.0f),
				m_maxRotationOffset.z * Random.Range(-1.0f, 1.0f)
				); 
			
			Selection.gameObjects[i].transform.localScale = m_startLocalScales[i];
			Vector3 ns = Selection.gameObjects[i].transform.localScale;
			ns.x = ns.x + (m_maxScaleOffset.x * Random.Range(-1.0f, 1.0f));
			ns.y = ns.y + (m_maxScaleOffset.y * Random.Range(-1.0f, 1.0f));
			ns.z = ns.z + (m_maxScaleOffset.z * Random.Range(-1.0f, 1.0f));
			ns *= (1 + (m_scalarScaler * Random.Range(-1.0f, 1.0f)));
			Selection.gameObjects[i].transform.localScale = ns; 
		}
	
	}   
	
    // When the user pressed the "Cancel" button OnWizardOtherButton is called.
    void OnWizardOtherButton () 
	{
		for(int i = 0; i < Selection.gameObjects.Length; i++)
		{
			Selection.gameObjects[i].transform.localPosition = m_startLocalPositions[i];
			Selection.gameObjects[i].transform.localRotation = m_startLocalRotations[i];
			Selection.gameObjects[i].transform.localScale = m_startLocalScales[i];
		}
    }
}
