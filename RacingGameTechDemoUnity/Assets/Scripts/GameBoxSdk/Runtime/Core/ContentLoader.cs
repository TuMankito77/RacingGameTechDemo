namespace GameBoxSdk.Runtime.Core
{
    using System;
    using System.Threading.Tasks;

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UObject = UnityEngine.Object;

    using GameBoxSdk.Runtime.Utils;

    public class ContentLoader : BaseSystem
    {
        public void LoadScene(string scenePath, LoadSceneMode loadSceneMode, Action OnSceneLoaded, bool setAsMainScene = true)
        {
            AsyncOperation loadSceneAsyncOperation = SceneManager.LoadSceneAsync(scenePath, loadSceneMode);

            loadSceneAsyncOperation.completed += (asyncOperation) =>
            {
                if(setAsMainScene)
                {
                    Scene activeScene = SceneManager.GetSceneByName(scenePath);
                    SceneManager.SetActiveScene(activeScene);
                    DynamicGI.UpdateEnvironment();
                }   
                
                OnSceneLoaded?.Invoke();
            };
        }

        public void UnloadScene(string scenePath, Action OnSceneUnloaded)
        {
            AsyncOperation unloadSceneAsyncOperation = SceneManager.UnloadSceneAsync(scenePath);

            unloadSceneAsyncOperation.completed += (asyncOperation) =>
            {
                OnSceneUnloaded?.Invoke();
            };
        }

        /// <summary>
        /// Loads object asynchronously, good for heavy objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <param name="onAssetLoaded"></param>
        /// <param name="onFailedToLoadAsset"></param>
        public void LoadAssetAsynchronously<T>(string address, Action<T> onAssetLoaded, Action onFailedToLoadAsset) where T : UnityEngine.Object
        {
            ResourceRequest resourceRequest = Resources.LoadAsync(address);
            resourceRequest.completed += (asyncOperation) =>
            {
                if(resourceRequest.asset == null)
                {
                    LoggerUtil.LogError($"{GetType().Name}: Failed to load asset with address {address}");
                    onFailedToLoadAsset?.Invoke();
                }
                else
                {
                    T assetLoaded = resourceRequest.asset as T;

                    if(assetLoaded == null)
                    {
                        assetLoaded = (resourceRequest.asset as GameObject).GetComponent<T>();
                    }

                    onAssetLoaded(assetLoaded);
                }
            };
        }

        /// <summary>
        /// Loads assets asynchronously, good for light objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <returns></returns>
        public T LoadAssetSynchronously<T>(string address) where T : UObject
        {
            T result = Resources.Load<T>(address);

            if (result == null)
            {
                LoggerUtil.LogError($"{GetType().Name}: Failed to load asset with address {address}");
            }

            return result; 
        }

        public async Task<T> LoadAsset<T>(string address) where T : UnityEngine.Object
        {
            bool isLoadingAsset = true;
            T assetReference = null;
            LoadAssetAsynchronously<T>
                (address,
                (assetLoaded) =>
                {
                    assetReference = assetLoaded;
                    isLoadingAsset = false;
                },
                () => isLoadingAsset = false);

            while (isLoadingAsset)
            {
                await Task.Yield();
            }

            return assetReference;
        }

        public void UnloadAsset(UObject asset)
        {
            Resources.UnloadAsset(asset);
        }
    }
}

