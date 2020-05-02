using UnityEngine;
using System.Collections;

public class RenderCameraOnScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Render ();
	}

	public void Render()
	{
		gameObject.GetComponent<Camera> ().rect = new Rect (.79f,.01f,.2f,.3f);
	}
}
