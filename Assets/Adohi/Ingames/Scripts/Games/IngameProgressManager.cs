using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pixelplacement;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

namespace Ingames
{
    public class IngameProgressManager : Singleton<IngameProgressManager>
    {
        private float dayProgress;
        private Camera camera;

        [Header("AssignedObjects")]
        public GameObjectReference character;

        [Header("Settings")]
        public int maxDays = 5;
        public float daytime = 10f;

        [Header("envirionment")]
        public Gradient backgroundGradient;
        public Light2D globalLight;
        public Gradient lightGradient;
        public Color endingLightColor;
        
        public int days;
        public Canvas ingameUICanvas;

        [Header("DayUI")]
        public TextMeshPro dayUI;
        public float dayUIHideduration;
        //public Ease dayUIease;

        public VoidBaseEventReference onDie;
        public VoidBaseEventReference onFin;

        [Header("ForegroundSprite")]
        public SpriteRenderer foreground;
        public float duration;

        [Header("TitleUI")]
        public TextMeshPro title;
        public TextMeshPro subTitle;
        public float minTitleDilate;
        public float maxTitleDilate;
        public float titleDuration;
        public Ease titleEase;

        [Header("Death")]
        public float deathDelay = 15f;



        [Header("Acorns")]
        public int currentAcorn;
        public int currentSavedAcorn;
        public List<int> survivedAcornCondition;
        public List<Image> acornUI;
        public IntEventReference onEatAcorn;

        [Header("Initialize")]
        public Transform cameraStartPosition;
        public float cameraStartOrthogonal;
        public Transform startCharacterPosition;

        private void Awake()
        {
            camera = Camera.main;
        }

        public async void StartPreday()
        {
            InitializeDayStart();

            await UniTask.Delay(3000);
            SoundManager.Instance.PlayBGM();
            if (days == 1)
            {
                ShowTitleAsnyc();
            }
            else
            {
                ShowDayUI(days);
            }
            await UniTask.Delay(2000);
            await foreground.DOFade(0f, duration).AsyncWaitForCompletion();
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

        public async UniTask ShowDayUI(int day)
        {
            dayUI.text = $"DAY {day}";
            dayUI.gameObject.SetActive(true);
            await UniTask.Delay(15000);
            dayUI.gameObject.SetActive(false);
        }

        /*
        public async UniTask HideTitleAsnyc()
        {
            //ShowDilateAsync(title, maxTitleDilate, minTitleDilate, titleDuration);
            //await ShowDilateAsync(subTitle, maxTitleDilate, minTitleDilate, titleDuration);
            title.gameObject.SetActive(false);
            subTitle.gameObject.SetActive(false);
        }
        */

        public async UniTask StartDay()
        {
            //days++;
            IngameCameraManager.Instance.TrackCharacter();
            Debug.Log($"Day {days}: Start");
            /*
            if (days >= 2)
            {
                ShowDayUI(days).Forget();
            }
            */
            //await Da&yTimeAsync();
        }

        public async UniTask DayTimeAsync()
        {
            await StartDay();
            for (float i = 0f; i < daytime; i+=Time.deltaTime)
            {
                dayProgress = i / daytime;
                camera.backgroundColor = backgroundGradient.Evaluate(dayProgress);
                globalLight.color = lightGradient.Evaluate(dayProgress);
                await UniTask.DelayFrame(1);
            }
            dayProgress = 1f;

            await EndDay(true);
        }

        public async UniTask EndDay(bool isSuccess)
        {
            if (isSuccess)
            {


                SoundManager.Instance.StopBGM();
                //ShowTitleAsnyc();
                await UniTask.Delay(2000);
                await foreground.DOFade(1f, duration).AsyncWaitForCompletion();

                currentSavedAcorn += currentAcorn;
                
                if (currentSavedAcorn < survivedAcornCondition[days-1])
                {
                    DieAtHomeAsync();
                    return;
                }

                days++;


                if (days < maxDays)
                {
                    StartPreday();

                }

                else
                {
                    
                }
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

        [Button]
        public async UniTask DieAtHomeAsync()
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

        private void InitializeDayStart()
        {
            character.Value.transform.position = startCharacterPosition.position;
            camera.backgroundColor = backgroundGradient.Evaluate(0f);
            globalLight.color = lightGradient.Evaluate(0f);
            IngameCameraManager.Instance.TrackCharacter(false);
            IngameCameraManager.Instance.SetCameraPosision(cameraStartPosition.position, cameraStartOrthogonal);
        }


        public async UniTask StartEndingAsync()
        {

        }

        public async void ShowAcornUI(int nextAcornCount)
        {
            foreach (var ui in acornUI)
            {
                ui.enabled = false;
            }

            acornUI[nextAcornCount].enabled = true;

            await UniTask.Delay(1000);

            //currentAcorn++;

            acornUI[nextAcornCount + 1].enabled = true;
            acornUI[currentAcorn].enabled = false;

            await UniTask.Delay(1000);

            acornUI[currentAcorn + 1].enabled = false;
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
        /*
        [Button]
        public async void EatAcorn()
        {
           

            if (currentAcorn == 5)
            {
                currentAcorn--;
            }
            //onEatAcorn?.Event?.Raise(currentAcorn);
        }
        */

    }

}
