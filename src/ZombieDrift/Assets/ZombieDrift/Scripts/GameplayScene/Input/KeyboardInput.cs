using System;
using UnityEngine;

public class KeyboardInput : IInput {
	public event Action<float> HorizontalAxisChangedEvent;
	public event Action AnyPressedEvent;

	private bool isLeftPressed => InputUtils.CheckIfAnyKeyPressed(_config.leftKeys);
	private bool isRightPressed => InputUtils.CheckIfAnyKeyPressed(_config.rightKeys);
	private bool isAnyPressed => Input.anyKey && !(Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2));
	private bool isNothingPressed => !isLeftPressed && !isRightPressed;

	private readonly InputConfig _config;

	private bool _pressedInLastFrame;

	public KeyboardInput(InputConfig config) {
		_config = config;
	}

	public void Tick() {
		//	if (_pressedInLastFrame) {
		//		HorizontalAxisChangedEvent?.Invoke(0);
		//		_pressedInLastFrame = false;
		//	}
		if (isNothingPressed)
			HorizontalAxisChangedEvent?.Invoke(0);

		if (isAnyPressed) {
			AnyPressedEvent?.Invoke();
		//	_pressedInLastFrame = true;
		}

		if (isLeftPressed)
			HorizontalAxisChangedEvent?.Invoke(-1);
		if (isRightPressed)
			HorizontalAxisChangedEvent?.Invoke(1);
	}
}
