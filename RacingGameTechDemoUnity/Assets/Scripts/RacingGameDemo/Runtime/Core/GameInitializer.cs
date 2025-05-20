namespace RacingGameDemo.Runtime.Core
{
    using UnityEngine;

    public class GameInitializer : MonoBehaviour
    {
        #region Unity Methods

        private void Awake()
        {
            GameManager gameManager = new GameManager();
        }

        #endregion
    }
}

