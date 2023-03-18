using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatesController : MonoBehaviour
{
    public static GameStatesController Instance { get; private set; }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGamePausedReset;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private HealthSystem playerHealthSystem;
    [SerializeField] private Transform winScreenTransform;
    [SerializeField] private Transform looseScreenTransform;

    private bool _gameIsPaused = true;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    private void Start()
    {
        playerController.GetPlayerInput().OnPauseButtonPressed += PlayerController_PlayerInput_OnPauseButtonPressed;
        playerHealthSystem.OnDied += HealthSystem_Player_OnDied;
        EnemyCounterController.Instance.OnAllEnemiesDead += EnemyCounterController_OnAllEnemiesDead;
    }
    private void EnemyCounterController_OnAllEnemiesDead(object sender, EventArgs e)
    {
        EndGame();
        winScreenTransform.gameObject.SetActive(true);
        SoundEffectsController.Instance.PlayOnShoot(SoundEffectsController.Sound.PlayerWin);
    }
    private void HealthSystem_Player_OnDied(object sender, EventArgs e)
    {
        EndGame();
        looseScreenTransform.gameObject.SetActive(true);
        SoundEffectsController.Instance.PlayOnShoot(SoundEffectsController.Sound.PlayerDied);
    }
    private void PlayerController_PlayerInput_OnPauseButtonPressed(object sender, System.EventArgs e)
    {
        _gameIsPaused = true;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SoundEffectsController.Instance.PlayOnShoot(SoundEffectsController.Sound.ClickMenuPoint);
    }
    public void BackToGame()
    {
        OnGamePausedReset?.Invoke(this, EventArgs.Empty);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
    public void ResetGamePauseState()
    {
        _gameIsPaused = false;
    }
    private void EndGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _gameIsPaused = true;
    }
    public bool GetGamePauseState() => _gameIsPaused;
    
}
