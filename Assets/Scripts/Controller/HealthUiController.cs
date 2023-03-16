using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUiController : MonoBehaviour
{
    [SerializeField] private HealthSystem playerHealthSystem;
    [SerializeField] private Image healthSliderAmountImage;
    private void Start()
    {
        playerHealthSystem.OnDamage += PlayerHealthSystem_OnDamage;
        playerHealthSystem.OnHeal += PlayerHealthSystem_OnHeal;
    }
    private void PlayerHealthSystem_OnHeal(object sender, System.EventArgs e)
    {
        UpdateHealthUI();
    }
    private void PlayerHealthSystem_OnDamage(object sender, System.EventArgs e)
    {
        UpdateHealthUI();
    }
    private void UpdateHealthUI()
    {
        healthSliderAmountImage.fillAmount = playerHealthSystem.GetHealthNormalized();
    }
}
