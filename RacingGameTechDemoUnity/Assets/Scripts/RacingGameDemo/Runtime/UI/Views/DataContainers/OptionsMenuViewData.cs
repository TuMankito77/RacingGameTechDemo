namespace RacingGameDemo.Runtime.UI.Views.Data
{
    using GameBoxSdk.Runtime.UI.Views.DataContainers;

    public class OptionsMenuViewData : ViewInjectableData
    {
        public float MasterVolumeSaved { get; private set; }
        public float MusicVolumeSaved { get; private set; }
        public float SoundEffectsVolumeSaved { get; private set; }

        public OptionsMenuViewData(float masterVolumeSaved, float musicVolumeSaved, float soundEffectsVolumeSaved)
        {
            MasterVolumeSaved = masterVolumeSaved;
            MusicVolumeSaved = musicVolumeSaved;
            SoundEffectsVolumeSaved = soundEffectsVolumeSaved;
        }
    }
}

