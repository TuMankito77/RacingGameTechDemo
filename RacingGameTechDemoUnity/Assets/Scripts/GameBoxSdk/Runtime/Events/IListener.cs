namespace GameBoxSdk.Runtime.Events
{
    using System;
    
    public interface IListener
    {
        public void HandleEvent(IComparable eventName, object data);
    }
}