namespace GameBoxSdk.Runtime.Core
{
    using UnityEngine;
    using UnityEngine.Rendering.Universal;
    
    using GameBoxSdk.Runtime.Utils;
    
    public class CameraStackingManager : BaseSystem
    {
        private Camera mainCamera = null;
        private UniversalAdditionalCameraData mainCameraData = null;

        public CameraStackingManager()
        {
            GameObject cameraStackingControllerGO = new GameObject(GetType().Name);
            GameObject.DontDestroyOnLoad(cameraStackingControllerGO);
            mainCamera = cameraStackingControllerGO.AddComponent<Camera>();
            mainCameraData = mainCamera.GetUniversalAdditionalCameraData();
            mainCamera.clearFlags = CameraClearFlags.Color;
            mainCamera.backgroundColor = Color.black;
            //NOTE: This is set to 0 since this camera should not render anything and it should only be used for stacking different cameras.
            mainCamera.cullingMask = 0;
        }

        /// <summary>
        /// The last camera in the list will be at the top.
        /// </summary>
        /// <param name="cameras"></param>
        public void AddCameraToStackAtTop(params Camera[] cameras)
        {
            foreach(Camera camera in cameras)
            {
                if(!IsCameraValid(camera))
                {
                    continue;
                }

                mainCameraData.cameraStack.Add(camera);
            }

        }

        /// <summary>
        /// The last camera in the list will be at the bottom.
        /// </summary>
        /// <param name="cameras"></param>
        public void AddCameraToStackAtBottom(params Camera[] cameras)
        {
            foreach(Camera camera in cameras)
            {
                if (!IsCameraValid(camera))
                {
                    continue;
                }

                mainCameraData.cameraStack.Insert(0, camera);
            }

        }
        
        public void RemoveCameraFromStack(params Camera[] cameras)
        {
            foreach(Camera camera in cameras)
            {
                mainCameraData.cameraStack.Remove(camera);
            }
        }

        private bool IsCameraValid(Camera camera)
        {
            if(mainCameraData.cameraStack.Contains(camera))
            {
                LoggerUtil.LogError($"{GetType().Name} - The camera {camera.name} is trying to be added to the stack more than once.");
                return false;
            }

            UniversalAdditionalCameraData cameraData = camera.GetUniversalAdditionalCameraData();
            
            if(cameraData.renderType != CameraRenderType.Overlay)
            {
                LoggerUtil.LogError($"{GetType().Name} - The camera {camera.name} cannot be added to the stack since its render type is not set to overlay.");
                return false;
            }

            return true;
        }
    }
}
