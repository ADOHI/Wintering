using Cysharp.Threading.Tasks;
using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Titles
{
    public class TitleSceneManager : Singleton<TitleSceneManager>
    {
        public string gameSceneName = "GameScene";

        public async void StartGameAsync()
        {

            await SceneManager.LoadSceneAsync(gameSceneName);
        }
    }

}
