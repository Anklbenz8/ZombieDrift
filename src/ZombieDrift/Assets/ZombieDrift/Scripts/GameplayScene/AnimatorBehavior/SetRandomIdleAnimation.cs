
using UnityEngine;

public class SetRandomIdleAnimation : StateMachineBehaviour {
    private const string IDLE_ANIMATION_PARAM_NAME = "IdleIndex";
    private const int IDLE_ANIMATIONS_COUNT = 5;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash) {
        animator.SetInteger(IDLE_ANIMATION_PARAM_NAME, Random.Range(0, IDLE_ANIMATIONS_COUNT));
    }
}
