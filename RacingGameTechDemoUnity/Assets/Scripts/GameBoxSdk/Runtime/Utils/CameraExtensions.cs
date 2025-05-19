namespace GameBoxSdk.Runtime.Utils
{
    using UnityEngine;
    
    public static class CameraExtensions
    {
        public static CameraBoundaries GetScreenBoundariesInWorld(this Camera camera, Vector3 worldPosition)
        {
            CameraBoundaries boundaries = new CameraBoundaries();
            float screenHeight = camera.pixelHeight;
            float screenWidth = camera.pixelWidth;
            Vector3 worldPositionToCameraVector = worldPosition - camera.transform.position;

            float depthDistanceToCamera = worldPositionToCameraVector.magnitude;
            boundaries.top = camera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, depthDistanceToCamera)).y;
            boundaries.bottom = camera.ScreenToWorldPoint(new Vector3(0, 0, depthDistanceToCamera)).y;
            boundaries.left = camera.ScreenToWorldPoint(new Vector3(0, 0, depthDistanceToCamera)).x;
            boundaries.right = camera.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, depthDistanceToCamera)).x;
            boundaries.center = camera.ScreenToWorldPoint(new Vector3(screenWidth / 2, screenHeight / 2, depthDistanceToCamera));

            return boundaries;
        }
    }
}