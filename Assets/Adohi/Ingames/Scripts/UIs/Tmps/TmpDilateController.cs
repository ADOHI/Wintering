using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace UI
{
    public class TmpDilateContoller : MonoBehaviour
    {
        public TextMeshPro tmp;
        public float minDilate;
        public float maxDilate;
        public float duuration;
        public Ease ease;

        public VoidBaseEventReference onFadeInStart;
        public VoidBaseEventReference onFadeInEnd;
        public VoidBaseEventReference onFadeOutStart;
        public VoidBaseEventReference onFadeOutEnd;

        private void Awake()
        {
            tmp = GetComponent<TextMeshPro>();
        }

        public async void DoFadeIn()
        {
            DoFadeInAsync().Forget();
        }

        public async UniTask DoFadeInAsync()
        {
            onFadeInStart?.Event?.Raise();
            await ShowDilateAsync(tmp, minDilate, maxDilate, duuration);
            onFadeInEnd?.Event?.Raise();
        }

        public async UniTask DoFadeOut()
        {
            DoFadeOutAsync().Forget();
        }

        public async UniTask DoFadeOutAsync()
        {
            onFadeOutStart?.Event?.Raise();
            await ShowDilateAsync(tmp, maxDilate, minDilate, duuration);
            onFadeOutEnd?.Event?.Raise();
        }

        public async UniTask ShowDilateAsync(TextMeshPro tmp, float fromValue, float toValue, float duration)
        {
            tmp.gameObject.SetActive(true);
            for (float i = 0f; i < duration; i += Time.deltaTime)
            {
                var uiProgress = i / duration;
                tmp.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, DOVirtual.EasedValue(fromValue, toValue, uiProgress, ease));
                await UniTask.DelayFrame(1);
            }
            tmp.gameObject.SetActive(true);
        }
    }

}
