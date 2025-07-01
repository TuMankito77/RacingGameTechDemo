namespace GameBoxSdk.Runtime.UI
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using UnityEngine;

    using GameBoxSdk.Runtime.Core;
    using GameBoxSdk.Runtime.UI.Views;
    using GameBoxSdk.Runtime.Input;
    using GameBoxSdk.Runtime.Sound;
    using GameBoxSdk.Runtime.Utils;
    using UnityEngine.InputSystem.UI;
    using UnityEngine.Rendering.Universal;
    using GameBoxSdk.Runtime.UI.Views.DataContainers;
    using UnityEngine.EventSystems;

    public class UiManager : BaseSystem, IInputControlableEntity
    {
        private const string EVENT_SYSTEM_PREFAB_PATH = "GameBox/Ui/EventSystem";
        private const int BACKGROUND_SORTING_GROUP = -1;

        private ViewsDatabase viewsDatabase = null;
        private GameObject uiManagerGO = null;
        private Camera uiCamera = null;
        private List<BaseView> viewsOpened = null;
        private EventSystem eventSystem = null;
        private Func<string, string> getLocalizedText = null;
        private Action<ClipIds> playClipOnce = null;

        public int TopInteractbleGroupId { get; private set; } = 0;
        
        public UiManager(Func<string, string> sourceGetLocalizedText, Action<ClipIds> sourcePlayClipOnce) : base()
        {
            getLocalizedText = sourceGetLocalizedText;
            playClipOnce = sourcePlayClipOnce;
        }

        //To-do: Create a request class that will be sent through an event in order to request a view.
        public override async Task<bool> Initialize(IEnumerable<BaseSystem> sourceDependencies)
        {
            await base.Initialize(sourceDependencies);

            viewsOpened = new List<BaseView>();
            //To-do: Create a database that categorizes the objects loaded based on a list that groups what is needed to be loaded depending on what needs to be shown.
            ContentLoader contentLoader = GetDependency<ContentLoader>();
            viewsDatabase = await contentLoader.LoadAsset<ViewsDatabase>(ViewsDatabase.VIEWS_DATABASE_SCRIPTABLE_OBJECT_PATH);
            
            if(viewsDatabase == null)
            {
                return false;
            }
            
            viewsDatabase.Initialize();

            uiManagerGO = new GameObject("UI Manager");
            GameObject.DontDestroyOnLoad(uiManagerGO);

            uiCamera = new GameObject("UI Camera").AddComponent<Camera>();
            uiCamera.cullingMask = viewsDatabase.ViewsLayerMask;
            uiCamera.clearFlags = CameraClearFlags.Nothing;
            uiCamera.gameObject.transform.SetParent(uiManagerGO.transform);
            uiCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            CameraStackingManager cameraStackingManager = GetDependency<CameraStackingManager>();
            cameraStackingManager.AddCameraToStackAtTop(uiCamera);

            InputSystemUIInputModule inputSystemUIInputModulePrefab = await contentLoader.LoadAsset<InputSystemUIInputModule>(EVENT_SYSTEM_PREFAB_PATH);
            InputSystemUIInputModule inputSystemUIInputModuleInstance = GameObject.Instantiate(inputSystemUIInputModulePrefab);
            inputSystemUIInputModuleInstance.transform.SetParent(uiManagerGO.transform);
            eventSystem = inputSystemUIInputModuleInstance.GetComponent<EventSystem>();

            return true;
        }

        public BaseView DisplayView(ViewIds viewId, bool placeInSeparateInteractableGroup, ViewInjectableData viewInjectableData = null, int interactableGroupStackPlacement = 0)
        {
            GetViewIndexAndInteractableGroupId(interactableGroupStackPlacement, placeInSeparateInteractableGroup, out int viewIndex, out int interactableGroupId);
            DisableBottomViews(viewIndex, interactableGroupId);
            BaseView viewFound = GameObject.Instantiate(viewsDatabase.GetFile(viewId.ToString()), uiManagerGO.transform);
            viewFound.Initialize(this, uiCamera, playClipOnce, viewInjectableData, getLocalizedText, eventSystem);
            //NOTE: This will update the values like the width and height so that they do not appear as zero,
            //dunno how I will remind this to myself -_-, BUT remember, we have to do this before trying to access any RectTransform values
            Canvas.ForceUpdateCanvases();
            viewFound.Canvas.sortingOrder = viewIndex;
            viewFound.TransitionIn(interactableGroupId);
            viewFound.transform.SetParent(uiManagerGO.transform);
            viewsOpened.Insert(viewIndex, viewFound);
            TopInteractbleGroupId = viewsOpened[viewsOpened.Count - 1].InteractableGroupId;
            return viewFound;
        }

        public BaseView GetTopStackView(ViewIds viewId)
        {
            Type viewType = viewsDatabase.GetFile(viewId.ToString()).GetType();

            for (int i = viewsOpened.Count - 1; i >= 0; i--)
            {
                if (viewsOpened[i].GetType() == viewType)
                {
                    return viewsOpened[i];
                }
            }

            LoggerUtil.LogError($"{GetType()}: There is no view with the id {viewId} being displayed currently.");
            return null;
        }

        public BaseView CurrentViewDisplayed()
        {
            return viewsOpened.Count > 0 ? viewsOpened[viewsOpened.Count - 1] : null;
        }

        public void RemoveView(ViewIds viewId)
        {
            Type viewType = viewsDatabase.GetFile(viewId.ToString()).GetType();
            
            if(!viewsOpened.Exists((view) => view.GetType() == viewType))
            {
                return;
            }

            BaseView viewFound = viewsOpened.FindLast((view) => view.GetType() == viewType);
            RemoveView(viewFound);
        }

        public void RemoveTopStackView()
        {
            int lastIndex = viewsOpened.Count - 1;
            BaseView topStackView = viewsOpened[lastIndex];
            RemoveView(topStackView);
        }

        public void RemoveTopStackInteractableGroup()
        {
            while(CurrentViewDisplayed().InteractableGroupId == TopInteractbleGroupId)
            {
                RemoveTopStackView();
            }
        }

        private void RemoveView(BaseView view)
        {
            int viewIndex = view.Canvas.sortingOrder;
            viewsOpened.Remove(view);

            void OnTransitionOutFinished()
            {
                view.onTransitionOutFinished -= OnTransitionOutFinished;
                view.Dispose();

                for (int i = viewIndex; i < viewsOpened.Count; i++)
                {
                    viewsOpened[i].Canvas.sortingOrder = i;
                }

                //To-do: Make this be handled by a pool so that it can be reused.
                GameObject.Destroy(view.gameObject);

                BaseView currentViewDisplayed = CurrentViewDisplayed();

                if (currentViewDisplayed != null && currentViewDisplayed.InteractableGroupId != TopInteractbleGroupId)
                {
                    TopInteractbleGroupId = currentViewDisplayed.InteractableGroupId;

                    for(int i = viewsOpened.Count - 1; i >= 0; i--)
                    {
                        BaseView view = viewsOpened[i];

                        if (view.InteractableGroupId != TopInteractbleGroupId)
                        {
                            break;
                        }

                        view.SetInteractable(true);
                    }
                }
            }

            view.onTransitionOutFinished += OnTransitionOutFinished;
            view.TransitionOut();
        }

        private void GetViewIndexAndInteractableGroupId(int interactableGroupStackPlacement, bool placeInSeparateInteractableGroup, out int viewIndex, out int interactableGroupId)
        {
            if(placeInSeparateInteractableGroup)
            {
                FindViewIndexAddingNewGroupId(interactableGroupStackPlacement, out viewIndex, out interactableGroupId);
            }
            else
            {
                FindViewIndexWithinExistingGroupIds(interactableGroupStackPlacement, out viewIndex, out interactableGroupId);
            }
        }

        private void FindViewIndexAddingNewGroupId(int interactableGroupStackPlacement, out int viewIndex, out int interactableGroupId)
        {
            int lastInteractableGroupIdPassed = TopInteractbleGroupId + 1;
            int groupIdsPassed = 0;
            int openedViewIndex = viewsOpened.Count - 1;
            
            while(groupIdsPassed < interactableGroupStackPlacement && openedViewIndex >= 0)
            {
                BaseView openedView = viewsOpened[openedViewIndex];

                if(lastInteractableGroupIdPassed != openedView.InteractableGroupId)
                {
                    lastInteractableGroupIdPassed = openedView.InteractableGroupId;
                    groupIdsPassed++;

                    if (groupIdsPassed > interactableGroupStackPlacement)
                    {
                        viewIndex = openedViewIndex + 1;
                        interactableGroupId = openedView.InteractableGroupId + 1;
                        return;
                    }
                }

                openedView.IncreaseInteractableGroupId();
                openedView.Canvas.sortingOrder++;
                openedViewIndex--;
            }

            if(openedViewIndex < 0)
            {
                viewIndex = 0;
                interactableGroupId = 0;
            }
            else
            {
                viewIndex = viewsOpened.Count;
                interactableGroupId = TopInteractbleGroupId + 1;
            }
        }

        private void FindViewIndexWithinExistingGroupIds(int interactableGroupStackPlacement, out int viewIndex, out int interactableGroupId)
        {
            int lastInteractableGroupIdPassed = TopInteractbleGroupId;
            int groupIdsPassed = 0;
            int openedViewIndex = viewsOpened.Count - 1; 

            while (groupIdsPassed < interactableGroupStackPlacement && openedViewIndex >= 0)
            {
                BaseView openedView = viewsOpened[openedViewIndex];

                if (lastInteractableGroupIdPassed != openedView.InteractableGroupId)
                {
                    lastInteractableGroupIdPassed = openedView.InteractableGroupId;
                    groupIdsPassed++;

                    if (groupIdsPassed == interactableGroupStackPlacement)
                    {
                        viewIndex = openedViewIndex + 1;
                        interactableGroupId = openedView.InteractableGroupId;
                        return;
                    }
                }

                openedView.Canvas.sortingOrder++;
                openedViewIndex--;
            }

            if(openedViewIndex < 0)
            {
                viewIndex = 0;
                interactableGroupId = 0;
            }
            else
            {
                viewIndex = viewsOpened.Count;
                interactableGroupId = TopInteractbleGroupId;
            }
        }

        private void DisableBottomViews(int startingIndex, int correspondingInteractableGroupId)
        {
            for (int i = startingIndex - 1; i >= 0; i--)
            {
                BaseView openedView = viewsOpened[i];

                if (!openedView.IsInteractable)
                {
                    break;
                }

                if (openedView.InteractableGroupId == correspondingInteractableGroupId)
                {
                    continue;
                }

                openedView.SetInteractable(false);
            }
        }
    }
}
