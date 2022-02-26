using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Ingame
{
    public class SnowController : MonoBehaviour
    {
        private ParticleSystem particleSystem;

        public GameObjectReference character;
        public Vector3 offset;
        public float lerpSpeed = 5f;
        private void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();
            particleSystem.Stop();
        }

        public void SetPlay(bool isPlay)
        {
            if (isPlay)
            {
                particleSystem.Play();
            }
            else
            {
                particleSystem.Stop();
            }
        }


        private void LateUpdate()
        {
            var shape = particleSystem.shape;

            var targetPosition = Vector3.zero;

            if (character.Value != null)
            {
                targetPosition = character.Value.transform.position + offset;
            }

            shape.position = Vector3.Lerp(shape.position, character.Value.transform.position + offset, Time.deltaTime * lerpSpeed);
        }

    }

}
