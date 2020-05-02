#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor(typeof(RenderCameraOnScreen))]
public class RenderCamera : Editor {

	public override void OnInspectorGUI(){
		//base.OnInspectorGUI ();
		DrawDefaultInspector ();
		
		if (GUILayout.Button ("Regenerate")) {
			GameObject.Find("BackCamera").GetComponent<RenderCameraOnScreen>().Render();
		}
	}

}
#endif