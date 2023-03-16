using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMenuController : MonoBehaviour
{
    [SerializeField] private Transform transitionTransform;
    public void LoadToGameScene()
    {
        transitionTransform.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
