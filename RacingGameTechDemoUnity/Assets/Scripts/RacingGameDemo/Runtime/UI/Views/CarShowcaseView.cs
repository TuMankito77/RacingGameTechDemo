namespace RacingGameDemo.Runtime.UI.Views
{
    using System;
    
    using UnityEngine;
    using UnityEngine.EventSystems;
    
    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.UI.WorldElements;
    using GameBoxSdk.Runtime.Sound;
    using GameBoxSdk.Runtime.UI.Views.DataContainers;
    using GameBoxSdk.Runtime.Events;
    using GameBoxSdk.Runtime.Utils;
    using GameBoxSdk.Runtime.UI.CoreElements;
    using GameBoxSdk.Runtime.Input;

    using RacingGameDemo.Runtime.Gameplay.Car;
    using RacingGameDemo.Runtime.UI.Views.Data;

    public class CarShowcaseView : BaseView, IListener, IInputControlableEntity
    {
        [SerializeField]
        private ModelShowcaseStudio modelShocaseStudioPrefab = null;

        [SerializeField, Min(0)]
        private float panRotationSpeed = 10;

        [SerializeField, Min(0)]
        private float panRotationAcceleration = 1;

        [SerializeField]
        private BaseButton exitCarViewButton = null;

        private CarsDatabase carsDatabase = null;

        public ModelShowcaseStudio ModelShowcaseStudioInstance { get; private set; } = null;
        public float PanRotationSpeed => panRotationSpeed;
        public float PanRotationAcceleration => panRotationAcceleration;

        #region IListener

        public void HandleEvent(IComparable eventName, object data)
        {
            switch(eventName)
            {
                case UiEvents uiEvent:
                    {
                        HandleUiEvents(uiEvent, data);
                        break;
                    }

                default:
                    {
                        LoggerUtil.LogError($"{GetType()} - The event {eventName} is not handled by this class. You may need to unsubscribe.");
                        break;
                    }
            }
        }

        #endregion

        public override void Initialize(Camera uiCamera, Action<ClipIds> playClipOnce, ViewInjectableData viewInjectableData, Func<string, string> getLocalizedText, EventSystem sourceEventSystem)
        {
            base.Initialize(uiCamera, playClipOnce, viewInjectableData, getLocalizedText, sourceEventSystem);

            CarShowcaseViewData carShowcaseViewData = viewInjectableData as CarShowcaseViewData;

            if(carShowcaseViewData != null)
            {
                carsDatabase = carShowcaseViewData.CarsDatabase;
            }
        }

        public override void TransitionIn(int sourceInteractableGroupId)
        {
            base.TransitionIn(sourceInteractableGroupId);
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents));
            ModelShowcaseStudioInstance = Instantiate(modelShocaseStudioPrefab);
            exitCarViewButton.onButtonPressed += OnExitCarViewButtonPressed;
            onTransitionOutFinished += OnTransitionOutFinished;
        }

        private void OnTransitionOutFinished()
        {
            exitCarViewButton.onButtonPressed -= OnExitCarViewButtonPressed;
            onTransitionOutFinished -= OnTransitionOutFinished;
            Destroy(ModelShowcaseStudioInstance.gameObject);
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
            EventDispatcher.Instance.RemoveListener(this, typeof(UiEvents));
        }

        private void OnExitCarViewButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnExitCarViewButtonPressed);
        }

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch(uiEvent)
            {
                case UiEvents.OnCarButtonPressed:
                    {
                        string carId = data as string;

                        if(carId == null)
                        {
                            return;
                        }

                        GameObject carPrefab = carsDatabase.GetFile(carId).CarPrefab;
                        ModelShowcaseStudioInstance.UpdateModelDisplayed(carPrefab);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }    
        }
    }    
}

