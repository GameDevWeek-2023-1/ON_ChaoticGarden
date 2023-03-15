using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderController : MonoBehaviour
{
    //Dummy class
    private class LoadingMonoBehaviour : MonoBehaviour { }
    public enum Scenes
    {
        HomeScene,
        GameScene,
        LoadingScene,
        OptionsScene,
    }
    private static Action _onLoaderCallBack;
    public static void Load(Scenes scene)
    {
        _onLoaderCallBack = () =>
        {
            GameObject loadingObject = new GameObject("loading Game Object");
            loadingObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };

        SceneManager.LoadScene(Scenes.LoadingScene.ToString());
    }
    private static IEnumerator LoadSceneAsync(Scenes scene)
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
    public static void InstantLoad(Scenes scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
    public static void LoaderCallBack()
    {
        if (_onLoaderCallBack != null)
        {
            _onLoaderCallBack();
            _onLoaderCallBack = null;
        }
    }
}
