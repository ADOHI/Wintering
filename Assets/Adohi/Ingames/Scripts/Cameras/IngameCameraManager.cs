using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pixelplacement;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Ingames
{
    public class IngameCameraManager : Singleton<IngameCameraManager>
    {
        public Camera camera;


        [Header("CharacterTracking")]
        public GameObject character;
        public Vector3 cameraOffset;
        public float trackingDistance;
        public float trackDistanceLerpValue;
        public float trackLerpValue;
        public bool isTracking;

        [Header("HeightLeap")]
        public float distanceMultiply;
        public float minHeight = 1f;
        public FloatReference height;
        public BoolReference isLaddering;

        [Header("House")]
        public Transform zoomInPoint;
        public Transform zoomOutPoint;
        public float zoomInValue;
        public float zoomOutValue;
        public float duration;
        public Ease ease;

        private void Awake()
        {
            camera = Camera.main;
        }

        private void LateUpdate()
        {
            if (isTracking)
            {
                var targetDistance = trackingDistance;

                if (isLaddering)
                {
                    var clampedHeight = Mathf.Clamp(height - minHeight, 0f, height);
                    targetDistance += clampedHeight * distanceMultiply; 
                }

                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetDistance, Time.deltaTime * trackDistanceLerpValue);
                camera.transform.position = Vector3.Lerp(camera.transform.position, character.transform.position + cameraOffset, Time.deltaTime * trackLerpValue);
            }
        }

        public void ZoomInHoleImmediately()
        {
            var cameraOffset = Vector3.back * 10f;
            camera.transform.position = zoomInPoint.position + cameraOffset;
            camera.orthographicSize = zoomInValue;
        }

        [Button]
        public async void ZoomInHole()
        {
            isTracking = false;

            var cameraOffset = Vector3.back * 10f;
            camera.transform.position = zoomOutPoint.position + cameraOffset;
            camera.orthographicSize = zoomOutValue;
            camera.DOOrthoSize(zoomInValue, duration).SetEase(ease);
            camera.transform.DOMove(zoomInPoint.position + cameraOffset, duration).SetEase(ease);
        }

        [Button]
        public async void ZoomOutHole()
        {
            var cameraOffset = Vector3.back * 10f;
            camera.transform.position = zoomInPoint.position + cameraOffset;
            camera.orthographicSize = zoomInValue;
            camera.DOOrthoSize(zoomOutValue, duration).SetEase(ease);
            await camera.transform.DOMove(zoomOutPoint.position + cameraOffset, duration).SetEase(ease).AsyncWaitForCompletion();

            //isTracking = true;
        }

        [Button]
        public async UniTask ZoomInHoleAsync()
        {
            isTracking = false;

            var cameraOffset = Vector3.back * 10f;
            camera.transform.position = zoomOutPoint.position + cameraOffset;
            camera.orthographicSize = zoomOutValue;
            camera.DOOrthoSize(zoomInValue, duration).SetEase(ease);
            await camera.transform.DOMove(zoomInPoint.position + cameraOffset, duration).SetEase(ease).AsyncWaitForCompletion();
        }

        [Button]
        public async UniTask ZoomOutHoleAsnyc()
        {
            var cameraOffset = Vector3.back * 10f;
            camera.transform.position = zoomInPoint.position + cameraOffset;
            camera.orthographicSize = zoomInValue;
            camera.DOOrthoSize(zoomOutValue, duration).SetEase(ease);
            await camera.transform.DOMove(zoomOutPoint.position + cameraOffset, duration).SetEase(ease).AsyncWaitForCompletion();

            //isTracking = true;
        }

        public void SetCameraPosision(Vector3 target, float orthogonal)
        {
            camera.transform.position = new Vector3(target.x, target.y, -10f);
            camera.orthographicSize = orthogonal;
        }

        public void TrackCharacter(bool isTrack = true)
        {
            isTracking = isTrack;
        }
    }

}
