namespace RacingGameDemo.Runtime.UI.Views
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    using GameBoxSdk.Runtime.UI.CoreElements;
    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.Sound;
    using GameBoxSdk.Runtime.UI.Views.DataContainers;
    
    using RacingGameDemo.Runtime.Gameplay.Track;
    using RacingGameDemo.Runtime.UI.Views.Data;
    using GameBoxSdk.Runtime.Events;
    using GameBoxSdk.Runtime.Utils;

    public class TrackSelectionView : BaseView
    {
        struct TrackIdButtonPair
        {
            public string id;
            public BaseButton button;
        }

        [SerializeField]
        private BaseButton trackOptionButtonPrefab = null;

        [SerializeField]
        private Transform trackButtonsContainer = null;

        [SerializeField]
        private BaseButton selectTrackButton = null;

        [SerializeField]
        private Image trackPreviewImage = null;

        private TracksDatabase tracksDatabase = null;
        private List<TrackIdButtonPair> trackIdButtonPairs = null;
        private string trackSelected = string.Empty;

        public override void Initialize(Camera uiCamera, Action<ClipIds> playClipOnce, ViewInjectableData viewInjectableData, Func<string, string> getLocalizedText, EventSystem sourceEventSystem)
        {
            base.Initialize(uiCamera, playClipOnce, viewInjectableData, getLocalizedText, sourceEventSystem);

            trackIdButtonPairs = new List<TrackIdButtonPair>();
            TrackSelectionViewData trackSelectionViewData = viewInjectableData as TrackSelectionViewData;

            if(trackSelectionViewData == null)
            {
                DisplayMissingInjectableViewDataError();
                return;
            }

            tracksDatabase = trackSelectionViewData.TracksDatabase;

            foreach(string trackId in tracksDatabase.Ids)
            {
                TrackDetails trackDetails = tracksDatabase.GetFile(trackId);
                BaseButton trackButton = Instantiate(trackOptionButtonPrefab, trackButtonsContainer);

                trackIdButtonPairs.Add(new TrackIdButtonPair()
                {
                    id = trackId,
                    button = trackButton
                });

                string localizedName = getLocalizedText(trackDetails.DisplayNameLocKey);
                trackButton.UpdateButtonText(localizedName);

                void OnTrackButtonPressed()
                {
                    EventDispatcher.Instance.Dispatch(UiEvents.OnTrackButtonPressed, trackId);
                    trackSelected = trackId;
                    UpdateTrackButtonSelected();
                }

                trackButton.onButtonPressed += OnTrackButtonPressed;
                trackButton.SetInteractable(trackId != trackSelectionViewData.LastTrackIdSelected);

            }
            
            if(trackSelectionViewData.LastTrackIdSelected == null)
            {
                EventDispatcher.Instance.Dispatch(UiEvents.OnTrackButtonPressed, tracksDatabase.Ids[0]);
                trackIdButtonPairs[0].button.SetInteractable(false);
                trackSelected = tracksDatabase.Ids[0];
            }
            else
            {
                EventDispatcher.Instance.Dispatch(UiEvents.OnTrackButtonPressed, trackSelected);
                trackSelected = trackSelectionViewData.LastTrackIdSelected;
            }
        }

        public override void TransitionIn(int sourceInteractableGroupId)
        {
            base.TransitionIn(sourceInteractableGroupId);
            selectTrackButton.onButtonPressed += OnSelectTrackButtonPressed;
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
            selectTrackButton.onButtonPressed -= OnSelectTrackButtonPressed;
        }
        
        private void OnSelectTrackButtonPressed()
        {
            LoggerUtil.Log("The race should start once this button has been pressed.");
        }

        private void UpdateTrackButtonSelected()
        {
            foreach(TrackIdButtonPair trackIdButtonPair in trackIdButtonPairs)
            {
                if(trackSelected == trackIdButtonPair.id)
                {
                    trackIdButtonPair.button.SetInteractable(false);
                    trackPreviewImage.sprite = tracksDatabase.GetFile(trackIdButtonPair.id).TrackPreview;
                }
                else
                {
                    trackIdButtonPair.button.SetInteractable(true);
                }
            }
        }
    }
}

