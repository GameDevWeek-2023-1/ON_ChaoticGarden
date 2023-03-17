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
        _player.GetPlayerInput().OnAttacked += PlayerController_PlayerInput_OnAttacked;
        SeedController.Instance.OnSeedPlanted += SeedController_OnSeedPlanted; ;
    }
    private void SeedController_OnSeedPlanted(object sender, SeedController.OnSeedPlantedEventArgs e)
    {
        if (!e.canPlant) return;

        _playerAnimator.SetTrigger("PlaceSeed");
    }
    private void PlayerController_PlayerInput_OnAttacked(object sender, System.EventArgs e)
    {
        _playerAnimator.SetTrigger("Attack");
    }
    private void Update()
    {
        if (GameStatesController.Instance.GetGamePauseState()) return;

        _playerAnimator.SetBool(IS_WALKING, _player.IsWalking());

        _playerAnimator.SetFloat("X", _player.GetPlayerInput().GetMovementVectorNormalized().x);
        _playerAnimator.SetFloat("Y", _player.GetPlayerInput().GetMovementVectorNormalized().y);
    }
}
