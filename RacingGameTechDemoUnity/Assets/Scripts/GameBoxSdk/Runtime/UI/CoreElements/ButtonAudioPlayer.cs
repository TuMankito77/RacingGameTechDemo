namespace GameBoxSdk.Runtime.UI.CoreElements
{
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Sound;
    using System;

    public class ButtonAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private BaseButton baseButton = null;

        [SerializeField]
        private ClipIds clipId = ClipIds.None;

        private Action<ClipIds> playClipOnce = null;

        #region Unity methods

        private void Awake()
        {
            baseButton.onSubmit += OnButtonSubmit;
        }

        private void OnDestroy()
        {
            baseButton.onSubmit -= OnButtonSubmit;
        }

        #endregion

        public void Initialize(Action<ClipIds> sourcePlayClipOnce)
        {
            playClipOnce = sourcePlayClipOnce;
        }

        private void OnButtonSubmit()
        {
            playClipOnce?.Invoke(clipId);
        }
    }
}
