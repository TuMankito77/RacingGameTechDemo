namespace GameBoxSdk.Runtime.UI.WorldElements
{
    using UnityEngine;

    public class ModelShowcaseStudio : MonoBehaviour
    {
        [SerializeField]
        private Transform modelPlacement = null;

        [SerializeField]
        private Transform cameraCenterParentTransform = null;

        private GameObject modelDisplayed = null;

        public Transform CameraCenterParentTransform => cameraCenterParentTransform;

        public void UpdateModelDisplayed(GameObject modelPrefab)
        {
            if(modelDisplayed != null)
            {
                Destroy(modelDisplayed);
            }

            modelDisplayed = Instantiate(modelPrefab, modelPlacement);
        }
    }
}
