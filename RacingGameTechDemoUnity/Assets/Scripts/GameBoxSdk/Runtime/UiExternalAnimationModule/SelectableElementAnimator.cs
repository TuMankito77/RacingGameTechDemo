namespace GameBoxSdk.Runtime.UiExternalAnimationModule
{
    using System;
    
    using UnityEngine;

    using DG.Tweening;

    using GameBoxSdk.Runtime.UI.CoreElements;

    public class SelectableElementAnimator : MonoBehaviour, ISeletableElementAnimator
    {
        private event Action onSubmitAnimationStart = null;
        private event Action onSubmitAnimationEnd = null;

        [SerializeField, Min(0)]
        private float submitAnimationDuration = 0.5f;

        [SerializeField, Min(0)]
        private float selectAnimationDuration = 0.25f;

        [SerializeField]
        private bool playAnimationOnSubmit = true;

        [SerializeField]
        private AnimationCurve selectAnimationCurve = AnimationCurve.Linear(0, 1, 1, 1.2f);

        [SerializeField]
        private AnimationCurve submitAnimationCurve = AnimationCurve.Linear(0, 0.5f, 1, 1);

        private bool isDoingSubmitAnimation = false;
        private Tweener currentTween = null;
        private SelectableElement selectableElement = null;
        private float timeInAnimationCurve = 0;

        #region ISelectableElementAnimator

        public event Action OnSubmitAnimationStart
        {
            add => onSubmitAnimationStart += value;
            remove => onSubmitAnimationStart -= value;
        }

        public event Action OnSubmitAnimationEnd
        {
            add => onSubmitAnimationEnd += value;
            remove => onSubmitAnimationEnd -= value;
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            selectableElement = GetComponent<SelectableElement>();
            selectableElement.SetAnimationExternalModule(this);
            selectableElement.onSelect += OnElementSelected;
            selectableElement.onDeselect += OnElementDeselected;

            if (playAnimationOnSubmit)
            {
                selectableElement.onSubmit += OnElementSubmit;
            }
        }

        private void OnDestroy()
        {
            selectableElement.onSelect -= OnElementSelected;
            selectableElement.onDeselect -= OnElementDeselected;

            if(playAnimationOnSubmit)
            {
                selectableElement.onSubmit -= OnElementSubmit;
            }

            if(currentTween.IsActive())
            {
                currentTween.Kill();
            }
        }

        #endregion

        private void OnElementSelected()
        {
            if(isDoingSubmitAnimation)
            {
                return;
            }

            if(currentTween.IsActive())
            {
                currentTween.Kill();
            }

            float startValue = selectAnimationCurve.keys[0].time;
            float targetValue = selectAnimationCurve.keys[selectAnimationCurve.keys.Length - 1].time;
            currentTween = DOTween.To(UpdateTimeInAnimationCurve, startValue, targetValue, selectAnimationDuration).SetEase(Ease.Linear);
            currentTween.onUpdate += () => transform.localScale = Vector3.one * selectAnimationCurve.Evaluate(timeInAnimationCurve);
        }

        private void OnElementDeselected()
        {
            if (isDoingSubmitAnimation)
            {
                return;
            }

            if (currentTween.IsActive())
            {
                currentTween.Kill();
            }

            float startValue = selectAnimationCurve.keys[selectAnimationCurve.keys.Length - 1].time;
            float targetValue = selectAnimationCurve.keys[0].time;
            currentTween = DOTween.To(UpdateTimeInAnimationCurve, startValue, targetValue, selectAnimationDuration).SetEase(Ease.Linear);
            currentTween.onUpdate += () => transform.localScale = Vector3.one * selectAnimationCurve.Evaluate(timeInAnimationCurve);
        }

        private void OnElementSubmit()
        {
            if(isDoingSubmitAnimation)
            {
                return;
            }

            onSubmitAnimationStart?.Invoke();

            if (currentTween.IsActive())
            {
                currentTween.Kill();
            }

            isDoingSubmitAnimation = true;
            float startValue = submitAnimationCurve.keys[0].time;
            float targetValue = submitAnimationCurve.keys[submitAnimationCurve.keys.Length - 1].time;
            currentTween = DOTween.To(UpdateTimeInAnimationCurve, startValue, targetValue, submitAnimationDuration).SetEase(Ease.Linear);
            currentTween.onUpdate += () => transform.localScale = Vector3.one * submitAnimationCurve.Evaluate(timeInAnimationCurve);
            currentTween.onComplete += OnTweenAnimationCompleted;
        }

        private void OnTweenAnimationCompleted()
        {
            isDoingSubmitAnimation = false;
            transform.localScale = Vector3.one;
            onSubmitAnimationEnd?.Invoke();
            currentTween.onComplete -= OnTweenAnimationCompleted;
        }

        private void UpdateTimeInAnimationCurve(float time)
        {
            timeInAnimationCurve = time;
        }
    }
}
