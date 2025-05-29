namespace RacingGameDemo.Runtime.UI.Views
{
    using System;
    
    using UnityEngine;
    using UnityEngine.EventSystems;

    using GameBoxSdk.Runtime.Sound;
    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.UI.Views.DataContainers;
    using GameBoxSdk.Runtime.UI.CoreElements;
    using GameBoxSdk.Runtime.Events;
    
    using RacingGameDemo.Runtime.Gameplay.Car;
    using RacingGameDemo.Runtime.UI.Views.Data;

    public class CarSelectionView : BaseView
    {
        [SerializeField]
        private BaseButton carOptionButtonPrefab = null;
        
        [SerializeField]
        private Transform carButtonsContainer = null;

        [SerializeField]
        private BaseButton selectCarButton = null;

        private CarsDatabase carsDatabase = null;
        private string carSelected = string.Empty;

        public override void Initialize(Camera uiCamera, Action<ClipIds> playClipOnce, ViewInjectableData viewInjectableData, Func<string, string> getLocalizedText, EventSystem sourceEventSystem)
        {
            base.Initialize(uiCamera, playClipOnce, viewInjectableData, getLocalizedText, sourceEventSystem);

            CarSelectionViewData carSelectionViewData = viewInjectableData as CarSelectionViewData;

            if(carSelectionViewData != null)
            {
                carsDatabase = carSelectionViewData.CarsDatabase;
            }

            foreach(string carId in carsDatabase.Ids)
            {
                CarDetails carDetails = carsDatabase.GetFile(carId);
                BaseButton carButton = Instantiate(carOptionButtonPrefab, carButtonsContainer);
                string localizedName = getLocalizedText(carDetails.DisplayNameLocKey);
                carButton.UpdateButtonText(localizedName);

                void OnCarButtonPressed()
                {
                    carButton.onButtonPressed -= OnCarButtonPressed;
                    EventDispatcher.Instance.Dispatch(UiEvents.OnCarButtonPressed, carId);
                    carSelected = carId;
                    //Update the car being displayed on the screen.
                }

                carButton.onButtonPressed += OnCarButtonPressed;
            }

            if(carSelectionViewData.LastCarIdSelected != null)
            {
                EventDispatcher.Instance.Dispatch(UiEvents.OnCarButtonPressed, carSelectionViewData.LastCarIdSelected);
            }
            else
            {
                EventDispatcher.Instance.Dispatch(UiEvents.OnCarButtonPressed, carsDatabase.Ids[0]);
            }
        }

        public override void TransitionIn(int sourceInteractableGroupId)
        {
            base.TransitionIn(sourceInteractableGroupId);
            selectCarButton.onButtonPressed += OnSelectCarButtonPressed;
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
            selectCarButton.onButtonPressed -= OnSelectCarButtonPressed;
        }

        private void OnSelectCarButtonPressed()
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnSelectCarButtonPressed, carSelected);
        }
    }
}

