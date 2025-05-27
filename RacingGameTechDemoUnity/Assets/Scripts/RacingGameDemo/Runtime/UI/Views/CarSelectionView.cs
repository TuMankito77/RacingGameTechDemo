namespace RacingGameDemo.Runtime.UI.Views
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    
    using GameBoxSdk.Runtime.Localization;
    using GameBoxSdk.Runtime.Sound;
    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.UI.Views.DataContainers;
    using GameBoxSdk.Runtime.UI.CoreElements;
    
    using RacingGameDemo.Runtime.Gameplay.Car;
    using RacingGameDemo.Runtime.UI.Views.Data;
    using System;

    public class CarSelectionView : BaseView
    {
        [SerializeField]
        private BaseButton carOptionButtonPrefab = null;
        
        [SerializeField]
        private Transform carButtonsContainer = null;

        private CarsDatabase carsDatabase = null;

        public override void Initialize(Camera uiCamera, Action<ClipIds> playClipOnce, ViewInjectableData viewInjectableData, Func<string, string> getLocalizedText, EventSystem sourceEventSystem)
        {
            base.Initialize(uiCamera, playClipOnce, viewInjectableData, getLocalizedText, sourceEventSystem);

            CarSelectionViewData carSelectionViewData = viewInjectableData as CarSelectionViewData;

            if(carSelectionViewData != null)
            {
                carsDatabase = carSelectionViewData.CarsDatabase;
            }

            foreach(string id in carsDatabase.Ids)
            {
                BaseButton button = Instantiate(carOptionButtonPrefab, carButtonsContainer);
            }
        }
    }
}

