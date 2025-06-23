namespace RacingGameDemo.Runtime.UI.Views
{
    using UnityEngine;

    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.UI.CoreElements;

    public class PauseMenuView : BaseView
    {
        [SerializeField]
        private BaseButton continueButton = null;

        [SerializeField]
        private BaseButton restartRaceButton = null;

        [SerializeField]
        private BaseButton optionsButton = null;

        [SerializeField]
        private BaseButton quitButton = null;

        public override void TransitionIn(int sourceInteractableGroupId)
        {
            base.TransitionIn(sourceInteractableGroupId);
            continueButton.onButtonPressed += OnContinueButtonPressed;
            restartRaceButton.onButtonPressed += OnRestartRaceButtonPressed;
            optionsButton.onButtonPressed += OnOptionsButtonPressed;
            quitButton.onButtonPressed += OnQuitButtonPressed;
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
            continueButton.onButtonPressed -= OnContinueButtonPressed;
            restartRaceButton.onButtonPressed -= OnRestartRaceButtonPressed;
            optionsButton.onButtonPressed -= OnOptionsButtonPressed;
            quitButton.onButtonPressed -= OnQuitButtonPressed;
        }

        private void OnContinueButtonPressed()
        {
            
        }

        private void OnRestartRaceButtonPressed()
        {
            
        }

        private void OnOptionsButtonPressed()
        {
            
        }

        private void OnQuitButtonPressed()
        {
            
        }
    }
}

