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
    using RacingGameDemo.Runtime.UI;

    public class GameManager : IListener
    {
        private SystemsInitializer systemsInitializer = null;

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
                        uiManager.DisplayView(ViewIds.CarSelection, disableCurrentInteractableGroup: true);
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
            return new List<BaseSystem>()
            {
                new ContentLoader(),
                new CameraStackingManager(),
                new LocalizationManager()
                    .AddDependency<ContentLoader>(),
                new AudioManager()
                    .AddDependency<ContentLoader>(),
                new UiManager()
                    .AddDependency<ContentLoader>()
                    .AddDependency<LocalizationManager>()
                    .AddDependency<AudioManager>()
                    .AddDependency<CameraStackingManager>()
            };
        }

        private void OnSystemsInitialized()
        {
            systemsInitializer.OnSystemsInitialized -= OnSystemsInitialized;
            uiManager = systemsInitializer.GetSystem<UiManager>();
            uiManager.DisplayView(ViewIds.MainMenu, disableCurrentInteractableGroup: false);
        }
    }
}
