using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedController : MonoBehaviour
{
    //AnimationEvent.cs dort PlaceSeed Event zum Planten
    public static SeedController Instance { get; private set; }

    public event EventHandler<OnSeedSwitchedEventArgs> OnSeedSwitched;
    public event EventHandler<OnSeedPlantedEventArgs> OnSeedPlanted;
    public event EventHandler<int> OnSeedAmountRestocked;
    public class OnSeedSwitchedEventArgs : EventArgs
    {
        public int currentSeedIndex;
        public int currentSeedAmount;
    }
    public class OnSeedPlantedEventArgs : EventArgs
    {
        public int currentSeedAmount;
        public bool canPlant;
    }

    private const int MAX_SEED_INDEX = 1;

    [SerializeField] private PlayerController playerController;

    private int _currentSeedIndex = 0;
    private int _mushroomSeedAmount = 10;
    private int _grasSeedAmount = 10;
    private int currentSeedAmountForSwitch;
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
        playerController.GetPlayerInput().OnSamenPlanted += PlayerInput_OnSamenPlanted;
    }
    private void PlayerInput_OnSamenPlanted(object sender, EventArgs e)
    {
        if(_currentSeedIndex == 0)
        {
            _grasSeedAmount--;
            if (_grasSeedAmount < 0)
            {
                _grasSeedAmount = 0;
                return;
            }
            OnSeedPlanted?.Invoke(this, new OnSeedPlantedEventArgs {currentSeedAmount = _grasSeedAmount, canPlant = CanPlaceGras()});
        }
        else if(_currentSeedIndex == 1)
        {
            _mushroomSeedAmount--;
            if (_mushroomSeedAmount < 0)
            {
                _mushroomSeedAmount = 0;
                return;
            }
            OnSeedPlanted?.Invoke(this, new OnSeedPlantedEventArgs {currentSeedAmount = _mushroomSeedAmount, canPlant = CanPlaceMushroom()});
        }
    }
    private void PlayerInput_OnSamenSwitched(object sender, System.EventArgs e)
    {
        _currentSeedIndex++;
        if(_currentSeedIndex > MAX_SEED_INDEX)
        {
            _currentSeedIndex = 0;
        }
 
        if(_currentSeedIndex == 0)
        {
            currentSeedAmountForSwitch = _grasSeedAmount;
        }
        else if(_currentSeedIndex == 1)
        {
            currentSeedAmountForSwitch = _mushroomSeedAmount;
        }

        OnSeedSwitched?.Invoke(this, new OnSeedSwitchedEventArgs { currentSeedIndex = _currentSeedIndex, currentSeedAmount = currentSeedAmountForSwitch});
    }
    public void RestockSeeds()
    {
        _grasSeedAmount += UnityEngine.Random.Range(1, 10);
        _grasSeedAmount = Mathf.Clamp(_grasSeedAmount, 0, 10);

        _mushroomSeedAmount += UnityEngine.Random.Range(1, 10);
        _mushroomSeedAmount = Mathf.Clamp(_mushroomSeedAmount, 0, 10);

        OnSeedAmountRestocked?.Invoke(this, _currentSeedIndex);
    }
    public int GetCurrentSeedIndex() => _currentSeedIndex;
    public bool CanPlaceMushroom() => _mushroomSeedAmount >= 0;
    public bool CanPlaceGras() => _grasSeedAmount >= 0;
    public int GetCurrentMushroomSeedAmount() => _mushroomSeedAmount;
    public int GetCurrentGrasSeedAmount() => _grasSeedAmount;
}
