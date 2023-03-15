using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedController : MonoBehaviour
{
    public static SeedController Instance { get; private set; }

    public event EventHandler<int> OnSeedSwitched;

    private const int MAX_SEED_INDEX = 1;

    [SerializeField] private PlayerController playerController;

    private int _currentSeedIndex = 0;
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
        playerController.GetPlayerInput().OnSamenSwitched += PlayerInput_OnSamenSwitched;
    }
    private void PlayerInput_OnSamenSwitched(object sender, System.EventArgs e)
    {
        _currentSeedIndex++;
        if(_currentSeedIndex > MAX_SEED_INDEX)
        {
            _currentSeedIndex = 0;
        }
        OnSeedSwitched?.Invoke(this, _currentSeedIndex);
    }
    public int GetCurrentSeedIndex() => _currentSeedIndex;
}
