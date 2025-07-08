namespace RacingGameDemo.Runtime.UI.Views
{
    using UnityEngine;
    
    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.UI.CoreElements;
    using GameBoxSdk.Runtime.Events;

    public class OptionsMenuView : BaseView
    {
        [SerializeField]
        private BaseSlider masterVolumeSlider = null;

        [SerializeField]
        private BaseSlider musicVolumeSlider = null;

        [SerializeField]
        private BaseSlider soundEffectsVolumeSlider = null;

        public override void TransitionIn(int interactableGroupId)
        {
            base.TransitionIn(interactableGroupId);
            masterVolumeSlider.OnValueChanged += OnMasterVolumeChanged;
            musicVolumeSlider.OnValueChanged += OnMusicVolumeChanged;
            soundEffectsVolumeSlider.OnValueChanged += OnSoundEffectsVolumeChanged;
        }

        public override void TransitionOut()
        {
            base.TransitionOut(); 
            masterVolumeSlider.OnValueChanged -= OnMasterVolumeChanged;
            musicVolumeSlider.OnValueChanged -= OnMusicVolumeChanged;
            soundEffectsVolumeSlider.OnValueChanged -= OnSoundEffectsVolumeChanged;
        }

        private void OnMasterVolumeChanged(float volume)
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnMasterVolumeChanged, volume);
        }

        private void OnMusicVolumeChanged(float volume)
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnMusicVolumeChanged, volume);
        }

        private void OnSoundEffectsVolumeChanged(float volume)
        {
            EventDispatcher.Instance.Dispatch(UiEvents.OnSoundEffectsVolumeChanged, volume);
        }
    }
}

