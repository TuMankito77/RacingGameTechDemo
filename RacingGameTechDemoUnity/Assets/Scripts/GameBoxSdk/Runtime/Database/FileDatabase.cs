namespace GameBoxSdk.Runtime.Database
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Utils;

    public abstract class FileDatabase<T> : ScriptableObject where T : class
    {
        /// <summary>
        /// This can only be used to retrieve the path from a custom inspector script since the class that derive from this class need to overwrite this value.
        /// </summary>
        public abstract string FileDatabasePathScriptableObjectPath { get; }

        protected abstract string TemplateIdsContainerScriptPath { get; }
        protected abstract string IdsContainerClassScriptPath { get; }
        protected abstract string TemplateIdVariableSlot { get; }
        protected abstract string IdScriptLineStart { get; }
        protected abstract string IdScriptLineEnd { get; }

        [SerializeField]
        private IdFilePair<T>[] idFilePairs = null;

        private Dictionary<string, T> idFileLookup = null;

        //NOTE: This will be used later on to create custom editor scripts that will display a drop-down many allowing to select the key for the clip that will be played.
        public List<string> Ids
        {
            get
            {
                if(idFileLookup == null)
                {
                    return new List<string>();
                }

                return new List<string>(idFileLookup.Keys);
            }
        }

        public void Initialize()
        {
            LoggerUtil.Assert(AreIdsUnique(), $"{GetType().Name} - There are duplicate {typeof(T)} ids, please make sure to not repeat any id on the list.");
            idFileLookup = GetIdFileLookup(idFilePairs);
        }

        public T GetFile(string id)
        {
            if(idFileLookup == null)
            {
                Initialize();
            }

            if (idFileLookup.TryGetValue(id, out T file))
            {
                return file;
            }

            LoggerUtil.LogError($"{GetType().Name} - The {id} id does not exist!");
            return null;
        }

        public bool DoesIdExist(string id)
        {
            return idFileLookup.ContainsKey(id);
        }

        private bool AreIdsUnique()
        {
            if (idFilePairs == null)
            {
                return true;
            }

            for(int i = 0; i < idFilePairs.Length - 1; i++)
            {
                for(int j = i + 1; j < idFilePairs.Length; j++)
                {
                    if (idFilePairs[i].Id.Equals(idFilePairs[j].Id))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private Dictionary<string, T> GetIdFileLookup(IdFilePair<T>[] idFilePairs)
        {
            Regex nonAlphanumeric = new Regex(@"\W|_");

            Dictionary<string, T> idFileLookup = new Dictionary<string, T>(idFilePairs.Length);

            foreach(IdFilePair<T> idFilePair in idFilePairs)
            {
                string idSimplified = nonAlphanumeric.Replace(idFilePair.Id, string.Empty);
                idFileLookup.Add(idSimplified, idFilePair.File);
            }

            return idFileLookup;
        }

#if UNITY_EDITOR

        #region Unity Methods

        private void OnValidate()
        {
            LoggerUtil.Assert(AreIdsUnique(), $"{GetType().Name} - There are duplicate {typeof(T)} ids, please make sure to not repeat any id on the list, otherwise the clips will not be stored in the lookup at runtime.");
        }

        #endregion

        public void GenerateEnumIdsContainerClassFile()
        {
            TextAsset scriptTemplate = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(TemplateIdsContainerScriptPath);
            string templateText = scriptTemplate.text;

            int contextMenuStartIndex = templateText.IndexOf(IdScriptLineStart, StringComparison.InvariantCulture);
            int contextMenuEndIndex = templateText.IndexOf(IdScriptLineEnd, contextMenuStartIndex, StringComparison.InvariantCulture) + IdScriptLineEnd.Length;

            string customDataConstantTemplate = templateText.Substring(contextMenuStartIndex, contextMenuEndIndex - contextMenuStartIndex);
            StringBuilder customDataConstants = new StringBuilder();

            LoggerUtil.Assert(AreIdsUnique(), $"{GetType().Name} - There are duplicate {typeof(T)} ids, please make sure to not repeat any id on the list.");
            idFileLookup = GetIdFileLookup(idFilePairs);
            int currentIndex = 0;

            foreach (string uniqueFileId in idFileLookup.Keys)
            {
                string fileIdVariableDeclaration = $"{uniqueFileId}";
                string keyConstant = customDataConstantTemplate.Replace(TemplateIdVariableSlot, fileIdVariableDeclaration);
                customDataConstants.Append(keyConstant);
                customDataConstants.Append(currentIndex == idFileLookup.Count - 1 ? string.Empty : "\n\t\t");
                ++currentIndex;
            }

            string contextMenuScript = templateText.Replace(customDataConstantTemplate, customDataConstants.ToString());

            if (File.Exists(IdsContainerClassScriptPath))
            {
                File.SetAttributes(IdsContainerClassScriptPath, FileAttributes.Normal);
            }

            File.WriteAllText(IdsContainerClassScriptPath, contextMenuScript);
            UnityEditor.AssetDatabase.Refresh();
        }

#endif
    }
}
