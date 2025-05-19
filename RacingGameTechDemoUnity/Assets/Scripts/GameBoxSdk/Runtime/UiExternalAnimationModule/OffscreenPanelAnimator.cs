namespace GameBoxSdk.Runtime.UiExternalAnimationModule
{
    using System;

    using UnityEngine;

    public class OffscreenPanelAnimator : ViewAnimator
    {
        [Serializable]
        private struct UiElementAnimConfiguration
        {
            [SerializeField]
            public RectTransform rectTransform;

            [SerializeField, Range(-1, 1)]
            public int horizontalEntranceDirection;

            [SerializeField, Range(-1, 1)]
            public int verticalEntranceDirection;

            [HideInInspector]
            public Vector2 startPosition;

            [HideInInspector]
            public Vector2 targetPosition;
        }

        [SerializeField]
        private RectTransform rootContainer = null;

        [SerializeField]
        private UiElementAnimConfiguration[] uiElementAnimConfigurations = new UiElementAnimConfiguration[0];

        protected override void OnTransitionInAnimationPreStart()
        {
            base.OnTransitionInAnimationPreStart();

            for(int i = 0; i < uiElementAnimConfigurations.Length; i++)
            {
                uiElementAnimConfigurations[i].startPosition = GetOffScreenPosition(
                    uiElementAnimConfigurations[i].rectTransform,
                    uiElementAnimConfigurations[i].horizontalEntranceDirection,
                    uiElementAnimConfigurations[i].verticalEntranceDirection);
                uiElementAnimConfigurations[i].targetPosition = uiElementAnimConfigurations[i].rectTransform.anchoredPosition;
            }

            //NOTE: DO NOT PUT THIS LINE OF CODE ON THE LOOP ABOVE, FOR SOME REASON UNITY IS RETURNING DIFFERENT VALUES
            //IN THE ROOT UI OBJECT THE MOMENT AN ELEMENT IS MOVED OUT OF IT. 
            for (int i = 0; i < uiElementAnimConfigurations.Length; i++)
            {
                uiElementAnimConfigurations[i].rectTransform.anchoredPosition = uiElementAnimConfigurations[i].startPosition;
            }
        }

        protected override void OnTransitionOutAnimationPreStart()
        {
            base.OnTransitionOutAnimationPreStart();

            for (int i = 0; i < uiElementAnimConfigurations.Length; i++)
            {
                uiElementAnimConfigurations[i].startPosition = uiElementAnimConfigurations[i].rectTransform.anchoredPosition;
                uiElementAnimConfigurations[i].targetPosition = GetOffScreenPosition(
                    uiElementAnimConfigurations[i].rectTransform, 
                    uiElementAnimConfigurations[i].horizontalEntranceDirection, 
                    uiElementAnimConfigurations[i].verticalEntranceDirection);
            }
        }

        protected override void OnTweenAnimationUpdate(float animationCurveEvaluatedValue)
        {
            base.OnTweenAnimationUpdate(animationCurveEvaluatedValue);

            for (int i = 0; i < uiElementAnimConfigurations.Length; i++)
            {
                float magnitude = Vector2.Distance(
                    uiElementAnimConfigurations[i].startPosition, 
                    uiElementAnimConfigurations[i].targetPosition);
                Vector2 direction = (uiElementAnimConfigurations[i].targetPosition - uiElementAnimConfigurations[i].startPosition).normalized;
                uiElementAnimConfigurations[i].rectTransform.anchoredPosition =
                    uiElementAnimConfigurations[i].startPosition + (direction * magnitude * animationCurveEvaluatedValue);
            }
        }

        private Vector2 GetOffScreenPosition(RectTransform rectTransform, float horizontalEntranceDirection, float verticalEntranceDirection)
        {
            Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(rectTransform, rootContainer);
            float horizontalStartPosition = rectTransform.anchoredPosition.x;
            float verticalStartPosition = rectTransform.anchoredPosition.y;

            if (horizontalEntranceDirection != 0)
            {
                float distanceToHorizontalCorner = Mathf.Abs(bounds.center.x - (horizontalEntranceDirection * rootContainer.rect.width / 2));
                distanceToHorizontalCorner += rectTransform.rect.width / 2;
                horizontalStartPosition = horizontalStartPosition - (horizontalEntranceDirection * distanceToHorizontalCorner);
            }

            if (verticalEntranceDirection != 0)
            {
                float distanceToVerticalCorner = Mathf.Abs(bounds.center.y - (verticalEntranceDirection * rootContainer.rect.height / 2));
                distanceToVerticalCorner += rectTransform.rect.height / 2;
                verticalStartPosition = verticalStartPosition - (verticalEntranceDirection * distanceToVerticalCorner);
            }

            return new Vector2(horizontalStartPosition, verticalStartPosition);
        }
    }

}
