using UnityEngine;

public class ZombieAnimator {
    private const string ANIMATOR_WALK_SPEED_KEY = "WalkSpeed";
    private readonly int ANIMATION_WALK_SPEED_HASH = Animator.StringToHash(ANIMATOR_WALK_SPEED_KEY);
    private const float TURN_SPEED = 180;
    private const float MOVING_TURN_SPEED = 180; 
    private readonly Animator _animator;
    private readonly Transform _transform;
    public bool isEnabled {
        get => _animator.enabled;
        set => _animator.enabled = value;
    }

  
    private float _turnAmount, _forwardAmount;

    public ZombieAnimator(Transform transform, Animator animator) {
        _transform = transform;
        _animator = animator;
    }

    public void PlayMove(Vector3 move) {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        move = _transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, Vector3.up);
        _turnAmount = Mathf.Atan2(move.x, move.z);
        _forwardAmount = move.z;

        ApplyExtraTurnRotation();
        SetSpeed(move.magnitude);
    }

    private void ApplyExtraTurnRotation() {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(TURN_SPEED, MOVING_TURN_SPEED, _forwardAmount);
        _transform.Rotate(0, _turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    private void SetSpeed(float speed) {
        _animator.SetFloat(ANIMATION_WALK_SPEED_HASH, speed);
    }
}