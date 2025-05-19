namespace GameBoxSdk.Runtime.UI.CoreElements
{
    using System.Collections.Generic;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;

    public class EventTriggerController
    {
        private struct EntryListenersCounter
        {
            public EventTrigger.Entry entry;
            public int numberOfCallbacks;

            public EntryListenersCounter(EventTrigger.Entry entry, EventTriggerType eventTriggerType, int numberOfCallbacks)
            {
                this.entry = entry;
                this.entry.eventID = eventTriggerType;
                this.numberOfCallbacks = numberOfCallbacks;
            }
        }

        private EventTrigger eventTrigger = null;
        private Dictionary<EventTriggerType, EntryListenersCounter> entryListernersCounterByType = null;

        public EventTriggerController(EventTrigger eventTrigger)
        {
            this.eventTrigger = eventTrigger;
            entryListernersCounterByType = new Dictionary<EventTriggerType, EntryListenersCounter>();
        }

        public void SubscribeToTiggerEvent(EventTriggerType eventTriggerType, UnityAction<BaseEventData> onEventTriggered)
        {
            if (entryListernersCounterByType.TryGetValue(eventTriggerType, out EntryListenersCounter entryListenersCounter))
            {
                entryListenersCounter.entry.callback.AddListener(onEventTriggered);
                entryListenersCounter.numberOfCallbacks++;
                return;
            }

            EntryListenersCounter newEntryListenersCounter = new EntryListenersCounter(new EventTrigger.Entry(), eventTriggerType, 0);
            newEntryListenersCounter.entry.callback.AddListener(onEventTriggered);
            eventTrigger.triggers.Add(newEntryListenersCounter.entry);
            entryListernersCounterByType.Add(eventTriggerType, newEntryListenersCounter);
        }

        public void UnsubscribeToTriggerEvent(EventTriggerType eventTriggerType, UnityAction<BaseEventData> onEventTriggered)
        {
            if (!entryListernersCounterByType.TryGetValue(eventTriggerType, out EntryListenersCounter entryListenersCounter))
            {
                return;
            }

            entryListenersCounter.entry.callback.RemoveListener(onEventTriggered);
            entryListenersCounter.numberOfCallbacks--;

            if (!(entryListenersCounter.numberOfCallbacks == 0))
            {
                return;
            }

            eventTrigger.triggers.Remove(entryListenersCounter.entry);
            entryListernersCounterByType.Remove(eventTriggerType);
        }
    }
}
