namespace GameBoxSdk.Runtime.SaveTool
{
    using Newtonsoft.Json;
    
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Utils;

    public class StorageAccessor
    {
        public void Save<T>(T classInformation) where T : IStorable
        {
            string classInformationJson = JsonConvert.SerializeObject(classInformation);
            PlayerPrefs.SetString(classInformation.Key, classInformationJson);
        }

        public T Load<T>(string key) where T : IStorable
        {
            LoggerUtil.Assert(PlayerPrefs.HasKey(key), $"{GetType().Name} - The information with the key {key} that you are trying to load does not exist.");
            string classInformationJson = PlayerPrefs.GetString(key);
            T classInformation = JsonConvert.DeserializeObject<T>(classInformationJson);
            return classInformation;
        }

        public bool DoesInformationExist(string key)
        {
            return PlayerPrefs.HasKey(key);
        }
    }
}

