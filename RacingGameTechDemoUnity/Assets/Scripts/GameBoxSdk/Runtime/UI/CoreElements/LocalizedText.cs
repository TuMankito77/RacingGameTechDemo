namespace GameBoxSdk.Runtime.UI.CoreElements
{
    using System;
    
    using UnityEngine;
    using UnityEngine.UI;   

    using GameBoxSdk.Runtime.Localization;
    using GameBoxSdk.Runtime.Events;
    using GameBoxSdk.Runtime.Utils;

    public class LocalizedText : MonoBehaviour, IListener
    {
        [SerializeField]
        private Text textComponent = null;

        [SerializeField]
        private string localizationKey = string.Empty;

        private Func<string, string> getLocalizaedText = null;

        #region Unity Methods

        private void OnEnable()
        {
            EventDispatcher.Instance.AddListener(this, typeof(LocalizationEvents));
        }

        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener(this, typeof(LocalizationEvents));
        }

        #endregion

        #region IListener

        public void HandleEvent(IComparable eventName, object data)
        {
            switch(eventName)
            {
                case LocalizationEvents localizationEvent:
                    {
                        HandleLocalizationEvents(localizationEvent, data);
                        break;
                    }

                default:
                    {
                        LoggerUtil.LogError($"{GetType()} - The event {eventName} is not handled by this class. You may need to unsubscribe.");
                        break;
                    }
            }
        }

        #endregion

        public void Initialize(in Func<string, string> soruceGetLocalizedText)
        {
            getLocalizaedText = soruceGetLocalizedText;
            textComponent.text = getLocalizaedText?.Invoke(localizationKey);
        }

        private void HandleLocalizationEvents(LocalizationEvents localizationEvent, object data)
        {
            switch(localizationEvent)
            {
                case LocalizationEvents.OnLanguageUpdated:
                    {
                        textComponent.text = getLocalizaedText?.Invoke(localizationKey);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }
}
