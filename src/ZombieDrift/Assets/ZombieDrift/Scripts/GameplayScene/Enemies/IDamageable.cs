using UnityEngine;

public interface IDamageable {
    Vector3 position { get; }
    void Deactivate();
}