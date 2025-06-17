using UnityEngine;

namespace Project {
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Animator))]
    public class AnimatedView : View {
        [SerializeField] private Animator animator;
        [SerializeField] private string closeTrigger = "disappear";

        public override bool isActive {
            get => base.isActive;
            set {
                if (value)
                    base.isActive = value;
                else
                    ClosePlay();
            }
        }

        public void ForceClose() =>
            base.isActive = false;

        private void ClosePlay() {
            if (!animator) {
                base.isActive = false;
                return;
            }

            var closeHash = Animator.StringToHash(closeTrigger);
            animator.SetTrigger(closeHash);
        }

        private void Handle_OnAnimationEnd() => // Animation event
            ForceClose();
    }
}