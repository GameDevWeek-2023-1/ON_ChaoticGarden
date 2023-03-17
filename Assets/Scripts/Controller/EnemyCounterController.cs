using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCounterController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;

    private int _enemyCounter;
    private void Start()
    {
        Enemy.OnAnyEnemySpawned += Enemy_OnAnyEnemySpawned;
        Enemy.OnAnyEnemyDied += Enemy_OnAnyEnemyDied;
    }
    private void Enemy_OnAnyEnemyDied(object sender, System.EventArgs e)
    {
        _enemyCounter--;
        UpdateCounterText();
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
