using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pixelplacement;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Ingames
{
    public class IngameProgressManager : Singleton<IngameProgressManager>
    {
        private float dayProgress;
        private Camera camera;

        public int maxDays = 5;
        public float daytime = 10f;
        public int days;
        public Gradient backgroundGradient;
        public Canvas ingameUICanvas;

        [Header("DayUI")]
        public TextMeshProUGUI dayUI;
        public float minDilateValue = 0f;
        public float maxDilateValue = 0.15f;
        public float dayUIHideduration;
        public Ease dayUIease;

        public VoidBaseEventReference onDie;

        [Header("ForegroundSprite")]
        public SpriteRenderer foreground;
        public float duration;

        [Header("Title")]
        public TextMeshPro title;
        public TextMeshPro subTitle;
        public float minTitleDilate;
        public float maxTitleDilate;
        public float titleDuration;
        public Ease titleEase;
        //[Header("PreGameSetting")]


        private void Awake()
        {
            camera = Camera.main;
        }

        public async void StartPreday()
        {
            IngameCameraManager.Instance.ZoomInHoleImmediately();

            await UniTask.Delay(3000);

            foreground.DOFade(0f, duration);
            await UniTask.Delay(3000);

            IngameCameraManager.Instance.ZoomOutHoleAsnyc();

            await ShowTitleAsnyc();
            await HideTitleAsnyc();

            StartDay();
        }

        public async UniTask ShowTitleAsnyc()
        {
            ShowDilateAsync(title, minTitleDilate, maxTitleDilate, titleDuration);
            await ShowDilateAsync(subTitle, minTitleDilate, maxTitleDilate, titleDuration);
        }

        public async UniTask HideTitleAsnyc()
        {
            ShowDilateAsync(title, maxTitleDilate, minTitleDilate, titleDuration);
            await ShowDilateAsync(subTitle, maxTitleDilate, minTitleDilate, titleDuration);
        }


        public async UniTask StartDay()
        {
            days++;
            IngameCameraManager.Instance.TrackCharacter();
            ShowDayUI(days).Forget();
        }

        public async UniTask DayTimeAsync()
        {
            await StartDay();
            for (float i = 0f; i < daytime; i+=Time.deltaTime)
            {
                dayProgress = i / daytime;
                camera.backgroundColor = backgroundGradient.Evaluate(dayProgress);
                await UniTask.DelayFrame(1);
            }
            dayProgress = 1f;

            await EndDay(false);
        }

        public async UniTask EndDay(bool isSuccess)
        {
            if (isSuccess)
            {

            }

            else
            {
                onDie?.Event?.Raise();
                //ingameUICanvas.gameObject.SetActive(false);
                
            }
        }

        [Button]
        public async UniTask DieAsync()
        {
            onDie?.Event?.Raise();

            //Á×´Â ¿¬Ãâ

            await UniTask.Delay(5000);

            await GameManager.Instance.EndGameAsync();
        }

        public async UniTask ShowDayUI(int day)
        {
            /*
            dayUI.text = $"DAY {day}";
            dayUI.gameObject.SetActive(true);
            for (float i = 0f; i < dayUIHideduration; i += Time.deltaTime)
            {
                var uiProgress = i / dayUIHideduration;
                dayUI.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, DOVirtual.EasedValue(maxDilateValue, minDilateValue, uiProgress, dayUIease));
                await UniTask.DelayFrame(1);
            }
            dayUI.gameObject.SetActive(true);
            */
            await ShowDilateAsync(dayUI, -1f, maxDilateValue, dayUIHideduration);
        }


        public async UniTask ShowDilateAsync(TextMeshPro tmp, float fromValue, float toValue, float duration)
        {
            tmp.gameObject.SetActive(true);
            for (float i = 0f; i < duration; i += Time.deltaTime)
            {
                var uiProgress = i / duration;
                tmp.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, DOVirtual.EasedValue(fromValue, toValue, uiProgress, dayUIease));
                await UniTask.DelayFrame(1);
            }
            tmp.gameObject.SetActive(true);
        }

        public async UniTask ShowDilateAsync(TextMeshProUGUI tmp, float fromValue, float toValue, float duration)
        {
            tmp.gameObject.SetActive(true);
            for (float i = 0f; i < duration; i += Time.deltaTime)
            {
                var uiProgress = i / duration;
                tmp.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, DOVirtual.EasedValue(fromValue, toValue, uiProgress, dayUIease));
                await UniTask.DelayFrame(1);
            }
            tmp.gameObject.SetActive(true);
        }
    }

}
