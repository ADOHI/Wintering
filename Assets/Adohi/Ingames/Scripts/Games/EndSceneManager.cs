using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneManager : MonoBehaviour
{
    public string gameScene = "GameScene";
    //public float duration;
    public VoidBaseEventReference onLoadScene;
    public VoidBaseEventReference onLoadedScene;

    // Start is called before the first frame update
    void Start()
    {
        onLoadedScene?.Event?.Register(LoadNextScene);
        onLoadScene?.Event?.Raise();
    }

    async void LoadNextScene()
    {
        SceneManager.LoadScene(gameScene);
    }
}
