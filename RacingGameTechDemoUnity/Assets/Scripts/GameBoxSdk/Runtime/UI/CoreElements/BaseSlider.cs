namespace GameBoxSdk.Runtime.UI.CoreElements
{
    using System;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class BaseSlider : SelectableElement
    {
        public event Action<float> OnValueChanged = null;

        [SerializeField]
        private Slider slider = null;

        [SerializeField]
        private Color sliderSelectedColor = Color.white;

        [SerializeField]
        private Image fillerImage = null;

        [SerializeField]
        private Image backgroundFillerImage = null;

        private bool isPointerDown = false;
        private bool hasPointerExited = false;
        private Color fillerImageOriginalColor = Color.white;
        private Color backgroundFillerImageOriginalColor = Color.white;

        public float Value => slider.value;

        protected override void Awake()
        {
            base.Awake();
            fillerImageOriginalColor = fillerImage.color;
            backgroundFillerImageOriginalColor = backgroundFillerImage.color;
        }

        public void UpdateSliderValue(float newValue)
        {
            slider.value = Mathf.Clamp01(newValue);
        }

        protected override void CheckNeededComponents()
        {
            base.CheckNeededComponents();
            AddComponentIfNotFound(ref slider);
        }

        protected override void OnPointerExit(BaseEventData baseEventData)
        {
            hasPointerExited = true;

            if(!isPointerDown)
            {
                base.OnPointerExit(baseEventData);
                hasPointerExited = false;
            }
        }

        protected override void OnPointerDown(BaseEventData baseEventData)
        {
            isPointerDown = true;
        }

        protected override void OnPointerUp(BaseEventData baseEventData)
        {
            base.OnPointerUp(baseEventData);

            if(hasPointerExited)
            {
                base.OnPointerExit(baseEventData);
                hasPointerExited = false;
            }

            isPointerDown = false;
            OnValueChanged?.Invoke(slider.value);
        }

        protected override void OnSelect(BaseEventData baseEventData)
        {
            base.OnSelect(baseEventData);
            fillerImage.color *= sliderSelectedColor;
            backgroundFillerImage.color *= sliderSelectedColor;
        }

        protected override void OnDeselect(BaseEventData baseEventData)
        {
            base.OnDeselect(baseEventData);
            fillerImage.color = fillerImageOriginalColor;
            backgroundFillerImage.color = backgroundFillerImageOriginalColor;
            OnValueChanged?.Invoke(slider.value);
        }
    }
}
