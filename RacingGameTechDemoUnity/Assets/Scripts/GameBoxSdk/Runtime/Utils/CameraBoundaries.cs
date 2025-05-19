namespace GameBoxSdk.Runtime.Utils
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct CameraBoundaries
    {
        [SerializeField]
        public float top;

        [SerializeField]
        public float bottom;

        [SerializeField]
        public float left;

        [SerializeField]
        public float right;

        [SerializeField]
        public Vector3 center;

        public void AddOffset(CameraBoundaries offset)
        {
            top += offset.top;
            bottom += offset.bottom;
            right += offset.right;
            left += offset.left;
            center += offset.center;
        }
    }
}
