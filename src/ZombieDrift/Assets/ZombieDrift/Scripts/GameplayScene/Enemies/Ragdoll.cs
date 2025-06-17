using UnityEngine;

public class Ragdoll : MonoBehaviour {
    private Rigidbody[] _rigidbodies;
    private Collider[] _colliders;

    private bool _isEnabled;

    public bool isEnabled {
        get => _isEnabled;
        set {
            SetCollidersEnable(value);
            SetRigidbodyIsKinematic(!value);
        }
    }

    public void Initialize() {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
    }

    private void SetCollidersEnable(bool isEnable) {
        foreach (var col in _colliders) {
            col.enabled = isEnable;
        }
    }

    private void SetRigidbodyIsKinematic(bool isOn) {
        foreach (var body in _rigidbodies)
            body.isKinematic = isOn;
    }
}