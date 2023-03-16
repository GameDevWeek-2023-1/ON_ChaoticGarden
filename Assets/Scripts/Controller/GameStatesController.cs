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
    }
    private void PlayerController_PlayerInput_OnPauseButtonPressed(object sender, System.EventArgs e)
    {
        _gameIsPaused = true;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
    public bool GetGamePauseState() => _gameIsPaused;
}
