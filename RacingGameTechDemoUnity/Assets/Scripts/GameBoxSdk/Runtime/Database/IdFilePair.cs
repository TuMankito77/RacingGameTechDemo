namespace GameBoxSdk.Runtime.Database
{
    using System;

    using UnityEngine;

    [Serializable]
    public class IdFilePair<T> where T : class
    {
        [SerializeField]
        private string id = string.Empty;

        [SerializeField]
        private T file = null;

        public string Id => id;
        public T File => file;
    }
}
