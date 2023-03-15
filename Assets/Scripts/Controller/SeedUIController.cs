using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedUIController : MonoBehaviour
{
    [SerializeField] private Image image;
    private void Start()
    {
        SeedController.Instance.OnSeedSwitched += SeedController_OnSeedSwitched;
    }
    private void SeedController_OnSeedSwitched(object sender, int e)
    {
        if(e == 0)
        {
            image.color = Color.green;
        }
        else if(e == 1)
        {
            image.color = Color.red;
        }
    }
}
