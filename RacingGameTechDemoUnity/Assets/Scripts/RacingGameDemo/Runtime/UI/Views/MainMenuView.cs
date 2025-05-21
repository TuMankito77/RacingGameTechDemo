namespace RacingGameDemo.Runtime.UI.Views
{
    using UnityEngine;

    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.UI.CoreElements;

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
            startRaceButton.onButtonPressed += OnStartRaceButtonPressed;
            optionsButton.onButtonPressed += OnOptionsButtonPressed;
            garageButton.onButtonPressed += OnGarageButtonPressed;
            quitButton.onButtonPressed += OnQuitButtonPressed;
        }

        protected override void OnDestroy()
        {
            startRaceButton.onButtonPressed -= OnStartRaceButtonPressed;
            optionsButton.onButtonPressed -= OnOptionsButtonPressed;
            garageButton.onButtonPressed -= OnGarageButtonPressed;
            quitButton.onButtonPressed -= OnQuitButtonPressed;
        }

        #endregion

        private void OnStartRaceButtonPressed()
        {

        }

        private void OnOptionsButtonPressed()
        {

        }
        
        private void OnGarageButtonPressed()
        {
            
        }

        private void OnQuitButtonPressed()
        {

        }
    }
}
