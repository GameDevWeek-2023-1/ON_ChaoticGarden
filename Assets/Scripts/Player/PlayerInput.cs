using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event EventHandler OnSamenPlanted;
    public event EventHandler OnSamenSwitched;
    public event EventHandler OnWeatherForward;
    public event EventHandler OnPauseButtonPressed;

    private PlayerActionInput _playerActionInput;
    private void OnEnable()
    {
        _playerActionInput = new PlayerActionInput();
        _playerActionInput.Player.Enable();
    }
    private void OnDisable()
    {
        _playerActionInput.Player.Disable();
    }
    private void Start()
    {
        _playerActionInput.Player.Planting.performed += PlayerInput_Planting_performed;
        _playerActionInput.Player.SeedsSwitchPC.performed += PlayerInput_SeedsSwitchPC_performed;
        _playerActionInput.Player.SeedsSwitchGamePad.performed += PlayerInput_SeedsSwitchGamePad_performed;
        _playerActionInput.Player.Attack.performed += PlayerInput_Attack_performed;
        _playerActionInput.Player.ForwardWeather.performed += PlayerInput_ForwardWeather_performed;
        _playerActionInput.Player.PauseMenu.performed += PlayerInput_PauseMenu_performed;
    }
    private void PlayerInput_PauseMenu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseButtonPressed?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerInput_ForwardWeather_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (GameStatesController.Instance.GetGamePauseState()) return;

        OnWeatherForward?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerInput_Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (GameStatesController.Instance.GetGamePauseState()) return;

        Debug.Log("Attack");
    }
    private void PlayerInput_SeedsSwitchPC_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (GameStatesController.Instance.GetGamePauseState()) return;

        //Scroll Wheel => Value & Any => Scroll/Y [Mouse]
        float y = obj.ReadValue<float>();
        if(y > 0)
        {
            OnSamenSwitched?.Invoke(this, EventArgs.Empty);
        }
    }
    private void PlayerInput_SeedsSwitchGamePad_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (GameStatesController.Instance.GetGamePauseState()) return;

        OnSamenSwitched?.Invoke(this, EventArgs.Empty);
    }
    private void PlayerInput_Planting_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (GameStatesController.Instance.GetGamePauseState()) return;

        OnSamenPlanted?.Invoke(this, EventArgs.Empty);
    }
    public Vector2 GetMovementVectorNormalized() => _playerActionInput.Player.Move.ReadValue<Vector2>().normalized;
}
