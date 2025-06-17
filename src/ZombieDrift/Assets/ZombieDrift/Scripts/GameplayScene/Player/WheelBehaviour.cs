using System;
using UnityEngine;

[Serializable]
public class WheelBehaviour {
    [SerializeField] private Transform[] _steerControlled;
    [SerializeField] private Transform[] _rotatable;
    [SerializeField] private float _rotationSpeed = 25;
    [SerializeField] private float _steerAngle = 45;

    public float wheelTurn {
        set {
            var angle = value == 0 ? 0 : _steerAngle * value;

             foreach (var wheel in _steerControlled)
                 wheel.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
    }

    public void FixedTick() {
        foreach (var wheel in _rotatable)
            wheel.localRotation *= Quaternion.AngleAxis(-_rotationSpeed, Vector3.left);
    }
}