namespace RacingGameDemo.Runtime.UI.Views
{
    using UnityEngine;

    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.UI.CoreElements;
    using GameBoxSdk.Runtime.Utils;

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

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            startRaceButton.onButtonPressed += OnStartRaceButtonPressed;
            optionsButton.onButtonPressed += OnOptionsButtonPressed;
            garageButton.onButtonPressed += OnGarageButtonPressed;
            quitButton.onButtonPressed += OnQuitButtonPressed;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            startRaceButton.onButtonPressed -= OnStartRaceButtonPressed;
            optionsButton.onButtonPressed -= OnOptionsButtonPressed;
            garageButton.onButtonPressed -= OnGarageButtonPressed;
            quitButton.onButtonPressed -= OnQuitButtonPressed;
        }

        #endregion

        private void OnStartRaceButtonPressed()
        {
            LoggerUtil.Log("Start race button pressed.");
        }

        private void OnOptionsButtonPressed()
        {
            LoggerUtil.Log("Start options button pressed.");
        }

        private void OnGarageButtonPressed()
        {
            LoggerUtil.Log("Start race button pressed.");
        }

        private void OnQuitButtonPressed()
        {
            LoggerUtil.Log("Start race button pressed.");
        }
    }
}
