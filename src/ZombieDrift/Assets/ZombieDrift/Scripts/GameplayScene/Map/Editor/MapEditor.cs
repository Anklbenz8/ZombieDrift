using Gameplay;
using UnityEditor;
using UnityEngine;

namespace Project {
	[CustomEditor(typeof(Map))]
	public class MapEditor : Editor {
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			if (!GUILayout.Button("Update Data"))
				return;

			var map = (Map)serializedObject.targetObject;
			map.UpdateZombiePoints();

			EditorUtility.SetDirty(serializedObject.targetObject);
			PrefabUtility.RecordPrefabInstancePropertyModifications(this);
			serializedObject.ApplyModifiedProperties();
		}
	}
}
