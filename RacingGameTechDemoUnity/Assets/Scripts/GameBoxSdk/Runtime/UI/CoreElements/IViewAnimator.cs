namespace GameBoxSdk.Runtime.UI.CoreElements
{
    using System;
    
    public interface IViewAnimator
    {
        public event Action OnTransitionInAnimationCompleted;
        public event Action OnTransitionOutAnimatonCompleted;
        public void PlayTransitionIn();
        public void PlayTransitionOut();
    }
}
