using System;using Zenject;

public interface IInput: ITickable {
	event Action<float> HorizontalAxisChangedEvent;
	event Action AnyPressedEvent;
}
