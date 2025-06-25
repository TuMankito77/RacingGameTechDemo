namespace RacingGameDemo.Runtime.UI.Views
{
    using UnityEngine;

    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.UI.CoreElements;
    using GameBoxSdk.Runtime.Events;

    public class PauseMenuView : BaseView
    {
        [SerializeField]
        private BaseButton continueRaceButton = null;

        [SerializeField]
        private BaseButton restartRaceButton = null;

        [SerializeField]
        private BaseButton optionsButton = null;

        [SerializeField]
        private BaseButton quitRaceButton = null;

        public override void TransitionIn(int sourceInteractableGroupId)
        {
            base.TransitionIn(sourceInteractableGroupId);
            continueRaceButton.onButtonPressed += OnContinueRaceButtonPressed;
            restartRaceButton.onButtonPressed += OnRestartRaceButtonPressed;
            optionsButton.onButtonPressed += OnOptionsButtonPressed;
            quitRaceButton.onButtonPressed += OnQuitRaceButtonPressed;
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
            continueRaceButton.onButtonPressed -= OnContinueRaceButtonPressed;
            restartRaceButton.onButtonPressed -= OnRestartRaceButtonPressed;
            optionsButton.onButtonPressed -= OnOptionsButtonPressed;
            quitRaceButton.onButtonPressed -= OnQuitRaceButtonPressed;
        }

        private void OnContinueRaceButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnContinueRaceButtonPressed);
        }

        private void OnRestartRaceButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnRestartRaceButtonPressed);
        }

        private void OnOptionsButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnOptionsButtonPressed);
        }

        private void OnQuitRaceButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnQuitRaceButtonPressed);
        }
    }
}

