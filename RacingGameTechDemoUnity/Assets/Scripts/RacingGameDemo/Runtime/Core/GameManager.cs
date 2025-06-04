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
        private InputManager inputManager = null;

        public GameManager()
        {
            Application.targetFrameRate = 60;
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
            ShowMainMenu();
        }

        private void OnCarsDatabaseLoaded(CarsDatabase carsDatabase)
        {
            carsDatabase.Initialize();
            uiManager.RemoveView(ViewIds.LoadingScreen);
            inputManager.EnableInput(uiManager);
            CarShowcaseViewData carShowcaseViewData = new CarShowcaseViewData(carsDatabase);
            uiManager.DisplayView(ViewIds.CarShowcase, disableCurrentInteractableGroup: true, carShowcaseViewData);
            CarSelectionViewData carSelectionViewData = new CarSelectionViewData(carsDatabase, raceData.carIdSelected);
            uiManager.DisplayView(ViewIds.CarSelection, disableCurrentInteractableGroup: false, carSelectionViewData);
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
                new UiController()
            };

            inputManager.AddInputController(inputControllers);
        }

        private void ShowMainMenu()
        {
            uiManager.DisplayView(ViewIds.MainMenu, disableCurrentInteractableGroup: false);
            inputManager.EnableInput(uiManager);
        }

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch (uiEvent)
            {
                case UiEvents.OnStartRaceButtonPressed:
                    {
                        inputManager.DisableInput(uiManager);
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

                case UiEvents.OnSelectCarButtonPressed:
                    {
                        //Display the track selection view.
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
