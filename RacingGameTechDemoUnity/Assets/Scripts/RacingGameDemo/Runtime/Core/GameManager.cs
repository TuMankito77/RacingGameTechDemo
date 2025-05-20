namespace RacingGameDemo.Runtime.Core
{
    using System.Collections.Generic;

    using GameBoxSdk.Runtime.Core;
    using GameBoxSdk.Runtime.Utils;
    
    public class GameManager
    {
        private SystemsInitializer systemsInitializer = null;

        public GameManager()
        {
            LoggerUtil.Log("Initializing game!");
            systemsInitializer = new SystemsInitializer();
            systemsInitializer.InitializeSystems(GetCoreSystems());
        }

        private List<BaseSystem> GetCoreSystems()
        {
            return new List<BaseSystem>()
            {
                
            };
        }
    }
}
