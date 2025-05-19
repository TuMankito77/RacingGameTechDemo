namespace GameBoxSdk.Runtime.UI.CoreElements
{
    using System;
    
    public interface ISeletableElementAnimator
    {
        public event Action OnSubmitAnimationStart;
        public event Action OnSubmitAnimationEnd;
    }
}
