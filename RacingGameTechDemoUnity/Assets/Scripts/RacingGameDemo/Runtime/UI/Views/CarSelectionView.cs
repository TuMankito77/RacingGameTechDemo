namespace RacingGameDemo.Runtime.UI.Views
{
    using System;
    using System.Collections.Generic;
    
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
        private struct CarIdButtonPair
        {
            public string id;
            public BaseButton button;
        }

        [SerializeField]
        private BaseButton carOptionButtonPrefab = null;
        
        [SerializeField]
        private Transform carButtonsContainer = null;

        [SerializeField]
        private BaseButton selectCarButton = null;

        private CarsDatabase carsDatabase = null;
        private string carSelected = string.Empty;
        private List<CarIdButtonPair> carIdButtonPairs = null;

        public override void Initialize(Camera uiCamera, Action<ClipIds> playClipOnce, ViewInjectableData viewInjectableData, Func<string, string> getLocalizedText, EventSystem sourceEventSystem)
        {
            base.Initialize(uiCamera, playClipOnce, viewInjectableData, getLocalizedText, sourceEventSystem);

            carIdButtonPairs = new List<CarIdButtonPair>();
            CarSelectionViewData carSelectionViewData = viewInjectableData as CarSelectionViewData;

            if(carSelectionViewData != null)
            {
                carsDatabase = carSelectionViewData.CarsDatabase;
            }

            foreach(string carId in carsDatabase.Ids)
            {
                CarDetails carDetails = carsDatabase.GetFile(carId);
                BaseButton carButton = Instantiate(carOptionButtonPrefab, carButtonsContainer);

                carIdButtonPairs.Add(new CarIdButtonPair() 
                { 
                    id = carId, 
                    button = carButton 
                } );

                string localizedName = getLocalizedText(carDetails.DisplayNameLocKey);
                carButton.UpdateButtonText(localizedName);

                void OnCarButtonPressed()
                {
                    EventDispatcher.Instance.Dispatch(UiEvents.OnCarButtonPressed, carId);
                    carSelected = carId;
                    UpdateCarButtonSelected();
                    //Update the car being displayed on the screen.
                }

                carButton.onButtonPressed += OnCarButtonPressed;
                carButton.SetInteractable(carId != carSelectionViewData.LastCarIdSelected);
            }

            if(carSelectionViewData.LastCarIdSelected == null)
            {
                EventDispatcher.Instance.Dispatch(UiEvents.OnCarButtonPressed, carsDatabase.Ids[0]);
                carIdButtonPairs[0].button.SetInteractable(false);
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

        private void UpdateCarButtonSelected()
        {
            foreach(CarIdButtonPair carIdButtonPair in carIdButtonPairs)
            {
                carIdButtonPair.button.SetInteractable(carSelected != carIdButtonPair.id);
            }
        }
    }
}

