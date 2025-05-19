namespace GameBoxSdk.Runtime.Events
{
    using System;
    using System.Collections.Generic;

    using GameBoxSdk.Runtime.Utils;
    
    public class EventDispatcher
    {
        private struct EventQueuedInfo
        {
            public IComparable eventName;
            public object data;
        }

        #region Singleton

        private static EventDispatcher instance = null;
        
        public static EventDispatcher Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new EventDispatcher();
                }

                return instance;
            }
        }

        #endregion

        private bool isDispatchingEvent = false;
        private Dictionary<Type, List<IListener>> listenersPerType = null;
        private Queue<EventQueuedInfo> eventsQueued = null;

        public EventDispatcher()
        {
            listenersPerType = new Dictionary<Type, List<IListener>>();
            isDispatchingEvent = false;
            eventsQueued = new Queue<EventQueuedInfo>();
        }

        public void AddListener(IListener listener, params Type[] eventTypes)
        {
            foreach(Type eventType in eventTypes)
            {
                List<IListener> listeners = null;

                if(listenersPerType.TryGetValue(eventType, out listeners) && !listeners.Contains(listener))
                {
                    listeners.Add(listener);
                    continue;
                }

                listeners = new List<IListener>()
                {
                    listener
                };

                listenersPerType.Add(eventType, listeners);
            }
        }

        public void RemoveListener(IListener listener, params Type[] eventTypes)
        {
            foreach (Type eventType in eventTypes)
            {
                List<IListener> listeners = null;

                if (listenersPerType.TryGetValue(eventType, out listeners) && listeners.Contains(listener))
                {
                    listeners.Remove(listener);
                    continue;
                }
            }
        }

        public void Dispatch(IComparable eventName, object data = null)
        {
            Type eventType = eventName.GetType();

            if(!listenersPerType.TryGetValue(eventType, out List<IListener> listeners) || listeners.Count <= 0)
            {
                return;
            }

            if(isDispatchingEvent)
            {
                EventQueuedInfo eventQueuedInfo = new EventQueuedInfo()
                {
                    eventName = eventName,
                    data = data
                };

                eventsQueued.Enqueue(eventQueuedInfo);
                return;
            }

            isDispatchingEvent = true;

            for(int i = 0; i < listeners.Count; i++)
            {
                //To-do: Send the data by allowing the senders to send an event request that will contain data
                try
                {
                    listeners[i].HandleEvent(eventName, data);
                }
                catch(Exception exception)
                {
                    LoggerUtil.LogError(exception);
                }
            }

            isDispatchingEvent = false;

            if(eventsQueued.Count > 0)
            {
                EventQueuedInfo eventQueuedInfo = eventsQueued.Dequeue();
                Dispatch(eventQueuedInfo.eventName, eventQueuedInfo.data); 
            }
        }
    }
}