namespace RacingGameDemo.Runtime.Core
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;

    using GameBoxSdk.Runtime.Core;
    using GameBoxSdk.Runtime.Events;
    using GameBoxSdk.Runtime.Localization;
    using GameBoxSdk.Runtime.Sound;
    using GameBoxSdk.Runtime.UI;
    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.Utils;
    using GameBoxSdk.Runtime.Input;
    
    using RacingGameDemo.Runtime.Gameplay;
    using RacingGameDemo.Runtime.Gameplay.Car;
    using RacingGameDemo.Runtime.UI;
    using RacingGameDemo.Runtime.UI.Views.Data;
    using RacingGameDemo.Runtime.UI.Views;
    using RacingGameDemo.Runtime.Gameplay.Track;

    public class GameManager : IListener
    {
        private RaceData raceData = default(RaceData);
        private SystemsInitializer systemsInitializer = null;
        private ContentLoader contentLoader = null;
        private CameraStackingManager cameraStackingManager = null;
        private LocalizationManager localizationManager = null;
        private AudioManager audioManager = null;
        private UiManager uiManager = null;
        private InputManager inputManager = null;
        private CarsDatabase carsDatabase = null;
        private TracksDatabase tracksDatabase = null;
        private int remainingDatabasesToLoad = 0;

        public GameManager()
        {
            Application.targetFrameRate = 60;
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
            CreateInputControllers();
            uiManager.DisplayView(ViewIds.LoadingScreen, disableCurrentInteractableGroup: false);
            LoadDataBases();
        }

        private void CreateInputControllers()
        {
#if UNITY_ANDROID || UNITY_IOS
            inputManager = new InputManager(enableTouchControls: true);
#else
            inputManager = new InputManager(enableTouchControls: false);
#endif

            InputController[] inputControllers = new InputController[]
            {
                new UiController(),
                new CarShowcaseViewController()
            };

            inputManager.AddInputController(inputControllers);
        }

        private void ShowMainMenu()
        {
            uiManager.DisplayView(ViewIds.MainMenu, disableCurrentInteractableGroup: false);
            inputManager.EnableInput(uiManager);
        }

        private void LoadDataBases()
        {
            remainingDatabasesToLoad++;
            contentLoader.LoadAssetAsynchronously<CarsDatabase>
                (
                    CarsDatabase.CARS_DATABASE_SCRIPTABLE_OBJECT_PATH,
                    (carsDatabaseAsset) =>
                    {
                        carsDatabase = carsDatabaseAsset;
                        carsDatabase.Initialize();
                        remainingDatabasesToLoad--;
                        OnDatabaseLoaded();
                    },
                    null
                );

            remainingDatabasesToLoad++;
            contentLoader.LoadAssetAsynchronously<TracksDatabase>
                (
                    TracksDatabase.TRACKS_DATABASE_SCRIPTABLE_OBJECT_PATH,
                    (tracksDatabaseAsset) =>
                    {
                        tracksDatabase = tracksDatabaseAsset;
                        tracksDatabase.Initialize();
                        remainingDatabasesToLoad--;
                        OnDatabaseLoaded();
                    },
                    null
                );
        }

        private void OnDatabaseLoaded()
        {
            if(remainingDatabasesToLoad <= 0)
            {
                uiManager.RemoveView(ViewIds.LoadingScreen);
                ShowMainMenu();
            }
        }

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch (uiEvent)
            {
                case UiEvents.OnStartRaceButtonPressed:
                    {
                        CarShowcaseViewData carShowcaseViewData = new CarShowcaseViewData(carsDatabase);
                        uiManager.DisplayView(ViewIds.CarShowcase, disableCurrentInteractableGroup: true, carShowcaseViewData);
                        CarSelectionViewData carSelectionViewData = new CarSelectionViewData(carsDatabase, raceData.carIdSelected);
                        uiManager.DisplayView(ViewIds.CarSelection, disableCurrentInteractableGroup: false, carSelectionViewData);
                        break;
                    }

                case UiEvents.OnViewCarButtonPressed:
                    {
                        uiManager.RemoveView(ViewIds.CarSelection);
                        CarShowcaseView carShowcaseView = uiManager.GetTopStackView(ViewIds.CarShowcase) as CarShowcaseView;
                        inputManager.EnableInput(carShowcaseView);
                        break;
                    }

                case UiEvents.OnExitCarViewButtonPressed:
                    {
                        CarShowcaseView carShowcaseView = uiManager.GetTopStackView(ViewIds.CarShowcase) as CarShowcaseView;
                        inputManager.DisableInput(carShowcaseView);
                        CarSelectionViewData carSelectionViewData = new CarSelectionViewData(carsDatabase, raceData.carIdSelected);
                        uiManager.DisplayView(ViewIds.CarSelection, disableCurrentInteractableGroup: false, carSelectionViewData);
                        break;
                    }
                
                case UiEvents.OnCarButtonPressed:
                    {
                        string carId = data as string;
                        raceData.carIdSelected = carId ?? string.Empty;
                        break;
                    }

                case UiEvents.OnSelectCarButtonPressed:
                    {
                        TrackSelectionViewData trackSelectionViewData = new TrackSelectionViewData(tracksDatabase, raceData.trackIdSelected);
                        uiManager.DisplayView(ViewIds.TrackSelection, disableCurrentInteractableGroup: true, trackSelectionViewData);
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
