using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public void PlaceCurrentSeed()
    {
        int currentSeedIndex = SeedController.Instance.GetCurrentSeedIndex();
        switch(currentSeedIndex)
        {
            case 0:
                Instantiate(GameAssets.i.seedGras, transform.parent.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(GameAssets.i.seedMushroom, transform.parent.position, Quaternion.identity);
                break;
        }
    }
}
