using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMenuController : MonoBehaviour
{
    public void LoadToGameScene()
    {
        SceneLoaderController.Load(SceneLoaderController.Scenes.GameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
