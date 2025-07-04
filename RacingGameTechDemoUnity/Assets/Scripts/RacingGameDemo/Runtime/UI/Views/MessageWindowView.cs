namespace GameBoxSdk.Runtime.UI.Views
{
    using System;
    
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    
    using GameBoxSdk.Runtime.Sound;
    using GameBoxSdk.Runtime.UI.CoreElements;
    using GameBoxSdk.Runtime.UI.Views.DataContainers;
    
    using RacingGameDemo.Runtime.UI.Views.Data;

    public class MessageWindowView : BaseView
    {
        [SerializeField]
        private Text messageText = null;

        [SerializeField]
        private BaseButton confirmButton = null;

        [SerializeField]
        private BaseButton cancelButton = null;

        private MessageWindowViewData messageWindowViewData = null;

        public BaseButton ConfirmButton => confirmButton;
        public BaseButton CancelButton => cancelButton;

        public override void Initialize(UiManager sourceUiManager, Camera uiCamera, Action<ClipIds> playClipOnce, ViewInjectableData viewInjectableData, Func<string, string> getLocalizedText, EventSystem sourceEventSystem)
        {
            base.Initialize(sourceUiManager, uiCamera, playClipOnce, viewInjectableData, getLocalizedText, sourceEventSystem);
            messageWindowViewData = viewInjectableData as MessageWindowViewData;
            
            if (messageWindowViewData != null)
            {
                messageText.text = messageWindowViewData.Message;
                cancelButton.gameObject.SetActive(messageWindowViewData.DisplayCancelButton);
            }
        }
    }
}
