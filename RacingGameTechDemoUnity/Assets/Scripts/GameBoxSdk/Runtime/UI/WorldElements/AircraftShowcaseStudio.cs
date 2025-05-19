namespace GameBoxSdk.Runtime.UI.WorldElements
{
    using UnityEngine;

    public class AircraftShowcaseStudio : MonoBehaviour
    {
        [SerializeField]
        private Transform aircraftPlacement = null;

        private GameObject aircraftDisplayed = null;

        public void UpdateAircraftDisplayed(GameObject aircraftPrefab)
        {
            if(aircraftDisplayed != null)
            {
                Destroy(aircraftDisplayed);
            }

            aircraftDisplayed = Instantiate(aircraftPrefab, aircraftPlacement);
        }
    }
}
