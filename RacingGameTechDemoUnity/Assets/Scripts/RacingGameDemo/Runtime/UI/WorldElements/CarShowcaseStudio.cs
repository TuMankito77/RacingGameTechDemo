namespace RacingGameDemo.Runtime.UI.WorldElements
{
    using System;
    using System.Collections;
    
    using UnityEngine;

    using GameBoxSdk.Runtime.UI.WorldElements;

    public class CarShowcaseStudio : ModelShowcaseStudio
    {
        [Serializable]
        private struct ShowcaseCameraConfiguration
        {
            [SerializeField]
            public int orthographicSize;

            [SerializeField]
            public Transform transform;
        }

        [SerializeField, Min(1)]
        private float settingsLerpDuration = 1;

        [SerializeField]
        private AnimationCurve transitionAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField]
        private ShowcaseCameraConfiguration stationaryCameraSettings = default(ShowcaseCameraConfiguration);

        [SerializeField]
        private ShowcaseCameraConfiguration movableCameraSettings = default(ShowcaseCameraConfiguration);

        private Coroutine cameraSettingsLerp = null;

        #region Unity Methods

        private void OnEnable()
        {
            RenderCamera.orthographicSize = stationaryCameraSettings.orthographicSize;
            RenderCamera.transform.position = stationaryCameraSettings.transform.position;
            RenderCamera.transform.rotation = stationaryCameraSettings.transform.rotation;
            RenderCamera.transform.localScale = stationaryCameraSettings.transform.localScale;
        }

        private void OnDisable()
        {
            if(cameraSettingsLerp != null)
            {
                StopCoroutine(cameraSettingsLerp);
            }
        }

        #endregion

        public void SwitchToStationaryCameraSettings()
        {
            if (cameraSettingsLerp != null)
            {
                StopCoroutine(cameraSettingsLerp);
            }

            cameraSettingsLerp = StartCoroutine(LerpCameraSettingValues(stationaryCameraSettings));
        }

        public void SwitchToMovableCameraSettings()
        {
            if (cameraSettingsLerp != null)
            {
                StopCoroutine(cameraSettingsLerp);
            }

            cameraSettingsLerp = StartCoroutine(LerpCameraSettingValues(movableCameraSettings));
        }

        private IEnumerator LerpCameraSettingValues(ShowcaseCameraConfiguration showcaseCameraConfiguration)
        {
            float timeTranscurred = 0;
            float startOrthographicSize = RenderCamera.orthographicSize;
            Vector3 startPosition = RenderCamera.transform.position;
            Quaternion startRotation = RenderCamera.transform.rotation;
            Vector3 startScale = RenderCamera.transform.localScale;

            while(timeTranscurred < settingsLerpDuration)
            {
                float animationProgress = timeTranscurred / settingsLerpDuration;
                float lerpValue = transitionAnimationCurve.Evaluate(animationProgress);
                RenderCamera.orthographicSize = Mathf.Lerp(startOrthographicSize, showcaseCameraConfiguration.orthographicSize, lerpValue);
                RenderCamera.transform.position = Vector3.Lerp(startPosition, showcaseCameraConfiguration.transform.position, lerpValue);
                RenderCamera.transform.rotation = Quaternion.Lerp(startRotation, showcaseCameraConfiguration.transform.rotation, lerpValue);
                RenderCamera.transform.localScale = Vector3.Lerp(startScale, showcaseCameraConfiguration.transform.localScale, lerpValue);
                yield return new WaitForEndOfFrame();
                timeTranscurred += Time.deltaTime;
            }

            RenderCamera.orthographicSize = showcaseCameraConfiguration.orthographicSize;
            RenderCamera.transform.position = showcaseCameraConfiguration.transform.position;
            RenderCamera.transform.rotation = showcaseCameraConfiguration.transform.rotation;
            RenderCamera.transform.localScale = showcaseCameraConfiguration.transform.localScale;
        }
    }
}
