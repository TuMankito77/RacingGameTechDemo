namespace RacingGameDemo.Runtime.Core
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.SceneManagement;

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
    using GameBoxSdk.Runtime.SaveTool;
    using RacingGameDemo.Runtime.SotorableClasses;

    public class GameManager : IListener
    {
        private RaceData raceData = default(RaceData);
        private SystemsInitializer systemsInitializer = null;
        private SystemsInitializer raceSystemsInitializer = null;
        private ContentLoader contentLoader = null;
        private CameraStackingManager cameraStackingManager = null;
        private LocalizationManager localizationManager = null;
        private AudioManager audioManager = null;
        private UiManager uiManager = null;
        private InputManager inputManager = null;
        private RaceLevelInitializer raceLevelInitializer = null;
        private StorageAccessor storageAccessor = null;
        private CarsDatabase carsDatabase = null;
        private TracksDatabase tracksDatabase = null;
        private GameSettings gameSettings = null;
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

        private List<BaseSystem> GetRaceLevelSystems()
        {
            BaseCar selectedGameplayCarPrefab = carsDatabase.GetFile(raceData.carIdSelected).GameplayCar;
            raceLevelInitializer = new RaceLevelInitializer(contentLoader, cameraStackingManager, selectedGameplayCarPrefab);

            return new List<BaseSystem>()
            {
                raceLevelInitializer
            };
        }

        private void OnSystemsInitialized()
        {
            systemsInitializer.OnSystemsInitialized -= OnSystemsInitialized;
            CreateInputControllers();
            LoadSavedData();
            audioManager.UpdateMusicVolume(Mathf.Min(gameSettings.MasterVolume, gameSettings.MusicVolume));
            audioManager.UpdateVFXMusicVolume(Math.Min(gameSettings.MasterVolume, gameSettings.SoundEffectsVolume));
            BaseView loadingScreenView = uiManager.DisplayView(ViewIds.LoadingScreen, placeInSeparateInteractableGroup: false);
            loadingScreenView.onTransitionInFinished += () =>
            {
                LoadDataBases();
            };
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
                new CarShowcaseViewController(),
                new GameplayController()
            };

            inputManager.AddInputController(inputControllers);
        }

        private void ShowMainMenu()
        {
            int interactableGroupStackPlacement = uiManager.GetTopStackView(ViewIds.LoadingScreen).InteractableGroupId + 1;
            BaseView mainMenuView = uiManager.DisplayView(ViewIds.MainMenu, placeInSeparateInteractableGroup: true, null, interactableGroupStackPlacement);

            mainMenuView.onTransitionInFinished += () =>
            {
                uiManager.RemoveView(ViewIds.LoadingScreen);
                inputManager.EnableInput(uiManager);
            };
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
                ShowMainMenu();
            }
        }

        private void OnTrackSceneLoaded()
        {
            raceSystemsInitializer = new SystemsInitializer();
            raceSystemsInitializer.OnSystemsInitialized += OnRaceSystemsInitialized;
            raceSystemsInitializer.InitializeSystems(GetRaceLevelSystems());
        }

        private void OnRaceSystemsInitialized()
        {
            inputManager.EnableInput(raceLevelInitializer.GameplayCarInstance);
            uiManager.RemoveView(ViewIds.LoadingScreen);
        }

        private void LoadTrackScene()
        {
            TrackDetails selectedTrackDetails = tracksDatabase.GetFile(raceData.trackIdSelected);
            string trackSceneName = selectedTrackDetails.TrackScene.SceneName;
            contentLoader.LoadScene(trackSceneName, LoadSceneMode.Additive, OnTrackSceneLoaded, setAsMainScene: true);
        }

        private void UnloadTrackScene(Action onTrackSceneUnloaded)
        {
            raceLevelInitializer.Dispose();
            raceLevelInitializer = null;
            raceSystemsInitializer.Dispose();
            raceSystemsInitializer = null;
            TrackDetails selectedTrackDetails = tracksDatabase.GetFile(raceData.trackIdSelected);
            string trackSceneName = selectedTrackDetails.TrackScene.SceneName;
            contentLoader.UnloadScene(trackSceneName, onTrackSceneUnloaded);
        }

        private void HandleUiEvents(UiEvents uiEvent, object data)
        {
            switch (uiEvent)
            {
                case UiEvents.OnStartRaceButtonPressed:
                    {
                        CarShowcaseViewData carShowcaseViewData = new CarShowcaseViewData(carsDatabase);
                        uiManager.DisplayView(ViewIds.CarShowcase, placeInSeparateInteractableGroup: true, carShowcaseViewData);
                        CarSelectionViewData carSelectionViewData = new CarSelectionViewData(carsDatabase, raceData.carIdSelected);
                        uiManager.DisplayView(ViewIds.CarSelection, placeInSeparateInteractableGroup: false, carSelectionViewData);
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
                        uiManager.DisplayView(ViewIds.CarSelection, placeInSeparateInteractableGroup: false, carSelectionViewData);
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
                        uiManager.DisplayView(ViewIds.TrackSelection, placeInSeparateInteractableGroup: true, trackSelectionViewData);
                        break;
                    }

                case UiEvents.OnTrackButtonPressed:
                    {
                        string trackId = data as string;
                        raceData.trackIdSelected = trackId ?? string.Empty;
                        break;
                    }

                case UiEvents.OnSelectTrackButtonPressed:
                    {
                        inputManager.DisableInput(uiManager);

                        BaseView loadingScreenView = uiManager.DisplayView(ViewIds.LoadingScreen, placeInSeparateInteractableGroup: true);

                        //NOTE: We are waiting for the removal of this view since all transition outs have the same duration in the current open views and we don't want to see
                        //leftover elements from other views if the race level is loded before these views have finished their transition out animations. 
                        loadingScreenView.onTransitionInFinished += () =>
                        {
                            LoadTrackScene();
                        };

                        uiManager.RemoveView(ViewIds.TrackSelection);
                        uiManager.RemoveView(ViewIds.CarSelection);
                        uiManager.RemoveView(ViewIds.CarShowcase);
                        uiManager.RemoveView(ViewIds.MainMenu);
                        break;
                    }

                case UiEvents.OnPauseButtonPressed:
                    {
                        inputManager.DisableInput(raceLevelInitializer.GameplayCarInstance);
                        inputManager.EnableInput(uiManager);
                        uiManager.DisplayView(ViewIds.PauseMenu, placeInSeparateInteractableGroup: true);
                        break;
                    }

                case UiEvents.OnContinueRaceButtonPressed:
                case UiEvents.OnUnpuaseButtonPressed:
                    {
                        inputManager.DisableInput(uiManager);
                        inputManager.EnableInput(raceLevelInitializer.GameplayCarInstance);
                        uiManager.RemoveView(ViewIds.PauseMenu);
                        break;
                    }

                case UiEvents.OnRestartRaceButtonPressed:
                    {
                        string message = localizationManager.GetLocalizedText("RestartRaceLocalizedMessage.");
                        MessageWindowViewData messageWindowViewData = new MessageWindowViewData(message, displayCancelButton: true);
                        MessageWindowView messageView = uiManager.DisplayView(ViewIds.MessageWindow, placeInSeparateInteractableGroup: true, messageWindowViewData) as MessageWindowView;
                        messageView.ConfirmButton.onButtonPressed += RestartRace;
                        break;
                    }

                case UiEvents.OnQuitRaceButtonPressed:
                    {
                        string message = localizationManager.GetLocalizedText("QuitRaceLocalizedMessage.");
                        MessageWindowViewData messageWindowViewData = new MessageWindowViewData(message, displayCancelButton: true);
                        MessageWindowView messageView = uiManager.DisplayView(ViewIds.MessageWindow, placeInSeparateInteractableGroup: true, messageWindowViewData) as MessageWindowView;
                        messageView.ConfirmButton.onButtonPressed += QuitRace;
                        break;
                    }

                case UiEvents.OnOptionsButtonPressed:
                    {
                        OptionsMenuViewData optionsMenuViewData = new OptionsMenuViewData(gameSettings.MasterVolume, gameSettings.MusicVolume, gameSettings.SoundEffectsVolume);
                        uiManager.DisplayView(ViewIds.OptionsMenu, placeInSeparateInteractableGroup: true, optionsMenuViewData, interactableGroupStackPlacement: 0);
                        break;
                    }

                case UiEvents.OnMasterVolumeChanged:
                    {
                        float volume = (float)data;
                        audioManager.UpdateMusicVolume(Mathf.Min(volume,gameSettings.MusicVolume));
                        audioManager.UpdateVFXMusicVolume(Mathf.Min(volume, gameSettings.SoundEffectsVolume));
                        gameSettings.MasterVolume = volume;
                        storageAccessor.Save(gameSettings);
                        break;
                    }

                case UiEvents.OnMusicVolumeChanged:
                    {
                        float volume = (float)data;
                        audioManager.UpdateMusicVolume(Mathf.Min(volume, gameSettings.MusicVolume));
                        gameSettings.MusicVolume = volume;
                        storageAccessor.Save(gameSettings);
                        break;
                    }

                case UiEvents.OnSoundEffectsVolumeChanged:
                    {
                        float volume = (float)data;
                        audioManager.UpdateVFXMusicVolume(Mathf.Min(volume, gameSettings.SoundEffectsVolume));
                        gameSettings.SoundEffectsVolume = volume;
                        storageAccessor.Save(gameSettings);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        private void RestartRace()
        {
            inputManager.DisableInput(uiManager);
            BaseView loadingScreenView = uiManager.DisplayView(ViewIds.LoadingScreen, placeInSeparateInteractableGroup: true);

            loadingScreenView.onTransitionInFinished += () =>
            {
                uiManager.RemoveView(ViewIds.MessageWindow);
                uiManager.RemoveView(ViewIds.PauseMenu);
                UnloadTrackScene(onTrackSceneUnloaded: LoadTrackScene);
            };
        }
        
        private void QuitRace()
        {
            inputManager.DisableInput(uiManager);
            BaseView loadingScreenView = uiManager.DisplayView(ViewIds.LoadingScreen, placeInSeparateInteractableGroup: true);

            loadingScreenView.onTransitionInFinished += () =>
            {
                uiManager.RemoveView(ViewIds.MessageWindow);
                uiManager.RemoveView(ViewIds.PauseMenu);
                UnloadTrackScene(onTrackSceneUnloaded: ShowMainMenu);
            };
        }

        private void LoadSavedData()
        {
            gameSettings = new GameSettings
                (
                    sourceMasterVolume: 1,
                    sourceMusicVolume: 1,
                    sourceSoundEffectsVolume: 1
                );

            storageAccessor = new StorageAccessor();

            if(storageAccessor.DoesInformationExist(gameSettings.Key))
            {
                GameSettings gameSettignsFound = storageAccessor.Load<GameSettings>(gameSettings.Key);
                gameSettignsFound.TransferValidValues(ref gameSettings);
            }
        }
    }
}
