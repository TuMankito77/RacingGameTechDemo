namespace RacingGameDemo.Runtime.SotorableClasses
{   
    using Newtonsoft.Json;
    
    using GameBoxSdk.Runtime.SaveTool;

    public class GameSettings : IStorable
    {
        public string Key => nameof(GameSettings);

        [JsonProperty]
        private float? masterVolume = null;

        [JsonProperty]
        private float? musicVolume = null;

        [JsonProperty]
        private float? soundEffectsVolume = null;

        [JsonIgnore]
        public float MasterVolume { get => masterVolume.Value;  set => masterVolume = value; }

        [JsonIgnore]
        public float MusicVolume { get => musicVolume.Value; set => musicVolume = value; }

        [JsonIgnore]
        public float SoundEffectsVolume { get => soundEffectsVolume.Value; set => soundEffectsVolume = value; }
    }
}
