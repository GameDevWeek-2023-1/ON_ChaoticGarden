using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (PlayerInput))]
[RequireComponent(typeof (PlayerAnimator))]
[RequireComponent(typeof (CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private float plantingResetTime = 1f;
    [SerializeField] private float attackingResetTime = .5f;
    [SerializeField] private float checkRadiusForHiding = 1f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float interactCheckradius;

    private float _playerRadius;
    private bool _isWalking;
    private bool _isPlantingSeed;
    private bool _isAttacking;
    private bool _isHidden;
    private void Awake()
    {
        _playerRadius = GetComponent<CapsuleCollider>().radius;
    }
    private void Start()
    {
        SeedController.Instance.OnSeedPlanted += SeedController_OnSeedPlanted;
        _playerInput.OnAttacked += PlayerInput_OnAttacked;
        _playerInput.OnInteract += PlayerInput_OnInteract;
    }
    private void PlayerInput_OnInteract(object sender, System.EventArgs e)
    {
        Collider[] interactableColliders = Physics.OverlapSphere(transform.position, interactCheckradius);

        foreach (Collider collider in interactableColliders)
        {
            if(collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }
    private void SeedController_OnSeedPlanted(object sender, SeedController.OnSeedPlantedEventArgs e)
    {
        if (!e.canPlant) return;
        
        _isPlantingSeed = true;

        FunctionTimer.Create(() =>
        {
            _isPlantingSeed = false;
        }, plantingResetTime);
    }

    private void PlayerInput_OnAttacked(object sender, System.EventArgs e)
    {
        _isAttacking = true;

        FunctionTimer.Create(() =>
        {
            _isAttacking = false;
        }, attackingResetTime);
    }
    private void Update()
    {
        if (GameStatesController.Instance.GetGamePauseState())
        {
            _isWalking = false;
            return;
        }

        if(_isAttacking)
        {
            return;
        }

        if (_isPlantingSeed)
        {
            _isWalking = false;
            return;
        }

        CheckIsPlayerIsHiding();
        HandleMovement();
    }
    private void CheckIsPlayerIsHiding()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadiusForHiding, layerMask);

        if(colliders.Length != 0)
        {
            foreach (Collider collider in colliders)
            {
                if(collider.TryGetComponent<GrasSeed>(out GrasSeed grasSeed))
                {
                    if(grasSeed.HasGrown())
                    {
                        SetPlayerHideState(true);
                    }
                }
            }
        }
        else
        {
            SetPlayerHideState(false);
        }
    }
    private void HandleMovement()
    {
        Vector2 inputVector = _playerInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerHight = 1f;
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position + (Vector3.up * .3f), transform.position + Vector3.up * playerHight, _playerRadius, moveDir, moveDistance, layerMask, QueryTriggerInteraction.Ignore);

        //Wenn wir nicht bewegen können, weil wir gegen was laufen
        if (!canMove)
        {
            //Only X Movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, _playerRadius, moveDirX, moveDistance, layerMask, QueryTriggerInteraction.Ignore);

            //Can move only on X
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                //Can not move on x than z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHight, _playerRadius, moveDirZ, moveDistance, layerMask, QueryTriggerInteraction.Ignore);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        _isWalking = moveDir != Vector3.zero;

        //transform.forward = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
    }
    public bool IsWalking() => _isWalking;
    public PlayerInput GetPlayerInput() => _playerInput;
    public void SetPlayerHideState(bool hide)
    {
        _isHidden = hide;
    }
    public bool IsPlayerHidden() => _isHidden;
}
