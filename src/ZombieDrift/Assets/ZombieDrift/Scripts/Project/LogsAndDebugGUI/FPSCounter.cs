using System;
using UnityEngine;
using UnityEngine.Serialization;

enum ScreenPos {
	LeftTop = 0,
	RightTop = 1,
	Center = 4
}

public class FPSCounter : MonoBehaviour {

	[FormerlySerializedAs("guiSize")] [SerializeField]
	private Vector2 guiRectSize = new(200, 100);

	[SerializeField] private int frameRange = 60;
	[SerializeField] private int fontSize = 15;
	[SerializeField] private Color color = Color.green;
	[SerializeField] private ScreenPos screenPos;
	public int averageFPS { get; private set; }

	private Rect rect => screenPos switch {
			ScreenPos.LeftTop => new Rect(0, 0, guiRectSize.x, guiRectSize.y),
			ScreenPos.RightTop => new Rect(Screen.width - guiRectSize.x, 0, guiRectSize.x, guiRectSize.y),
			ScreenPos.Center => new Rect((Screen.width - guiRectSize.x) / 2, (Screen.height - guiRectSize.y) / 2,
					guiRectSize.x, guiRectSize.y),
			_ => throw new ArgumentOutOfRangeException($"{screenPos.ToString()} not exist")
	};

	private GUIStyle guiStyle => screenPos switch {
			ScreenPos.LeftTop => new GUIStyle {
					fontSize = fontSize, fontStyle = UnityEngine.FontStyle.Bold, normal = {textColor = color},
					alignment = TextAnchor.UpperLeft
			},
			ScreenPos.RightTop => new GUIStyle {
					fontSize = fontSize, fontStyle = UnityEngine.FontStyle.Bold, normal = {textColor = color},
					alignment = TextAnchor.UpperRight
			},
			ScreenPos.Center => new GUIStyle {
					fontSize = fontSize, fontStyle = UnityEngine.FontStyle.Bold, normal = {textColor = color},
					alignment = TextAnchor.MiddleCenter
			},
			_ => throw new ArgumentOutOfRangeException()
	};

	private GUIStyle _guiStyle;
	private int[] _fpsBuffer;
	private int _fpsBufferIndex;

	private void Awake() =>
			_guiStyle = new GUIStyle
					{fontSize = fontSize, fontStyle = UnityEngine.FontStyle.Bold, normal = {textColor = color}};

	private void Update() {
		if (_fpsBuffer == null || frameRange != _fpsBuffer.Length)
			InitializeBuffer();

		UpdateBuffer();
		CalculateAverageFps();
	}

	private void InitializeBuffer() {
		if (frameRange <= 0)
			frameRange = 1;

		_fpsBuffer = new int[frameRange];
		_fpsBufferIndex = 0;
	}

	private void UpdateBuffer() {
		_fpsBuffer[_fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
		if (_fpsBufferIndex >= frameRange)
			_fpsBufferIndex = 0;
	}

	private void CalculateAverageFps() {
		int sum = 0;

		for (int i = 0; i < frameRange; i++) {
			int fps = _fpsBuffer[i];
			sum += fps;
		}

		averageFPS = sum / frameRange;
	}

	private void OnGUI() {
		GUILayout.BeginArea(rect);
		GUILayout.Label($"{averageFPS} fps", guiStyle);
		GUILayout.EndArea();
	}
}
