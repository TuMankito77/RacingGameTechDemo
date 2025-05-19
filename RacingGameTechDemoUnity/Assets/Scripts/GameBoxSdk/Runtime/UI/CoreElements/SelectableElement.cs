namespace GameBoxSdk.Runtime.UI.CoreElements
{
    using System;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public abstract class SelectableElement : MonoBehaviour
    {
        [Serializable]
        private struct SelectableElementNeighborGroup
        {
            public SelectableNeighborDirection neighborDirection;
            public SelectableElement[] selectableNeighbors;
        }

        public event Action onSubmit = null;
        public event Action onSelect = null;
        public event Action onDeselect = null;

        [SerializeField]
        private EventTrigger eventTrigger = null;

        [SerializeField]
        private bool deselectAfterSubmitAction = true;

        [SerializeField]
        private SelectableElementNeighborGroup[] selectableElementNeighborGroups = new SelectableElementNeighborGroup[0];

        protected EventTriggerController eventTriggerController = null;
        
        private bool isSubscribedToInteractableEvents = false;

        public bool DeselectAfterSubmitAction => deselectAfterSubmitAction;
        public bool IsInteractable { get; private set; } = true;

        #region Unity Methods

        protected virtual void Awake()
        {
            CheckNeededComponents();
            eventTriggerController = new EventTriggerController(eventTrigger);
        }

        protected virtual void OnEnable()
        {
            SubscribeToInteractableEvents();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeFromInteractableEvents();
        }

        protected virtual void OnDestroy()
        {
            
        }

        #endregion

        public virtual void SetAnimationExternalModule(ISeletableElementAnimator selectableElementAnimator)
        {
            
        }

        public virtual void SetInteractable(bool isInteractable)
        {
            IsInteractable = isInteractable;

            if (isInteractable && !isSubscribedToInteractableEvents)
            {
                SubscribeToInteractableEvents();
            }
            else if (!isInteractable && isSubscribedToInteractableEvents)
            {
                UnsubscribeFromInteractableEvents();
            }
        }

        protected virtual void CheckNeededComponents()
        {
            AddComponentIfNotFound(ref eventTrigger);
        }

        public SelectableElement GetNeighbor(SelectableNeighborDirection neighborDirection)
        {
            foreach (SelectableElementNeighborGroup selectableElementNeighborGroup in selectableElementNeighborGroups)
            {
                if (selectableElementNeighborGroup.neighborDirection == neighborDirection)
                {
                    foreach(SelectableElement neighbor in selectableElementNeighborGroup.selectableNeighbors)
                    {
                        if(neighbor.IsInteractable)
                        {
                            return neighbor;
                        }
                    }
                }
            }

            return null;
        }

        protected void AddComponentIfNotFound<T>(ref T componentReference) where T : UnityEngine.Component
        {
            if(componentReference != null)
            {
                return;
            }

            componentReference = GetComponent<T>();

            if(componentReference != null)
            {
                return;
            }

            componentReference = gameObject.AddComponent<T>();
        }

        protected virtual void OnPointerEnter(BaseEventData baseEventData)
        {
            baseEventData.selectedObject = this.gameObject;
        }

        protected virtual void OnPointerExit(BaseEventData baseEventData)
        {
            if(baseEventData.selectedObject == gameObject)
            {
                baseEventData.selectedObject = null;
            }
        }

        protected virtual void OnSubmit(BaseEventData baseEventData)
        {
            if(baseEventData.selectedObject == gameObject)
            {
                onSubmit?.Invoke();
            }

            if(deselectAfterSubmitAction && baseEventData.selectedObject == gameObject)
            {
                baseEventData.selectedObject = null;
            }
        }

        protected virtual void OnSelect(BaseEventData baseEventData)
        {
            onSelect?.Invoke();
        }

        protected virtual void OnDeselect(BaseEventData baseEventData)
        {
            onDeselect?.Invoke();
        }

        protected virtual void OnPointerDown(BaseEventData baseEventData)
        {

        }

        protected virtual void OnPointerUp(BaseEventData baseEventData)
        {

        }

        private void SubscribeToInteractableEvents()
        {
            isSubscribedToInteractableEvents = true;
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerEnter, OnPointerEnter);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerExit, OnPointerExit);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerClick, OnSubmit);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.Submit, OnSubmit);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.Select, OnSelect);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.Deselect, OnDeselect);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerDown, OnPointerDown);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerUp, OnPointerUp);
        }

        private void UnsubscribeFromInteractableEvents()
        {
            isSubscribedToInteractableEvents = false;
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerEnter, OnPointerEnter);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerExit, OnPointerExit);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerClick, OnSubmit);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.Submit, OnSubmit);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.Select, OnSelect);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.Deselect, OnDeselect);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerDown, OnPointerDown);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerUp, OnPointerUp);
        }
    }
}
