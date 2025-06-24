namespace RacingGameDemo.Runtime.Gameplay
{
    using System;

    using GameBoxSdk.Runtime.Events;
    using GameBoxSdk.Runtime.Input;

    using RacingGameDemo.Runtime.UI;
    using RacingGameDemo.Runtime.Gameplay.Car;

    public class GameplayController : InputController
    {
        public override Type EntityToControlType => typeof(BaseCar);

        private BaseCar baseCar = null;

        public override void Enable(InputActions sourceInputActions, IInputControlableEntity sourceEntityToControl)
        {
            base.Enable(sourceInputActions, sourceEntityToControl);
            inputActions.GameplayController.Enable();
            inputActions.GameplayController.Pause.performed += OnPauseActionTriggered;
            baseCar = sourceEntityToControl as BaseCar;
        }

        public override void Disable()
        {
            base.Disable();
            inputActions.GameplayController.Pause.performed -= OnPauseActionTriggered;
            inputActions.GameplayController.Disable();
            baseCar = null;
        }
        
        private void OnPauseActionTriggered(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnPauseButtonPressed);
        }
    }
}

