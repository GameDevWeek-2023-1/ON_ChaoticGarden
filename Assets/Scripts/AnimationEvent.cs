using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public void PlaceCurrentSeed()
    {
        int currentSeedIndex = SeedController.Instance.GetCurrentSeedIndex();

        float spawnOffsetAmount = -1f;
        switch(currentSeedIndex)
        {
            case 0:
                Instantiate(GameAssets.i.seedGras, transform.parent.position + (Vector3.forward * spawnOffsetAmount), Quaternion.identity);
                break;
            case 1:
                Instantiate(GameAssets.i.seedMushroom, transform.parent.position + (Vector3.forward * spawnOffsetAmount), Quaternion.identity);
                break;
        }
    }
    public void SwitchToGameScene()
    {
        SceneLoaderController.Load(SceneLoaderController.Scenes.GameScene);
    }
    public void SelfDeactivation()
    {
        gameObject.SetActive(false);
        GameStatesController.Instance.ResetGamePauseState();
    }
    public void RestartGameScene()
    {
        SceneLoaderController.Load(SceneLoaderController.Scenes.GameScene);
    }
    public void LoadToHomeScene()
    {
        SceneLoaderController.Load(SceneLoaderController.Scenes.HomeScene);
    }
    public void SelfDeactivationHomeMenu()
    {
        gameObject.SetActive(false);
    }
}
