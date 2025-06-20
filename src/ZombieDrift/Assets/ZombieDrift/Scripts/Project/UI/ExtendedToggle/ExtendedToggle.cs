using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI {
    //[AddComponentMenu("UI/ExtendeToggle", 30)]
    [RequireComponent(typeof(RectTransform))]
    public class ExtendedToggle : Selectable, IPointerClickHandler, ISubmitHandler, ICanvasElement {
        /// <summary>
        /// Display settings for when a toggle is activated or deactivated.
        /// </summary>
        public enum ToggleTransition {
            /// <summary>
            /// Show / hide the toggle instantly
            /// </summary>
            None,

            /// <summary>
            /// Fade the toggle in / out smoothly.
            /// </summary>
            Fade
        }

        [Serializable]
        public class ToggleEvent : UnityEvent<bool> {
        }

        /// <summary>
        /// Transition mode for the toggle.
        /// </summary>
        public ToggleTransition toggleTransition = ToggleTransition.Fade;

        /// <summary>
        /// Graphic the toggle should be working with.
        /// </summary>
        public Graphic isOnGraphic;

        public Graphic isOffGraphic;

        [SerializeField]
        private ToggleGroup m_Group;

        /// <summary>
        /// Group the toggle belongs to.
        /// </summary>
        public ToggleGroup group {
            get { return m_Group; }
            set {
                SetToggleGroup(value, true);
                PlayEffect(true);
            }
        }

        public ToggleEvent onValueChanged = new ToggleEvent();

        // Whether the toggle is on
        [Tooltip("Is the toggle currently on or off?")]
        [SerializeField]
        private bool m_IsOn;

        protected ExtendedToggle() {
        }

#if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();

            if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this) && !Application.isPlaying)
                CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(this);
        }

#endif // if UNITY_EDITOR

        public virtual void Rebuild(CanvasUpdate executing) {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
                onValueChanged.Invoke(m_IsOn);
#endif
        }

        public virtual void LayoutComplete() {
        }

        public virtual void GraphicUpdateComplete() {
        }

        protected override void OnDestroy() {
            if (m_Group != null)
                m_Group.EnsureValidState();
            base.OnDestroy();
        }

        protected override void OnEnable() {
            base.OnEnable();
            SetToggleGroup(m_Group, false);
            PlayEffect(true);
        }

        protected override void OnDisable() {
            SetToggleGroup(null, false);
            base.OnDisable();
        }

        private void SetToggleGroup(ToggleGroup newGroup, bool setMemberValue) {
            /* Если будет надо сделай расширенную Toggle группу
             
             * // Sometimes IsActive returns false in OnDisable so don't check for it.
            // Rather remove the toggle too often than too little.
            if (m_Group != null)
                m_Group.UnregisterToggle(this);

            // At runtime the group variable should be set but not when calling this method from OnEnable or OnDisable.
            // That's why we use the setMemberValue parameter.
            if (setMemberValue)
                m_Group = newGroup;

            // Only register to the new group if this Toggle is active.
            if (newGroup != null && IsActive())
                newGroup.RegisterToggle(this);

            // If we are in a new group, and this toggle is on, notify group.
            // Note: Don't refer to m_Group here as it's not guaranteed to have been set.
            if (newGroup != null && isOn && IsActive())
                newGroup.NotifyToggleOn(this);
                
                */
        }

        /// <summary>
        /// Whether the toggle is currently active.
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// /Attach this script to a Toggle GameObject. To do this, go to Create>UI>Toggle.
        /// //Set your own Text in the Inspector window
        ///
        /// using UnityEngine;
        /// using UnityEngine.UI;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///     Toggle m_Toggle;
        ///     public Text m_Text;
        ///
        ///     void Start()
        ///     {
        ///         //Fetch the Toggle GameObject
        ///         m_Toggle = GetComponent<Toggle>();
        ///         //Add listener for when the state of the Toggle changes, and output the state
        ///         m_Toggle.onValueChanged.AddListener(delegate {
        ///                 ToggleValueChanged(m_Toggle);
        ///             });
        ///
        ///         //Initialize the Text to say whether the Toggle is in a positive or negative state
        ///         m_Text.text = "Toggle is : " + m_Toggle.isOn;
        ///     }
        ///
        ///     //Output the new state of the Toggle into Text when the user uses the Toggle
        ///     void ToggleValueChanged(Toggle change)
        ///     {
        ///         m_Text.text =  "Toggle is : " + m_Toggle.isOn;
        ///     }
        /// }
        /// ]]>
        ///</code>
        /// </example>

        public bool isOn {
            get { return m_IsOn; }

            set { Set(value); }
        }

        /// <summary>
        /// Set isOn without invoking onValueChanged callback.
        /// </summary>
        /// <param name="value">New Value for isOn.</param>
        public void SetIsOnWithoutNotify(bool value) {
            Set(value, false);
        }

        void Set(bool value, bool sendCallback = true) {
            if (m_IsOn == value)
                return;

            // if we are in a group and set to true, do group logic
            m_IsOn = value;
            if (m_Group != null && m_Group.isActiveAndEnabled && IsActive()) {
                if (m_IsOn || (!m_Group.AnyTogglesOn() && !m_Group.allowSwitchOff)) {
                    m_IsOn = true;
                    //--         m_Group.NotifyToggleOn(this, sendCallback);
                }
            }

            // Always send event when toggle is clicked, even if value didn't change
            // due to already active toggle in a toggle group being clicked.
            // Controls like Dropdown rely on this.
            // It's up to the user to ignore a selection being set to the same value it already was, if desired.
            PlayEffect(toggleTransition == ToggleTransition.None);

            if (sendCallback) {
                UISystemProfilerApi.AddMarker("Toggle.value", this);
                onValueChanged.Invoke(m_IsOn);
            }
        }

        // ------- Extended
        private void PlayEffect(bool instant) {
            if (isOnGraphic != null)
                PlayIsOnGraphicEffect(isOnGraphic, instant);

            if (isOffGraphic != null)
                PlayIsOffGraphicEffect(isOffGraphic, instant);
        }


        private void PlayIsOnGraphicEffect(Graphic graphic, bool instant) {
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                graphic.canvasRenderer.SetAlpha(m_IsOn ? 1f : 0f);
            }
            else
#endif
                graphic.CrossFadeAlpha(m_IsOn ? 1f : 0f, instant ? 0f : 0.1f, true);
        }
        
        private void PlayIsOffGraphicEffect(Graphic graphic, bool instant) {
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                graphic.canvasRenderer.SetAlpha(m_IsOn ? 0f : 1f);
            }
            else
#endif
                graphic.CrossFadeAlpha(m_IsOn ? 0f : 1f, instant ? 0f : 0.1f, true);
        }

        // ------- Extended

        /// <summary>
        /// Assume the correct visual state.
        /// </summary>
        protected override void Start() {
            PlayEffect(true);
        }

        private void InternalToggle() {
            if (!IsActive() || !IsInteractable())
                return;

            isOn = !isOn;
        }

        /// <summary>
        /// React to clicks.
        /// </summary>
        public virtual void OnPointerClick(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            InternalToggle();
        }

        public virtual void OnSubmit(BaseEventData eventData) {
            InternalToggle();
        }
    }
}