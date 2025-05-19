namespace GameBoxSdk.Runtime.Input
{
    using System;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.InputSystem.EnhancedTouch;

    using GameBoxSdk.Runtime.Utils;
    using System.Security.Cryptography;

    public class InputManager
    {
        private PlayerInput playerInput = null;
        private Dictionary<Type, InputController> typeInputControllerPairs = null;

        public InputManager(bool enableTouchControls)
        {
            playerInput = new GameObject("Player Input Reader").AddComponent<PlayerInput>();
            playerInput.Initialize();
            GameObject.DontDestroyOnLoad(playerInput);
            typeInputControllerPairs = new Dictionary<Type, InputController>();

            if(enableTouchControls)
            {
                EnhancedTouchSupport.Enable();
            }
        }

        public void AddInputController(params InputController[] inputControllers)
        {
            foreach(InputController inputController in inputControllers)
            {
                LoggerUtil.Assert(!typeInputControllerPairs.ContainsKey(inputController.GetType()), 
                    $"{GetType().Name}: You are trying to add the {inputController.GetType().Name} input more than once!");
                typeInputControllerPairs.Add(inputController.EntityToControlType, inputController);
            }
        }

        public InputController GetInputController(IInputControlableEntity entityControlled)
        {
            LoggerUtil.Assert(typeInputControllerPairs.TryGetValue(entityControlled.GetType(), out InputController inputControllerFound),
                $"{GetType().Name}: No input controller was found for the {entityControlled.GetType()} entity.");
            return inputControllerFound;
        }

        public void RemoveInputContoller(params InputController[] inputControllers)
        {
            foreach (InputController inputController in inputControllers)
            {
                LoggerUtil.Assert(typeInputControllerPairs.TryGetValue(inputController.EntityToControlType, out InputController inputControllerFound),
                $"{GetType().Name}: No input controller to be removed was found for the {inputController.EntityToControlType} entity.");
                typeInputControllerPairs.Remove(inputController.EntityToControlType);
                playerInput.RemoveActiveInputController(inputControllerFound);
                inputController.Dispose();
            }
        }

        public void EnableInput(IInputControlableEntity entityToControl)
        {
            LoggerUtil.Assert(typeInputControllerPairs.TryGetValue(entityToControl.GetType(), out InputController inputControllerFound), 
                $"{GetType().Name}: No input controller was found for the {entityToControl.GetType().Name} entity.");
            playerInput.AddActiveInputController(inputControllerFound, entityToControl);
        }

        public void DisableInput(IInputControlableEntity entityControlled)
        {
            LoggerUtil.Assert(typeInputControllerPairs.TryGetValue(entityControlled.GetType(), out InputController inputControllerFound),
                $"{GetType().Name}: No input controller was found for the {entityControlled.GetType()} entity.");
            playerInput.RemoveActiveInputController(inputControllerFound);
        }

        public void Dispose()
        {
            GameObject.Destroy(playerInput.gameObject);
            typeInputControllerPairs.Clear();

            if(EnhancedTouchSupport.enabled)
            {
                EnhancedTouchSupport.Disable();
            }
        }
    }
}
