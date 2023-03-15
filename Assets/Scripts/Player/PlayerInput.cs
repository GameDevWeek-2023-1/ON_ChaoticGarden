using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event EventHandler OnSamenPlanted;
    public event EventHandler OnSamenSwitched;

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
        _playerActionInput.Player.SeedsSwitch.performed += PlayerInput_SeedsSwitch_performed;
        _playerActionInput.Player.Attack.performed += PlayerInput_Attack_performed;
    }
    private void PlayerInput_Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Attack");
    }
    private void PlayerInput_SeedsSwitch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //Scroll Wheel => Value & Any => Scroll/Y [Mouse]
        float y = obj.ReadValue<float>();
        if(y > 0)
        {
            OnSamenSwitched?.Invoke(this, EventArgs.Empty);
        }
    }
    private void PlayerInput_Planting_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSamenPlanted?.Invoke(this, EventArgs.Empty);
    }
    public Vector2 GetMovementVectorNormalized() => _playerActionInput.Player.Move.ReadValue<Vector2>().normalized;
}