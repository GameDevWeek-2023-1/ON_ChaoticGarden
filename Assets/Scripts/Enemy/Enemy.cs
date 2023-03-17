using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public static event EventHandler OnAnyEnemySpawned;
    public static event EventHandler OnAnyEnemyDied;

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private float checkRadius;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float hitRadius = 3f;
    [SerializeField] private float attackWaitTime = 2f;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private Animator animator;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Image healthBar;

    private PlayerController _playerController;
    private float _attackWaitTime;
    private void Start()
    {
        navMeshAgent.speed = moveSpeed;
        _attackWaitTime = attackWaitTime;

        _playerController = FindObjectOfType<PlayerController>();
        healthSystem.OnDamage += HealthSystem_OnDamage;
        healthSystem.OnDied += HealthSystem_OnDied;

        OnAnyEnemySpawned?.Invoke(this, EventArgs.Empty);
    }
    private void HealthSystem_OnDied(object sender, EventArgs e)
    {
        OnAnyEnemyDied?.Invoke(this, EventArgs.Empty);

        int randomChanceForSeedBag = UnityEngine.Random.Range(0, 101);
        if(randomChanceForSeedBag <= 30)
        {
            Instantiate(GameAssets.i.seedBag, transform.position, Quaternion.identity);
        }
    }
    private void HealthSystem_OnDamage(object sender, System.EventArgs e)
    {
        healthBar.fillAmount = healthSystem.GetHealthNormalized();
    }
    private void Update()
    {
        if(GameStatesController.Instance.GetGamePauseState())
        {
            navMeshAgent.SetDestination(transform.position);
            animator.SetBool("noTarget", false);
            return;
        }

        if (_playerController.IsPlayerHidden())
        {
            navMeshAgent.SetDestination(transform.position);
            animator.SetBool("noTarget", false);
            return;
        }

        if(Physics.CheckSphere(transform.position, checkRadius, playerLayerMask))
        {
            navMeshAgent.SetDestination(_playerController.transform.position);
            animator.SetBool("noTarget", true);

            if (Vector3.Distance(transform.position, _playerController.transform.position) < hitRadius)
            {
                if(_attackWaitTime <= 0f)
                {
                    _playerController.GetComponent<HealthSystem>().TakeDamage();
                    _attackWaitTime = attackWaitTime;
                }
                else
                {
                    _attackWaitTime -= Time.deltaTime;
                }
            }
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
            animator.SetBool("noTarget", false);
        }
    }
}
