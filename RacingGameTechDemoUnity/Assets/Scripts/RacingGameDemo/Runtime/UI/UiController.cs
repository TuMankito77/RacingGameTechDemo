namespace RacingGameDemo.Runtime.UI
{
    using System;
    
    using GameBoxSdk.Runtime.Input;
    using GameBoxSdk.Runtime.UI;
    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.Utils;
    using RacingGameDemo.Runtime.UI.Views;

    public class UiController : InputController
    {
        public override Type EntityToControlType => typeof(UiManager);

        private UiManager uiManager = null;

        public override void Enable(InputActions sourceInputActions, IInputControlableEntity sourceEntityToControl)
        {
            base.Enable(sourceInputActions, sourceEntityToControl);
            inputActions.UiController.Enable();
            inputActions.UiController.GoBack.performed += OnGoBackActionPerformed;
            inputActions.UiController.Navigate.performed += OnNavigateActionPerformed;
            uiManager = sourceEntityToControl as UiManager;
        }

        public override void Disable()
        {
            base.Disable();
        }

        private void OnNavigateActionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
        }

        private void OnGoBackActionPerformed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            LoggerUtil.Log("Go back action performed");
            Type viewType = uiManager.CurrentViewDisplayed().GetType();

            if (viewType == typeof(MainMenuView))
            {
                return;
            }

            uiManager.RemoveTopStackInteractableGroup();
        }
    }
}
