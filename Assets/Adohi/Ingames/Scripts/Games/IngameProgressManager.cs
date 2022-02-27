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

        public bool isHoleTriggered;

        [Header("AssignedObjects")]
        public GameObjectReference character;

        [Header("Settings")]
        public int maxDays = 5;
        public float daytime = 10f;
        public int maxAcornCount = 4;

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

        [Header("Initialize")]
        public Transform cameraStartPosition;
        public float cameraStartOrthogonal;
        public Transform startCharacterPosition;

        [Header("Events")]
        public IntEventReference onEatAcorn;
        public VoidBaseEventReference onDayStart;
        public VoidBaseEventReference onDayEnd;
        public VoidBaseEventReference onDie;
        public VoidBaseEventReference onSurvived;
        public VoidBaseEventReference onStartEnding;
        public VoidBaseEventReference onFin;
        public VoidBaseEventReference onInitializeDay;

        [Header("Home")]
        public Transform insideHomeWaypoint;

        [Header("Ending")]
        public Light2D pointLight;
        public float minRadius;
        public float maxRadius;
        public Color endingSkyColor;
        public Color endingGlobalLightColor;

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

        public async UniTask StartDay()
        {
            //days++;
            IngameCameraManager.Instance.TrackCharacter();
            onDayStart.Event.Raise();
            Debug.Log($"Day {days}: Start");
        }

        public async UniTask DayTimeAsync()
        {
            await StartDay();
            for (float i = 0f; i < daytime; i+=Time.deltaTime)
            {
                dayProgress = i / daytime;
                camera.backgroundColor = backgroundGradient.Evaluate(dayProgress);
                globalLight.color = lightGradient.Evaluate(dayProgress);
                if (isHoleTriggered)
                {
                    await EndDay(true);
                    return;
                }
                
                await UniTask.DelayFrame(1);
            }
            dayProgress = 1f;
            await EndDay(false);
        }

        public async UniTask EndDay(bool isSuccess)
        {
            onDayEnd.Event.Raise();
;
            if (isSuccess)
            {
                SoundManager.Instance.StopBGM();
                //ShowTitleAsnyc();
                await UniTask.Delay(2000);
                await foreground.DOFade(1f, 3f).AsyncWaitForCompletion();
                camera.backgroundColor = backgroundGradient.Evaluate(1f);
                globalLight.color = lightGradient.Evaluate(1f);
                currentSavedAcorn += currentAcorn;
                ShowHomeInside();
                await foreground.DOFade(0f, 3f).AsyncWaitForCompletion();



                //Acorn is lack
                if (currentSavedAcorn < survivedAcornCondition[days-1])
                {
                    DieAtHomeAsync();
                    return;
                }
                else
                {
                    onSurvived.Event.Raise();
                    await UniTask.Delay(3000);
                    await foreground.DOFade(1f, duration).AsyncWaitForCompletion();
                }


                days++;
                if (days < maxDays)
                {
                    StartPreday();

                }

                else
                {
                    StartEndingAsync();
                }
            }

            else
            {
                DieAsync();
                //onDie?.Event?.Raise();
                //ingameUICanvas.gameObject.SetActive(false);           
            }
        }

        public void ShowHomeInside()
        {
            var offset = Vector3.up * 2.3f;
            IngameCameraManager.Instance.TrackCharacter(false);
            character.Value.transform.position = insideHomeWaypoint.position;
            IngameCameraManager.Instance.SetCameraPosision(insideHomeWaypoint.position + offset, 4.5f);
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
            isHoleTriggered = false;
            currentAcorn = 0;
            onInitializeDay.Event.Raise();
        }


        public async UniTask StartEndingAsync()
        {
            Debug.Log("StartEnding");
            onStartEnding?.Event?.Raise();
            camera.backgroundColor = endingSkyColor;
            globalLight.color = endingLightColor;

            pointLight.pointLightInnerRadius = minRadius;
            await foreground.DOFade(0f, duration).OnUpdate(() => pointLight.pointLightInnerRadius += (maxRadius - minRadius) / duration * Time.deltaTime).AsyncWaitForCompletion();
        }

        public async void ShowAcornUI(int nextAcornCount)
        {
            currentAcorn = nextAcornCount;

            foreach (var ui in acornUI)
            {
                ui.enabled = false;
            }

            acornUI[nextAcornCount - 1].enabled = true;

            await UniTask.Delay(1000);

            //currentAcorn++;

            acornUI[nextAcornCount].enabled = true;
            acornUI[nextAcornCount - 1].enabled = false;

            await UniTask.Delay(1000);

            acornUI[nextAcornCount].enabled = false;
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
        public void GoInside()
        {
            isHoleTriggered = true;
        }
    }

}
