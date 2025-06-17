using UnityEngine;

[CreateAssetMenu(fileName = "keyboardConfig", menuName = "Configs/KeyboardConfig")]
public class InputConfig : ScriptableObject {
	[Header("Keyboard")]
	[SerializeField] private KeyCode[] _leftKeys;

	[SerializeField] private KeyCode[] _right;
	public KeyCode[] leftKeys => _leftKeys;
	public KeyCode[] rightKeys => _right;
}