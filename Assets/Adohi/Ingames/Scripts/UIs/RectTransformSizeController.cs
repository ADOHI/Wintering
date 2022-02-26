using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class RectTransformSizeController : MonoBehaviour
    {
        private RectTransform rectTransform;

        public Vector2 startSize;
        public Vector2 endSize;
        public Ease ease;
        public float duration;


        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = startSize;
        }

        [Button]
        public void DoAction()
        {
            rectTransform.sizeDelta = startSize;
            rectTransform.DOSizeDelta(endSize, duration).SetEase(ease);
        }
    }
}

