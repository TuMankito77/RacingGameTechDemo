namespace RacingGameDemo.Runtime.Core
{
    using System.Collections.Generic;

    using GameBoxSdk.Runtime.Core;
    using GameBoxSdk.Runtime.Localization;
    using GameBoxSdk.Runtime.Sound;
    using GameBoxSdk.Runtime.UI;
    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.Utils;
    
    public class GameManager
    {
        private SystemsInitializer systemsInitializer = null;

        private UiManager uiManager = null;

        public GameManager()
        {
            LoggerUtil.Log("Initializing game!");
            systemsInitializer = new SystemsInitializer();
            systemsInitializer.OnSystemsInitialized += OnSystemsInitialized;
            systemsInitializer.InitializeSystems(GetCoreSystems());
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
            uiManager = systemsInitializer.GetSystem<UiManager>();
            uiManager.DisplayView(ViewIds.MAIN_MENU, disableCurrentInteractableGroup: true);
        }
    }
}
