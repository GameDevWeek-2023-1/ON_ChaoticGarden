using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shild : MonoBehaviour, IInteractable
{
    [SerializeField] private float checkRadius;
    [SerializeField] private Transform interactableSign;
    [SerializeField] private Transform interactableBanner;
    [SerializeField] private LayerMask playerLayerMask;

    private bool _isPlayerNearBy;
    private void Update()
    {
        _isPlayerNearBy = Physics.CheckSphere(transform.position, checkRadius, playerLayerMask);

        if(_isPlayerNearBy)
        {
            interactableSign.gameObject.SetActive(true);
        }
        else
        {
            interactableSign.gameObject.SetActive(false);
            interactableBanner.gameObject.SetActive(false);
        }
    }
    public void Interact()
    {
        if (!_isPlayerNearBy) return;

        interactableBanner.gameObject.SetActive(true);
    }
}
