namespace RacingGameDemo.Runtime.UI.Views
{
    using System.Collections.Generic;

    using UnityEngine;

    using GameBoxSdk.Runtime.UI.CoreElements;
    using GameBoxSdk.Runtime.UI.Views;

    public class TrackSelectionView : BaseView
    {
        [SerializeField]
        private Transform trackButtonsContainer = null;

        private List<BaseButton> trackButtons = null;
    }
}

