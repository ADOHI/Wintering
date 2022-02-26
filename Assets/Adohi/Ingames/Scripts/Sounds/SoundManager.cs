using DG.Tweening;
using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingames
{
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioSource backgroundMusic;
        public float bgmFadeDuration = 3f;

        public void PlayBGM()
        {
            backgroundMusic.volume = 0f;
            backgroundMusic.Play();
            Fade(backgroundMusic, 1f, bgmFadeDuration);
        }



        public void Fade(AudioSource audioSource, float fadeValue, float fadeDuration)
        {
            audioSource.DOFade(fadeValue, fadeDuration);
        }
    }

}
