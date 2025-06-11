namespace RacingGameDemo.Runtime.UI
{
    using System;
    
    using GameBoxSdk.Runtime.Input;
    using GameBoxSdk.Runtime.UI;
    using GameBoxSdk.Runtime.Events;

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
            Type viewType = uiManager.CurrentViewDisplayed().GetType();

            if (viewType == typeof(MainMenuView) || 
                viewType == typeof(LoadingScreenView))
            {
                return;
            }

            if(viewType == typeof(CarShowcaseView))
            {
                EventDispatcher.Instance.Dispatch(UiEvents.OnExitCarViewButtonPressed);
                return;
            }

            uiManager.RemoveTopStackInteractableGroup();
        }
    }
}
