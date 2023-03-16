using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUIController : MonoBehaviour
{
    [SerializeField] private Transform pauseMenuUITransform;
    [SerializeField] private Transform restartGameSceneTranstion;
    [SerializeField] private Transform loadHomeSceneTransition;
    [SerializeField] private Animator pauseMenuUIAnimator;
    private void Start()
    {
        GameStatesController.Instance.OnGamePaused += GameStatesController_OnGamePaused;
        GameStatesController.Instance.OnGamePausedReset += GameStatesController_OnGamePausedReset;
    }
    private void GameStatesController_OnGamePausedReset(object sender, System.EventArgs e)
    {
        pauseMenuUIAnimator.SetBool("deactivate", true);
    }
    private void GameStatesController_OnGamePaused(object sender, System.EventArgs e)
    {
        pauseMenuUITransform.gameObject.SetActive(true);
    }
    public void RestartGameScene()
    {
        restartGameSceneTranstion.gameObject.SetActive(true);
    }
    public void LoadToHomeScene()
    {
        loadHomeSceneTransition.gameObject.SetActive(true);
    }
}
