namespace GameBoxSdk.Runtime.Localization
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    
    using UnityEngine;

    using Newtonsoft.Json;

    using GameBoxSdk.Runtime.Core;
    using GameBoxSdk.Runtime.Events;
    using GameBoxSdk.Runtime.Utils;

    public class LocalizationManager : BaseSystem
    {
        private LocalizationDatabase localizationDatabase = null;
        private Dictionary<string, string> localizationKeyLocalizedTextPair = null;

        public SystemLanguage LanguageSelected { get; private set; } = SystemLanguage.Unknown;

        public override async Task<bool> Initialize(IEnumerable<BaseSystem> sourceDependencies)
        {
            await base.Initialize(sourceDependencies);
            ContentLoader contentLoader = GetDependency<ContentLoader>();
            localizationDatabase = await contentLoader.LoadAsset<LocalizationDatabase>(LocalizationDatabase.LOCALIZATION_DATABASE_SCRIPTABLE_OBJECT_PATH);
            
            if(localizationDatabase == null)
            {
                return false;
            }
            
            localizationDatabase.Initialize();
            
            if (localizationDatabase.Ids.Count > 0)
            {
                switch(Application.systemLanguage)
                {
                    case SystemLanguage.English:
                        {
                            LanguageSelected = SystemLanguage.English;
                            break;
                        }

                    case SystemLanguage.French:
                        {
                            LanguageSelected = SystemLanguage.French;
                            break;
                        }

                    case SystemLanguage.Spanish:
                        {
                            LanguageSelected = SystemLanguage.Spanish;
                            break;
                        }

                    default:
                        {
                            LanguageSelected = SystemLanguage.English;
                            break;
                        }
                }

                TextAsset textAsset = localizationDatabase.GetFile(LanguageSelected.ToString());
                localizationKeyLocalizedTextPair = JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
            }

            return true;
        }

        public void UpdateSelectedLanguage(SystemLanguage languageSelected)
        {
            string systemLanguageAsString = languageSelected.ToString();

            if(localizationDatabase.DoesIdExist(systemLanguageAsString))
            {
                LanguageSelected = languageSelected;
                TextAsset textAsset = localizationDatabase.GetFile(systemLanguageAsString);
                localizationKeyLocalizedTextPair = JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
                EventDispatcher.Instance.Dispatch(LocalizationEvents.OnLanguageUpdated);
            }
            else
            {
                LoggerUtil.Log($"{GetType()}: Language key {languageSelected} not found, keeping the current language as the one selected.");
            }
        }

        public string GetLocalizedText(string localizationKey)
        {
            if(localizationKeyLocalizedTextPair != null && localizationKeyLocalizedTextPair.TryGetValue(localizationKey, out string localizedText))
            {
                return localizedText;
            }

            return localizationKey;
        }
    }
}
