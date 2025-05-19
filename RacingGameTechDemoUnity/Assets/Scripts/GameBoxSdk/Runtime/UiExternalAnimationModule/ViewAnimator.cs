namespace GameBoxSdk.Runtime.UiExternalAnimationModule
{
    using System;

    using UnityEngine;

    using DG.Tweening;
    
    using GameBoxSdk.Runtime.UI.CoreElements;

    public abstract class ViewAnimator : MonoBehaviour, IViewAnimator
    {
        [SerializeField, Min(0.01f)]
        private float animationDuration = 1f;

        [SerializeField]
        private AnimationCurve transitionAnimaitonCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Tweener currentTween = null;
        protected TweenCallback onTweenAnimationFinished = null;

        #region Unity Methods

        private void OnDestroy()
        {
            if (currentTween.IsActive())
            {
                currentTween.Kill();
            }
        }

        #endregion

        #region IViewAnimator

        public event Action OnTransitionInAnimationCompleted;
        public event Action OnTransitionOutAnimatonCompleted;

        public void PlayTransitionIn()
        {
            onTweenAnimationFinished += OnTransitionInTweenAnimationCompleted;
            OnTransitionInAnimationPreStart();
            PlayTweenAnimation();
        }
        public void PlayTransitionOut()
        {
            onTweenAnimationFinished += OnTransitionOutTweenAnimationCompleted;
            OnTransitionOutAnimationPreStart();
            PlayTweenAnimation();
        }

        #endregion

        protected virtual void OnTransitionInAnimationPreStart()
        {

        }

        protected virtual void OnTransitionOutAnimationPreStart()
        {

        }

        protected virtual void OnTweenAnimationUpdate(float animationCurveEvaluatedValue)
        {

        }

        private void OnTransitionInTweenAnimationCompleted()
        {
            onTweenAnimationFinished -= OnTransitionInTweenAnimationCompleted;
            OnTransitionInAnimationCompleted?.Invoke();
        }

        private void OnTransitionOutTweenAnimationCompleted()
        {
            onTweenAnimationFinished -= OnTransitionOutTweenAnimationCompleted;
            OnTransitionOutAnimatonCompleted?.Invoke();
        }

        private void PlayTweenAnimation()
        {
            float startValue = transitionAnimaitonCurve.keys[0].time;
            float targetValue = transitionAnimaitonCurve.keys[transitionAnimaitonCurve.keys.Length - 1].time;
            currentTween = DOTween.To(UpdateTimeInAnimationCurve, startValue, targetValue, animationDuration).SetEase(Ease.Linear);
            currentTween.onComplete += onTweenAnimationFinished;
        }

        private void UpdateTimeInAnimationCurve(float time)
        {
            OnTweenAnimationUpdate(transitionAnimaitonCurve.Evaluate(time));
        }
    }
}
