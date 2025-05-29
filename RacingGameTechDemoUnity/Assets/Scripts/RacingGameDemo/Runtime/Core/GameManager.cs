namespace RacingGameDemo.Runtime.Core
{
    using System;
    using System.Collections.Generic;

    using GameBoxSdk.Runtime.Core;
    using GameBoxSdk.Runtime.Events;
    using GameBoxSdk.Runtime.Localization;
    using GameBoxSdk.Runtime.Sound;
    using GameBoxSdk.Runtime.UI;
    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.Utils;
    
    using RacingGameDemo.Runtime.Gameplay;
    using RacingGameDemo.Runtime.Gameplay.Car;
    using RacingGameDemo.Runtime.UI;
    using RacingGameDemo.Runtime.UI.Views.Data;

    public class GameManager : IListener
    {
        private const string CARS_DATABASE_PATH = "RacingGameDemo/Cars/CarsDatabase";

        private RaceData raceData = default(RaceData);
        private SystemsInitializer systemsInitializer = null;
        private ContentLoader contentLoader = null;
        private CameraStackingManager cameraStackingManager = null;
        private LocalizationManager localizationManager = null;
        private AudioManager audioManager = null;
        private UiManager uiManager = null;

        public GameManager()
        {
            LoggerUtil.Log("Initializing game!");
            systemsInitializer = new SystemsInitializer();
            systemsInitializer.OnSystemsInitialized += OnSystemsInitialized;
            systemsInitializer.InitializeSystems(GetCoreSystems());
            EventDispatcher.Instance.AddListener(this, typeof(UiEvents));
        }

        ~GameManager()
        {
            EventDispatcher.Instance.RemoveListener(this, typeof(UiEvents));
        }

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

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch(uiEvent)
            {
                case UiEvents.OnStartRaceButtonPressed:
                    {
                        uiManager.RemoveView(ViewIds.MainMenu);
                        uiManager.DisplayView(ViewIds.LoadingScreen, disableCurrentInteractableGroup: true);
                        contentLoader.LoadAssetAsynchronously<CarsDatabase>(CARS_DATABASE_PATH, OnCarsDatabaseLoaded, null);
                        break;
                    }

                case UiEvents.OnCarButtonPressed:
                    {
                        string carId = data as string;
                        raceData.carIdSelected = carId ?? string.Empty;
                        break;
                    }

                default:
                    {
                        break;
                    }
            }    
        }

        private List<BaseSystem> GetCoreSystems()
        {
            contentLoader = new ContentLoader();
            cameraStackingManager = new CameraStackingManager();
            localizationManager = new LocalizationManager();
            audioManager = new AudioManager();
            uiManager = new UiManager(localizationManager.GetLocalizedText, audioManager.PlayGeneralClip);

            return new List<BaseSystem>()
            {
                contentLoader,
                cameraStackingManager,
                localizationManager
                    .AddDependency<ContentLoader>(),
                audioManager
                    .AddDependency<ContentLoader>(),
                uiManager
                    .AddDependency<ContentLoader>()
                    .AddDependency<CameraStackingManager>()
            };
        }

        private void OnSystemsInitialized()
        {
            systemsInitializer.OnSystemsInitialized -= OnSystemsInitialized;
            uiManager.RemoveView(ViewIds.LoadingScreen);
            uiManager.DisplayView(ViewIds.MainMenu, disableCurrentInteractableGroup: false);
        }

        private void OnCarsDatabaseLoaded(CarsDatabase carsDatabase)
        {
            carsDatabase.Initialize();
            CarSelectionViewData carSelectionViewData = new CarSelectionViewData(carsDatabase, raceData.carIdSelected);
            uiManager.DisplayView(ViewIds.CarSelection, disableCurrentInteractableGroup: true, carSelectionViewData);
        }
    }
}
