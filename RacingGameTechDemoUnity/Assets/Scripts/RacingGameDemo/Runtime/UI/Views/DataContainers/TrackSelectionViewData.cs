namespace RacingGameDemo.Runtime.UI.Views.Data
{
    using GameBoxSdk.Runtime.UI.Views.DataContainers;
    
    using RacingGameDemo.Runtime.Gameplay.Track;

    public class TrackSelectionViewData : ViewInjectableData
    {
        public string LastTrackIdSelected { get; private set; } = null;
        public TracksDatabase TracksDatabase { get; private set; } = null;

        public TrackSelectionViewData(TracksDatabase tracksDatabase, string lastTrackIdSelected)
        {
            TracksDatabase = tracksDatabase;
            LastTrackIdSelected = lastTrackIdSelected;
        }
    }
}