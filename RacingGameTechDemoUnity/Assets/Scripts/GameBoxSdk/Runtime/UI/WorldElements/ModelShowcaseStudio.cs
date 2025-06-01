namespace GameBoxSdk.Runtime.UI.WorldElements
{
    using UnityEngine;

    public class ModelShowcaseStudio : MonoBehaviour
    {
        [SerializeField]
        private Transform ModelPlacement = null;

        private GameObject ModelDisplayed = null;

        public void UpdateModelDisplayed(GameObject aircraftPrefab)
        {
            if(ModelDisplayed != null)
            {
                Destroy(ModelDisplayed);
            }

            ModelDisplayed = Instantiate(aircraftPrefab, ModelPlacement);
        }
    }
}
