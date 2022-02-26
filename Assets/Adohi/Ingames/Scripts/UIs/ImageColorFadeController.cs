using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ImageColorFadeController : MonoBehaviour
    {
        private Image image;

        public Color startColor;
        public Color endColor;
        public float duration;
        public Ease ease;

        public VoidBaseEventReference onFadeStart;
        public VoidBaseEventReference onFadeEnd;

        private void Awake()
        {
            image = GetComponent<Image>();
            image.color = startColor;
        }

        public void Fade()
        {
            onFadeStart?.Event?.Raise();
            if (onFadeEnd.Event != null)
            {
                image.DOColor(endColor, duration).SetEase(ease).OnComplete(onFadeEnd.Event.Raise);
            }
            else
            {
                image.DOColor(endColor, duration).SetEase(ease);
            }
        }
    }

}
