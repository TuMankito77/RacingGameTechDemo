namespace GameBoxSdk.Runtime.UI.CoreElements
{
    using System;

    using UnityEngine;
    using UnityEngine.UI;

    public class BaseButton : SelectableElement
    {
        public event Action onButtonPressed = null;

        [SerializeField]
        private Button button = null;

        [SerializeField]
        private Text buttonText = null;

        [SerializeField]
        private bool triggerButtonActionBeforeAnimation = false;

        private ISeletableElementAnimator buttonAniator = null;

        #region Unity Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            onSubmit += OnButtonPressed;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if(buttonAniator != null)
            {
                buttonAniator.OnSubmitAnimationStart -= onButtonPressed;
                buttonAniator.OnSubmitAnimationEnd -= onButtonPressed;
            }
        }

        #endregion

        public override void SetAnimationExternalModule(ISeletableElementAnimator sourceButtonAnimator)
        {
            buttonAniator = sourceButtonAnimator;
            onSubmit -= OnButtonPressed;

            if(triggerButtonActionBeforeAnimation)
            {
                buttonAniator.OnSubmitAnimationStart += OnButtonPressed;
            }
            else
            {
                buttonAniator.OnSubmitAnimationEnd += OnButtonPressed;
            }
        }

        public override void SetInteractable(bool isActive)
        {
            base.SetInteractable(isActive);
            button.interactable = isActive;
        }

        public void UpdateButtonText(string text)
        {
            buttonText.text = text;
        }

        protected override void CheckNeededComponents()
        {
            base.CheckNeededComponents();
            AddComponentIfNotFound(ref button);
        }

        private void OnButtonPressed()
        {
            onButtonPressed?.Invoke();
        }
    }
}
