namespace GameBoxSdk.Runtime.Input.TouchControls
{
    using System;
    
    using UnityEngine;
    using UnityEngine.InputSystem.EnhancedTouch;
    using UnityEngine.InputSystem.Layouts;
    using UnityEngine.InputSystem.OnScreen;
    using ETouch = UnityEngine.InputSystem.EnhancedTouch;

    public class Joystick : OnScreenControl
    {
        public event Action<Vector2> OnTouchStart = null;
        public event Action<Vector2> OnTouchDrag = null;
        public event Action<Vector2> OnTouchEnd = null;

        [InputControl(layout = "Button")]
        [SerializeField]
        private string controlPathSelected;

        [SerializeField, Min(0)]
        private float deadzoneRadius = 0;

        [SerializeField, Range(0.1f, 1)]
        private float horizontalUsableSpacePercentage = 0.5f;

        [SerializeField, Range(0.1f, 1)]
        private float verticalUsableSpacePercentage = 0.5f;
        
        private Finger fingerMovement = null;
        private Vector2 initialPosition = Vector2.zero;
        private Vector2 movementAmount = Vector2.zero;

        public float DeadzoneRadius { get => deadzoneRadius; set => deadzoneRadius = value; }

        protected override string controlPathInternal 
        { 
            get => controlPathSelected;
            set => controlPathSelected = value;
        }

        public void RegisterToTouchEvents()
        {
            ETouch.Touch.onFingerDown += OnFingerDown;
            ETouch.Touch.onFingerMove += OnFingerMove;
            ETouch.Touch.onFingerUp += OnFingerUp;

            //Making sure the movement is cleaned up when we are start
            if(movementAmount.magnitude > 0)
            {
                movementAmount = Vector2.zero;
                SendValueToControl(movementAmount);
            }
        }

        public void DeregisterFromTouchEvents()
        {
            ETouch.Touch.onFingerDown -= OnFingerDown;
            ETouch.Touch.onFingerMove -= OnFingerMove;
            ETouch.Touch.onFingerUp -= OnFingerUp;
            
            //Making sure the movement is cleaned up when we are done
            if(fingerMovement != null)
            {
                fingerMovement = null;
                movementAmount = Vector2.zero;
                SendValueToControl(movementAmount);
                //In case the last touch end is not sent becuase it was cut all of a sudden.
                OnTouchEnd?.Invoke(movementAmount);

            }
        }

        private void OnFingerDown(Finger finger)
        {
            if(fingerMovement != null)
            {
                return;
            }

            if(finger.screenPosition.x > Screen.width * horizontalUsableSpacePercentage ||
               finger.screenPosition.y > Screen.height * verticalUsableSpacePercentage)
            {
                return;
            }
            
            fingerMovement = finger;
            initialPosition = finger.screenPosition;
            OnTouchStart?.Invoke(initialPosition);
        }

        private void OnFingerMove(Finger finger)
        {
            if(fingerMovement != finger)
            {
                return;
            }

            movementAmount = finger.screenPosition - initialPosition;
            OnTouchDrag?.Invoke(finger.screenPosition);
            
            if(movementAmount.magnitude > deadzoneRadius)
            {
                SendValueToControl(movementAmount.normalized);
            }
            else
            {
                SendValueToControl(Vector2.zero);
            }
        }

        private void OnFingerUp(Finger finger)
        {
            if(fingerMovement != finger)
            {
                return;
            }

            fingerMovement = null;
            movementAmount = Vector2.zero;
            SendValueToControl(movementAmount);
            OnTouchEnd?.Invoke(finger.screenPosition);
        }

        //NOTE: We are deregistering when the object is destroyed since the editor could be stopped at any point
        //but this class will still be listening to the simulated touch input.
#if UNITY_EDITOR

        #region Unity Methods

        private void OnDestroy()
        {
            DeregisterFromTouchEvents();
        }

        #endregion
#endif

    }
}

