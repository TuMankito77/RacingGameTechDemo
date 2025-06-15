namespace RacingGameDemo.Runtime.UI.Views
{
    using System;
    
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Input;

    public class CarShowcaseViewController : InputController
    {
        public override Type EntityToControlType => typeof(CarShowcaseView);

        private CarShowcaseView carShowCaseView = null;
        private Transform renderCameraTransform = null;
        private float currenPanRotationSpeed = 0;

        public override void Enable(InputActions sourceInputActions, IInputControlableEntity sourceEntityToControl)
        {
            base.Enable(sourceInputActions, sourceEntityToControl);
            carShowCaseView = sourceEntityToControl as CarShowcaseView;
            inputActions.CarShowcaseController.Enable();
            renderCameraTransform = carShowCaseView.CarShowcaseStudio.CameraCenterParentTransform;
            currenPanRotationSpeed = 0;
        }

        public override void Update()
        {
            base.Update();
            CalculatePanRotationSpeed();
            renderCameraTransform.Rotate(Vector3.up, currenPanRotationSpeed);
        }

        private void CalculatePanRotationSpeed()
        {
            Vector2 panRotationDirection = inputActions.CarShowcaseController.Rotate.ReadValue<Vector2>();
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
