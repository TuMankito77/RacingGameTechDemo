namespace GameBoxSdk.Runtime.Input
{
    using System.Collections.Generic;

    using UnityEngine;
    
    public class PlayerInput : MonoBehaviour
    {
        private InputActions inputActions = null;
        private List<InputController> activeInputControllers = null;

        #region Unity Methods

        private void Update()
        {
            foreach(InputController inputController in activeInputControllers)
            {
                inputController.Update();
            }
        }

        #endregion

        public void Initialize()
        {
            inputActions = new InputActions();
            activeInputControllers = new List<InputController>();
        }

        public void AddActiveInputController(InputController inputController, IInputControlableEntity entityToControl)
        {
            inputActions = inputActions ?? new InputActions();
            inputController.Enable(inputActions, entityToControl);
            activeInputControllers.Add(inputController);
        }

        public void RemoveActiveInputController(InputController inputController)
        {
            activeInputControllers.Remove(inputController);
            inputController.Disable();
        }
    }
}

