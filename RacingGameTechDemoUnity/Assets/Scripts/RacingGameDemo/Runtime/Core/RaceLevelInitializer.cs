namespace RacingGameDemo.Runtime.Core
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Core;

    public class RaceLevelInitializer : BaseSystem
    {
        private const string GAMEPLAY_RENDERING_CAMERA_PATH = "RacingGameDemo/RaceLevel/GameplayRenderingCamera";

        private ContentLoader contentLoader = null;
        private CameraStackingManager cameraStackingManager = null;
        private Camera gameplayRenderingCamera = null;

        public RaceLevelInitializer(ContentLoader sourceContentLoader, CameraStackingManager sourceCameraStackingManager)
        {
            contentLoader = sourceContentLoader;
            cameraStackingManager = sourceCameraStackingManager;
        }

        public override async Task<bool> Initialize(IEnumerable<BaseSystem> sourceDependencies)
        {
            if(!await base.Initialize(sourceDependencies))
            {
                return false;
            }

            Camera gameplayRenderingCameraPrefab = await contentLoader.LoadAsset<Camera>(GAMEPLAY_RENDERING_CAMERA_PATH);

            if(gameplayRenderingCameraPrefab == null)
            {
                return false;
            }

            gameplayRenderingCamera = GameObject.Instantiate(gameplayRenderingCameraPrefab);
            cameraStackingManager.AddCameraToStackAtBottom(gameplayRenderingCamera);
            return true;
        }
    }
}