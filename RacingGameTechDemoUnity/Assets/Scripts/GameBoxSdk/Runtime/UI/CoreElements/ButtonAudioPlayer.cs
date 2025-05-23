namespace GameBoxSdk.Runtime.UI.CoreElements
{
    using UnityEngine;
    
    using GameBoxSdk.Runtime.Sound;

    public class ButtonAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        private BaseButton baseButton = null;

        //To-do: Make this value be part of a drop-down that will allow us to choose the value
        [SerializeField]
        private string clipId = string.Empty;

        private AudioManager audioManager = null;

        #region Unity methods

        private void Awake()
        {
            baseButton.onSubmit += OnButtonSubmit;
        }

        private void OnDestroy()
        {
            baseButton.onSubmit -= OnButtonSubmit;
        }

        #endregion

        public void Initialize(AudioManager sourceAudioManager)
        {
            audioManager = sourceAudioManager;
        }

        private void OnButtonSubmit()
        {
            audioManager.PlayGeneralClip(clipId);
        }
    }
}
