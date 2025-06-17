using System;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(PrefsDeleteAll))]
public class PrefsDeleteAllEditor : Editor {
	public override void OnInspectorGUI() {
		if (GUILayout.Button("Delete All"))
			((PrefsDeleteAll)serializedObject.targetObject).DeleteAllPrefs();
	}
}
