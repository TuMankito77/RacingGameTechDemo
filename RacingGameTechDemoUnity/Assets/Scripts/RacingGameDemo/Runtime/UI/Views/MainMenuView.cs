namespace RacingGameDemo.Runtime.UI.Views
{
    using UnityEngine;

    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.UI.CoreElements;
    using GameBoxSdk.Runtime.Utils;
    using GameBoxSdk.Runtime.Events;

    public class MainMenuView : BaseView
    {
        [SerializeField]
        private BaseButton startRaceButton = null;

        [SerializeField]
        private BaseButton optionsButton = null;

        [SerializeField]
        private BaseButton garageButton = null;

        [SerializeField]
        private BaseButton quitButton = null;

        public override void TransitionIn(int sourceInteractableGroupId)
        {
            base.TransitionIn(sourceInteractableGroupId);
            startRaceButton.onButtonPressed += OnStartRaceButtonPressed;
            optionsButton.onButtonPressed += OnOptionsButtonPressed;
            garageButton.onButtonPressed += OnGarageButtonPressed;
            quitButton.onButtonPressed += OnQuitButtonPressed;
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
            startRaceButton.onButtonPressed -= OnStartRaceButtonPressed;
            optionsButton.onButtonPressed -= OnOptionsButtonPressed;
            garageButton.onButtonPressed -= OnGarageButtonPressed;
            quitButton.onButtonPressed -= OnQuitButtonPressed;
        }

        private void OnStartRaceButtonPressed()
        {
            LoggerUtil.Log("Start race button pressed.");
            EventDispatcher.Instance.Dispatch(UiEvents.OnStartRaceButtonPressed);
        }

        private void OnOptionsButtonPressed()
        {
            LoggerUtil.Log("Start options button pressed.");
            EventDispatcher.Instance.Dispatch(UiEvents.OnOptionsButtonPressed);
        }

        private void OnGarageButtonPressed()
        {
            LoggerUtil.Log("Start race button pressed.");
            EventDispatcher.Instance.Dispatch(UiEvents.OnGarageButtonPressed);
        }

        private void OnQuitButtonPressed()
        {
            LoggerUtil.Log("Start race button pressed.");
            EventDispatcher.Instance.Dispatch(UiEvents.OnQuitButtonPressed);
        }
    }
}
