using UnityEngine;
using System.Collections;

public class ItemGetText : MonoBehaviour {

	private TextMesh m_textMesh;
	private string m_textString;

	// Use this for initialization
	void Start () {
		m_textString = "";
	}

	public void Initialize( string text, Color color, float duration = 1.0f )
	{
		m_textMesh = this.gameObject.GetComponent<TextMesh>();
		m_textMesh.text = text;
		m_textMesh.color = color;
		StartCoroutine( ShowText( duration ) );
	}
	
	// Update is called once per frame
	void Update () 
	{
		this.transform.position += new Vector3(Player.Instance.m_runSpeed * 0.5f, 0.5f, 0) * Time.deltaTime;
	}

	private IEnumerator ShowText( float duration )
	{
		yield return new WaitForSeconds( duration );
		Destroy( this.gameObject );
	}
}
