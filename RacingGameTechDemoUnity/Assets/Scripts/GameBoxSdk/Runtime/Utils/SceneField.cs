namespace GameBoxSdk.Runtime.Utils
{
    using System;

    using UnityEngine;

    using UObject = UnityEngine.Object;

    [Serializable]
    public class SceneField
    {
        [SerializeField]
        private UObject scene = null;

        [SerializeField]
        private string sceneName = string.Empty;

        public UObject Scene => scene;
        public string SceneName => sceneName;
    }
}

