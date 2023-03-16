using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Animator _playerAnimator;

    private PlayerController _player;
    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }
    private void Start()
    {
        _player.GetPlayerInput().OnSamenPlanted += PlayerController_PlayerInput_OnSamenPlanted;
    }
    private void PlayerController_PlayerInput_OnSamenPlanted(object sender, System.EventArgs e)
    {
        _playerAnimator.SetTrigger("PlaceSeed");
    }
    private void Update()
    {
        if (GameStatesController.Instance.GetGamePauseState()) return;

        _playerAnimator.SetBool(IS_WALKING, _player.IsWalking());

        _playerAnimator.SetFloat("X", _player.GetPlayerInput().GetMovementVectorNormalized().x);
        _playerAnimator.SetFloat("Y", _player.GetPlayerInput().GetMovementVectorNormalized().y);
    }
}
