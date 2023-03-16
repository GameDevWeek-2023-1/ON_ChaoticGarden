using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateAnimationEvent : MonoBehaviour
{
    public void SelfDeactivation()
    {
        GameStatesController.Instance.ResetGamePauseState();
        gameObject.SetActive(false);
    }
}
