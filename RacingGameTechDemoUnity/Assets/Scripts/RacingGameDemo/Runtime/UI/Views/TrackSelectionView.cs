namespace RacingGameDemo.Runtime.UI.Views
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.EventSystems;

    using GameBoxSdk.Runtime.UI.CoreElements;
    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.Sound;
    using GameBoxSdk.Runtime.UI.Views.DataContainers;

    public class TrackSelectionView : BaseView
    {
        [SerializeField]
        private Transform trackButtonsContainer = null;

        private List<BaseButton> trackButtons = null;

        public override void Initialize(Camera uiCamera, Action<ClipIds> playClipOnce, ViewInjectableData viewInjectableData, Func<string, string> getLocalizedText, EventSystem sourceEventSystem)
        {
            base.Initialize(uiCamera, playClipOnce, viewInjectableData, getLocalizedText, sourceEventSystem);
        }

        public override void TransitionIn(int sourceInteractableGroupId)
        {
            base.TransitionIn(sourceInteractableGroupId);
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
        }
    }
}

