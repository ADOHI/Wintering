using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TmpColorFadeController : MonoBehaviour
    {
        private TextMeshProUGUI tmpUgui;
        private TextMeshPro tmp;

        public Color startColor;
        public Color endColor;
        public float delay;
        public float fadeDuration;
        public Ease ease;


        private void Awake()
        {
            TryGetComponent(out tmpUgui);
            TryGetComponent(out tmp);

            if (tmpUgui != null)
            {
                tmpUgui.color = startColor;
            }

            if (tmp != null)
            {
                tmp.color = startColor;
            }
        }

        public async void Fade()
        {
            await UniTask.Delay((int)(delay * 1000f));

            if (tmpUgui != null)
            {
                tmpUgui.DOColor(endColor, fadeDuration).SetEase(ease);
            }

            if (tmp != null)
            {
                tmp.DOColor(endColor, fadeDuration).SetEase(ease);
            }
        }

    }

}
