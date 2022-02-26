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
        public TextMeshPro dayUI;
        //public float minDilateValue = 0f;
        //public float maxDilateValue = 0.15f;
        public float dayUIHideduration;
        //public Ease dayUIease;

        public VoidBaseEventReference onDie;
        public VoidBaseEventReference onFin;

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

        [Header("Death")]
        public float deathDelay = 15f;

        private void Awake()
        {
            camera = Camera.main;
        }

        public async void StartPreday()
        {
            //IngameCameraManager.Instance.ZoomInHoleImmediately();

            await UniTask.Delay(3000);

            SoundManager.Instance.PlayBGM();
            ShowTitleAsnyc();

            await UniTask.Delay(2000);
            await foreground.DOFade(0f, duration).AsyncWaitForCompletion();
            //await UniTask.Delay(15000);
            //HideTitleAsnyc();
            /*
            IngameCameraManager.Instance.ZoomOutHoleAsnyc();

            await ShowTitleAsnyc();
            await HideTitleAsnyc();

            StartDay();
            */

            //StartDay();

            DayTimeAsync();
        }

        public async UniTask ShowTitleAsnyc()
        {
            //ShowDilateAsync(title, minTitleDilate, maxTitleDilate, titleDuration);
            //await ShowDilateAsync(subTitle, minTitleDilate, maxTitleDilate, titleDuration);
            title.gameObject.SetActive(true);
            subTitle.gameObject.SetActive(true);
            await UniTask.Delay(15000);
            title.gameObject.SetActive(false);
            subTitle.gameObject.SetActive(false);
        }

        public async UniTask HideTitleAsnyc()
        {
            //ShowDilateAsync(title, maxTitleDilate, minTitleDilate, titleDuration);
            //await ShowDilateAsync(subTitle, maxTitleDilate, minTitleDilate, titleDuration);
            title.gameObject.SetActive(false);
            subTitle.gameObject.SetActive(false);
        }


        public async UniTask StartDay()
        {
            days++;
            IngameCameraManager.Instance.TrackCharacter();
            //ShowDayUI(days).Forget();
            Debug.Log($"Day {days}: Start");
            //await Da&yTimeAsync();
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
                DieAsync();
                //onDie?.Event?.Raise();
                //ingameUICanvas.gameObject.SetActive(false);           
            }
        }

        [Button]
        public async UniTask DieAsync()
        {
            Debug.Log("Die");
            onDie?.Event?.Raise();
            SoundManager.Instance.StopBGM();
            await UniTask.Delay(3000);
            SoundManager.Instance.PlayDieBGM();
            //Á×´Â ¿¬Ãâ
            await UniTask.Delay((int)(deathDelay * 1000f));
            Debug.Log("Fin");
            onFin?.Event?.Raise();

            await GameManager.Instance.EndGameAsync();
            //await GameManager.Instance.EndGameAsync();
        }



        public async UniTask ShowDayUI(int day)
        {
            dayUI.text = $"DAY {day}";
            dayUI.gameObject.SetActive(true);
            //dayUI.gameObject.SetActive(true);
            await UniTask.Delay(15000);
            //title.gameObject.SetActive(false);
            dayUI.gameObject.SetActive(false);
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
            //await ShowDilateAsync(dayUI, -1f, maxDilateValue, dayUIHideduration);
        }


        /*
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
        */
    }

}
