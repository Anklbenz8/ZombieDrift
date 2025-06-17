using System;
using System.Collections.Generic;
using Zenject;

namespace Project {
    public class StateSwitcher : IFixedTickable, ITickable {
        private State current { get; set; }
        private Type currentType { get; set; }
        private readonly Dictionary<Type, State> _states = new();

        public void AddState(State state) =>
            _states.Add(state.GetType(), state);

        public void SetState<T>() where T : State {
            var type = typeof(T);

            if (type == currentType)
                return;

            if (!_states.TryGetValue(type, out var state))
                throw new Exception($"State {type} not found in state machine");

            currentType = type;
            current?.Exit();
            current = state;
            current.Enter();
        }


        public void FixedTick() =>
            current?.FixedTick();

        public void Tick() =>
            current?.Tick();
    }
}