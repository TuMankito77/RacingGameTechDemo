namespace RacingGameDemo.Runtime.Gameplay.Track
{
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Utils;

    [CreateAssetMenu(fileName = TRACK_DETAILS_ASSET_NAME, menuName = "RacingGameDemo/TrackDetails")]
    public class TrackDetails : ScriptableObject
    {
        private const string TRACK_DETAILS_ASSET_NAME = "TrackDetails";

        [SerializeField]
        private string displayName = string.Empty;

        [SerializeField]
        private string displayNameLocKey = string.Empty;

        [SerializeField]
        private SceneField trackScene = null;

        public string DisplayName => displayName;
        public string DisplayNameLocKey => displayNameLocKey;
        public SceneField TrackScene => trackScene;
    }
}