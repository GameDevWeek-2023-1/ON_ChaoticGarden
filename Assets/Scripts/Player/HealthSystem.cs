using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDamage;
    public event EventHandler OnHeal;
    public event EventHandler OnDied;

    [SerializeField] private float maxHealth;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor;
    [SerializeField] private Color defaultColor;

    private float _health;
    private void Start()
    {
        _health = maxHealth;
    }
    public void TakeDamage()
    {
        _health -= 10;
        _health = Mathf.Clamp(_health, 0, maxHealth);
        OnDamage?.Invoke(this, EventArgs.Empty);

        spriteRenderer.color = hitColor;

        float delayTime = .1f;
        FunctionTimer.Create(() =>
        {
            if (this == null) return;

            spriteRenderer.color = defaultColor;
        }, delayTime);

        if(_health <= 0)
        {
            OnDied?.Invoke(this, EventArgs.Empty);
            Destroy(gameObject);
        }
    }
    public void HealUp()
    {
        _health = maxHealth;
        OnHeal?.Invoke(this, EventArgs.Empty);
    }
    public float GetHealthNormalized() => _health / maxHealth;
    public float GetCurrentHealth() => _health;
}
