namespace GameBoxSdk.Runtime.UI.Views 
{
    using System;
    
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    
    using GameBoxSdk.Runtime.Sound;
    using GameBoxSdk.Runtime.UI.CoreElements;
    using GameBoxSdk.Runtime.UI.Views.DataContainers;
    using GameBoxSdk.Runtime.Localization;

    [RequireComponent(typeof (Canvas),typeof(CanvasGroup))]
    public abstract class BaseView : MonoBehaviour
    {
        [SerializeField]
        private Image inputBlockerImage = null;

        public event Action onTransitionInFinished;
        public event Action onTransitionOutFinished;

        private ButtonAudioPlayer[] buttonAudioPlayers = new ButtonAudioPlayer[0];
        private SelectableElement[] selectableElements = new SelectableElement[0];
        private IViewAnimator viewAnimator = null;
        private int interactableGroupId = -1;
        private EventSystem eventSystem = null;

        public Canvas Canvas { get; private set; } = null;
        public CanvasGroup CanvasGroup { get; private set; } = null;
        public CanvasScaler CanvasScaler { get; private set; } = null;
        public int InteractableGroupId { get => interactableGroupId; private set => interactableGroupId = value; }
        public SelectableElement[] SelectableElements => selectableElements;
        public SelectableElement currentSelectableElementSelected = null;

        #region Unity Methods

        protected virtual void Awake()
        {
            buttonAudioPlayers = GetComponentsInChildren<ButtonAudioPlayer>();
            selectableElements = GetComponentsInChildren<SelectableElement>();

            viewAnimator = GetComponent<IViewAnimator>();

            if(viewAnimator != null)
            {
                viewAnimator.OnTransitionInAnimationCompleted += OnTransitionInAnimationCompleted;
                viewAnimator.OnTransitionOutAnimatonCompleted += OnTransitionOutAnimatonCompleted;
            }
        }

        protected virtual void OnDestroy()
        {

            if (viewAnimator != null)
            {
                viewAnimator.OnTransitionInAnimationCompleted -= OnTransitionInAnimationCompleted;
                viewAnimator.OnTransitionOutAnimatonCompleted -= OnTransitionOutAnimatonCompleted;
            }
        }

        #endregion

        public void SetInteractable(bool isInractable)
        {
            inputBlockerImage.raycastTarget = !isInractable;
            CanvasGroup.interactable = isInractable;
        }

        public virtual void Initialize(Camera uiCamera, Action<ClipIds> playClipOnce, ViewInjectableData viewInjectableData, Func<string, string> getLocalizedText, EventSystem sourceEventSystem)
        {
            Canvas = GetComponent<Canvas>();
            CanvasGroup = GetComponent<CanvasGroup>();
            CanvasScaler = GetComponent<CanvasScaler>();
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            Canvas.worldCamera = uiCamera;
            CanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            eventSystem = sourceEventSystem;

            foreach(ButtonAudioPlayer buttonAudioPlayer in buttonAudioPlayers)
            {
                buttonAudioPlayer.Initialize(playClipOnce);
            }

            foreach(LocalizedText localizedText in GetComponentsInChildren<LocalizedText>(includeInactive: true))
            {
                localizedText.Initialize(getLocalizedText);
            }
        }

        public virtual void Dispose()
        {
            Canvas = null;
            CanvasGroup = null;
        }

        public virtual void TransitionIn(int sourceInteractableGroupId)
        {
            InteractableGroupId = sourceInteractableGroupId;
            CanvasGroup.alpha = 1;

            if(viewAnimator != null)
            {
                SetInteractable(false);
                viewAnimator.PlayTransitionIn();
            }
            else
            {
                SetInteractable(true);
                onTransitionInFinished?.Invoke();
            }
        }

        public virtual void SelectNeighborButton(SelectableNeighborDirection neighborDirection)
        {
            if(currentSelectableElementSelected == null)
            {
                SelectFirstActiveButton();
                return;
            }

            SelectableElement neighbor = currentSelectableElementSelected.GetNeighbor(neighborDirection);
            
            if(neighbor != null && neighbor.isActiveAndEnabled)
            {
                eventSystem.SetSelectedGameObject(neighbor.gameObject);
                currentSelectableElementSelected = neighbor;
            }
        }

        public virtual void TransitionOut()
        {
            SetInteractable(false);

            if(viewAnimator != null)
            {
                viewAnimator.PlayTransitionOut();
            }
            else
            {
                CanvasGroup.alpha = 0;
                onTransitionOutFinished?.Invoke();
            }
        }

        private void SelectFirstActiveButton()
        {
            foreach (SelectableElement selectableElement in SelectableElements)
            {
                if (selectableElement.IsInteractable)
                {
                    currentSelectableElementSelected = selectableElement;
                    eventSystem.SetSelectedGameObject(selectableElement.gameObject);
                    break;
                }
            }
        }

        private void OnTransitionOutAnimatonCompleted()
        {
            CanvasGroup.alpha = 0;
            onTransitionOutFinished?.Invoke();
        }

        private void OnTransitionInAnimationCompleted()
        {
            SetInteractable(true);
            onTransitionInFinished?.Invoke();
        }
    }
}

