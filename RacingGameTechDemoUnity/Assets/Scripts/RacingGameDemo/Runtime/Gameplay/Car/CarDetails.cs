namespace RacingGameDemo.Runtime.Gameplay.Car
{
    using UnityEngine;

    [CreateAssetMenu(fileName = CAR_DETAILS_ASSET_NAME, menuName = "RacingGameDemo/CarDetails")]
    public class CarDetails : ScriptableObject
    {
        private const string CAR_DETAILS_ASSET_NAME = "CarDetails";

        [SerializeField]
        private string displayName = string.Empty;

        [SerializeField]
        private string displayNameLocKey = string.Empty;

        [SerializeField]
        private GameObject carPrefab = null;

        [SerializeField]
        private float maxSpeed = 300;

        [SerializeField]
        private float maxAcceleration = 100;

        public string DisplayName => displayName;
        public string DisplayNameLocKey => displayNameLocKey;
        public GameObject CarPrefab => carPrefab;
        public float MaxSpeed => maxSpeed;
        public float MaxAcceleration => maxAcceleration;
    }
}

