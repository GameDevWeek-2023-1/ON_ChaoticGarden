using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCounterController : MonoBehaviour
{
    public static EnemyCounterController Instance { get; private set; }

    public event EventHandler OnAllEnemiesDead;

    [SerializeField] private TextMeshProUGUI counterText;

    private int _enemyCounter;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        Enemy.OnAnyEnemySpawned += Enemy_OnAnyEnemySpawned;
        Enemy.OnAnyEnemyDied += Enemy_OnAnyEnemyDied;
    }
    private void Enemy_OnAnyEnemyDied(object sender, System.EventArgs e)
    {
        _enemyCounter--;
        UpdateCounterText();

        if(_enemyCounter <= 0)
        {
            OnAllEnemiesDead?.Invoke(this, EventArgs.Empty);
        }
    }
    private void Enemy_OnAnyEnemySpawned(object sender, System.EventArgs e)
    {
        _enemyCounter++;
        UpdateCounterText();
    }
    private void UpdateCounterText()
    {
        counterText.SetText(_enemyCounter.ToString());
    }
}
