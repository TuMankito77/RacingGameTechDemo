namespace RacingGameDemo.Runtime.UI.Views
{
    using UnityEngine;
    
    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.UI.CoreElements;

    public class OptionsMenuView : BaseView
    {
        [SerializeField]
        private BaseSlider masterVolumeSlider = null;

        [SerializeField]
        private BaseSlider musicVolumeSlider = null;

        [SerializeField]
        private BaseSlider soundEffectsVolumeSlider = null;
    }
}

