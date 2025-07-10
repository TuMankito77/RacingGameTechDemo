namespace RacingGameDemo.Runtime.UI.Views
{
    using System;
    
    using UnityEngine;
    using UnityEngine.EventSystems;
    
    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.UI.CoreElements;
    using GameBoxSdk.Runtime.Events;
    using GameBoxSdk.Runtime.UI;
    using GameBoxSdk.Runtime.Sound;
    using GameBoxSdk.Runtime.UI.Views.DataContainers;
    using RacingGameDemo.Runtime.UI.Views.Data;

    public class OptionsMenuView : BaseView
    {
        [SerializeField]
        private BaseSlider masterVolumeSlider = null;

        [SerializeField]
        private BaseSlider musicVolumeSlider = null;

        [SerializeField]
        private BaseSlider soundEffectsVolumeSlider = null;

        public override void Initialize(UiManager sourceUiManager, Camera uiCamera, Action<ClipIds> playClipOnce, ViewInjectableData viewInjectableData, Func<string, string> getLocalizedText, EventSystem sourceEventSystem)
        {
            base.Initialize(sourceUiManager, uiCamera, playClipOnce, viewInjectableData, getLocalizedText, sourceEventSystem);

            OptionsMenuViewData optionsMenuViewData = viewInjectableData as OptionsMenuViewData;

            if(optionsMenuViewData != null)
            {
                masterVolumeSlider.UpdateSliderValue(optionsMenuViewData.MasterVolumeSaved);
                musicVolumeSlider.UpdateSliderValue(optionsMenuViewData.MusicVolumeSaved);
                soundEffectsVolumeSlider.UpdateSliderValue(optionsMenuViewData.SoundEffectsVolumeSaved);
            }
        }

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

