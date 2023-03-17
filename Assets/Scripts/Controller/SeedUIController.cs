using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedUIController : MonoBehaviour
{
    [SerializeField] private Image currentSeedVisual;
    [SerializeField] private Sprite mushroomVisual;
    [SerializeField] private Sprite grasVisual;
    [SerializeField] private TextMeshProUGUI seedAmountText;
    private void Start()
    {
        SeedController.Instance.OnSeedSwitched += SeedController_OnSeedSwitched;
        SeedController.Instance.OnSeedPlanted += SeedController_OnSeedPlanted;
        SeedController.Instance.OnSeedAmountRestocked += SeedController_OnSeedAmountRestocked;
    }
    private void SeedController_OnSeedAmountRestocked(object sender, int e)
    {
        if(e == 0)
        {
            seedAmountText.SetText(SeedController.Instance.GetCurrentGrasSeedAmount().ToString());
        }
        else if(e == 1)
        {
            seedAmountText.SetText(SeedController.Instance.GetCurrentMushroomSeedAmount().ToString());
        }
    }
    private void SeedController_OnSeedPlanted(object sender, SeedController.OnSeedPlantedEventArgs e)
    {
        seedAmountText.SetText(e.currentSeedAmount.ToString());
    }
    private void SeedController_OnSeedSwitched(object sender, SeedController.OnSeedSwitchedEventArgs e)
    {
        if(e.currentSeedIndex == 0)
        {
            currentSeedVisual.sprite = grasVisual;
        }
        else if(e.currentSeedIndex == 1)
        {
            currentSeedVisual.sprite = mushroomVisual;
        }

        seedAmountText.SetText(e.currentSeedAmount.ToString());
    }
}
