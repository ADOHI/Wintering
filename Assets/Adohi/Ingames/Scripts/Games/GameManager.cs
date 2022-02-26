using Cysharp.Threading.Tasks;
using Ingames.Maps;
using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ingames
{
    public class GameManager : Singleton<GameManager>
    {
        public VoidBaseEventReference OnGamePrepared;
        public VoidBaseEventReference OnGameEnd;
        public string titleSceneName = "TitleScene";




        private async void Start()
        {
            await GameStartAsync();
        }

        public async UniTask GameStartAsync()
        {
            await PrepareGame();
            //IngameProgressManager.Instance.DayTimeAsync().AttachExternalCancellation(this.GetCancellationTokenOnDestroy()).Forget();
        }

        public async UniTask PrepareGame()
        {
            MapManager.Instance.Spawn();
            Debug.Log("Game is prepared");
            await UniTask.Delay(2000);
            OnGamePrepared?.Event?.Raise();
        }

        
        public async UniTask EndGameAsync()
        {
            OnGameEnd.Event.Raise();
            await SceneManager.LoadSceneAsync(titleSceneName);
        }



    }

}
