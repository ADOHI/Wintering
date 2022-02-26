using DG.Tweening;
using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ingames
{
    public class SoundManager : Singleton<SoundManager>
    {
        public AudioSource idleBackgroundMusic;
        public AudioSource dieBackgroundMusic;
        public float bgmFadeDuration = 3f;

        public void PlayBGM()
        {
            idleBackgroundMusic.volume = 0f;
            idleBackgroundMusic.Play();
            Fade(idleBackgroundMusic, 1f, bgmFadeDuration);
        }

        public void StopBGM()
        {
            //idleBackgroundMusic.volume = 0f;
            //idleBackgroundMusic.Play();
            Fade(idleBackgroundMusic, 0f, bgmFadeDuration);
        }

        public void PlayDieBGM()
        {
            dieBackgroundMusic.volume = 0f;
            dieBackgroundMusic.Play();
            Fade(dieBackgroundMusic, 1f, bgmFadeDuration);
        }

        public void StopDieBGM()
        {
            //dieBackgroundMusic.volume = 0f;
            //dieBackgroundMusic.Play();
            Fade(dieBackgroundMusic, 0f, bgmFadeDuration);
        }


        public void Fade(AudioSource audioSource, float fadeValue, float fadeDuration)
        {
            audioSource.DOFade(fadeValue, fadeDuration);
        }
    }

}
