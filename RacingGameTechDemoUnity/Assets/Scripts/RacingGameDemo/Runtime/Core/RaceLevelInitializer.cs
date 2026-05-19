namespace RacingGameDemo.Runtime.Core
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Core;

    using RacingGameDemo.Runtime.Gameplay.Car;

    public class RaceLevelInitializer : BaseSystem
    {
        private const string GAMEPLAY_RENDERING_CAMERA_PATH = "RacingGameDemo/RaceLevel/GameplayRenderingCamera";

        private ContentLoader contentLoader = null;
        private CameraStackingManager cameraStackingManager = null;
        private Camera gameplayRenderingCamera = null;
        private BaseCar gameplayCarPrefab = null;
        private BaseCar gameplayCarInstance = null;

        public BaseCar GameplayCarInstance => gameplayCarInstance;

        public RaceLevelInitializer(ContentLoader sourceContentLoader, CameraStackingManager sourceCameraStackingManager, BaseCar gameplayCarPrefab)
        {
            contentLoader = sourceContentLoader;
            cameraStackingManager = sourceCameraStackingManager;
            this.gameplayCarPrefab = gameplayCarPrefab;
        }

        public override async Task<bool> Initialize(IEnumerable<BaseSystem> sourceDependencies)
        {
            if(!await base.Initialize(sourceDependencies))
            {
                return false;
            }

            GameObject respawnObject = GameObject.FindGameObjectWithTag("Respawn");
            
            if(respawnObject == null)
            {
                return false;
            }

            Vector3 carSpawnPosition = respawnObject.transform.position;
            gameplayCarInstance = GameObject.Instantiate(gameplayCarPrefab, carSpawnPosition, Quaternion.identity);

            Camera gameplayRenderingCameraPrefab = await contentLoader.LoadAsset<Camera>(GAMEPLAY_RENDERING_CAMERA_PATH);

            if(gameplayRenderingCameraPrefab == null)
            {
                return false;
            }

            gameplayRenderingCamera = GameObject.Instantiate(gameplayRenderingCameraPrefab, gameplayCarInstance.CameraSocket);
            cameraStackingManager.AddCameraToStackAtBottom(gameplayRenderingCamera);
            //TO-DO: Place the car at the start of the track.
            //TO-DO: Display the HUD view.
            return true;
        }

        public void Dispose()
        {
            //TO-DO: Remove the HUD view.
            gameplayCarInstance.Dispose();
            cameraStackingManager.RemoveCameraFromStack(gameplayRenderingCamera);
            GameObject.Destroy(gameplayCarInstance.gameObject);
            GameObject.Destroy(gameplayRenderingCamera.gameObject);
            gameplayCarPrefab = null;
            gameplayCarInstance = null;
            gameplayRenderingCamera = null;
            cameraStackingManager = null;
            contentLoader = null;
        }
    }
}