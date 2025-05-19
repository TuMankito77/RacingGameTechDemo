namespace GameBoxSdk.Runtime.Input
{
    using System;

    public abstract class InputController
    {
        public abstract Type EntityToControlType { get; }

        protected InputActions inputActions = null;
        protected IInputControlableEntity entityToControl = null;

        public virtual void Update()
        {

        }

        public virtual void Enable(InputActions sourceInputActions, IInputControlableEntity sourceEntityToControl)
        {
            inputActions = sourceInputActions;
            entityToControl = sourceEntityToControl;
        }

        public virtual void Disable()
        {

        }

        public virtual void Dispose()
        {

        }
    }
}