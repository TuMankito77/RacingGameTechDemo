namespace GameBoxSdk.Runtime.Input.TouchControls
{
    using UnityEngine.InputSystem.Layouts;
    using UnityEngine;
    using UnityEngine.InputSystem.OnScreen;
    using GameBoxSdk.Runtime.UI.CoreElements;

    public class InputActionFromOnScreenButton : OnScreenControl
    {
        [InputControl(layout = "Button")]
        [SerializeField]
        private string controlPathSelected = string.Empty;

        [SerializeField]
        private BaseButton linkedButton = null;

        protected override string controlPathInternal 
        { 
            get => controlPathSelected; 
            set => controlPathSelected = value; 
        }

        #region Unity Methods

        private void Awake()
        {
            linkedButton.onButtonPressed += OnButtonPressed;
        }

        private void OnDestroy()
        {
            linkedButton.onButtonPressed -= OnButtonPressed;
        }

        #endregion
        
        private void OnButtonPressed()
        {
            SendValueToControl(1.0f);
            SendValueToControl(0.0f);
        }
    }
}
