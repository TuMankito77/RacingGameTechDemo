namespace RacingGameDemo.Runtime.UI.Views
{
    using System;
    
    using UnityEngine;
    using UnityEngine.InputSystem;
    
    using GameBoxSdk.Runtime.Input;

    public class CarShowcaseViewController : InputController
    {
        public override Type EntityToControlType => typeof(CarShowcaseView);

        private CarShowcaseView carShowCaseView = null;
        private Transform renderCameraTransform = null;
        private float currenPanRotationSpeed = 0;
        private Vector2? lastMousePosition = null;

        public override void Enable(InputActions sourceInputActions, IInputControlableEntity sourceEntityToControl)
        {
            base.Enable(sourceInputActions, sourceEntityToControl);
            carShowCaseView = sourceEntityToControl as CarShowcaseView;
            inputActions.CarShowcaseController.Enable();
            inputActions.CarShowcaseController.ActivateMouseRotation.started += OnActivateMouseRotationStarted;
            inputActions.CarShowcaseController.ActivateMouseRotation.canceled += OnActivateMouseRotationCanceled;
            renderCameraTransform = carShowCaseView.CarShowcaseStudio.CameraCenterParentTransform;
            currenPanRotationSpeed = 0;
        }

        private void OnActivateMouseRotationCanceled(InputAction.CallbackContext inputContext)
        {
            lastMousePosition = null;
        }

        private void OnActivateMouseRotationStarted(InputAction.CallbackContext inputContext)
        {
            lastMousePosition = inputActions.CarShowcaseController.MouseRotate.ReadValue<Vector2>();
        }

        public override void Update()
        {
            base.Update();
            CalculatePanRotationSpeed();
            renderCameraTransform.Rotate(Vector3.up, currenPanRotationSpeed);
        }

        private void CalculatePanRotationSpeed()
        {
            Vector2 panRotationDirection = Vector2.zero;

            if(inputActions.CarShowcaseController.ActivateMouseRotation.IsPressed() && lastMousePosition != null)
            {   
                Vector2 currentMousePosition = inputActions.CarShowcaseController.MouseRotate.ReadValue<Vector2>();
                panRotationDirection = currentMousePosition - lastMousePosition.Value;
                panRotationDirection.Normalize();
                lastMousePosition = currentMousePosition;
            }
            else
            {
                panRotationDirection = inputActions.CarShowcaseController.Rotate.ReadValue<Vector2>();
                panRotationDirection.Normalize();
            }

            float desiredPanRotationSpeed = panRotationDirection.x * carShowCaseView.PanRotationSpeed;

            if(currenPanRotationSpeed < desiredPanRotationSpeed)
            {
                currenPanRotationSpeed = Mathf.Min(currenPanRotationSpeed + carShowCaseView.PanRotationAcceleration * Time.deltaTime, desiredPanRotationSpeed);
            }
            else if(currenPanRotationSpeed > desiredPanRotationSpeed)
            {
                currenPanRotationSpeed = Mathf.Max(currenPanRotationSpeed - carShowCaseView.PanRotationAcceleration * Time.deltaTime, desiredPanRotationSpeed);
            }
        }
    }
}
