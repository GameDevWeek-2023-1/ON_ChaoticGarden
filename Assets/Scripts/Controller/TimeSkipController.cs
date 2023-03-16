using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSkipController : MonoBehaviour
{
    private const float TIME_SKIP_TIME_MULTIPLIER = 1.2f;
    public static TimeSkipController Instance { get; private set; }

    public event EventHandler OnTimeSkipped;

    [SerializeField] private Image timeSkipOverlayImage;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float timeSkipResetTime;

    private bool _canTimeSkip = true;
    private int _timeSkipCounter;
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
        playerController.GetPlayerInput().OnWeatherForward += PlayerController_PlayerInput_OnWeatherForward;
        timeSkipOverlayImage.gameObject.SetActive(false);
    }
    private void PlayerController_PlayerInput_OnWeatherForward(object sender, System.EventArgs e)
    {
        if (!_canTimeSkip) return;

        _canTimeSkip = false;
        _timeSkipCounter++;
        timeSkipOverlayImage.gameObject.SetActive(true);
        OnTimeSkipped?.Invoke(this, EventArgs.Empty);

        //Je öffter man ihn benutzt, desto größerer Cooldown
        if(_timeSkipCounter >= 3)
        {
            timeSkipResetTime = (timeSkipResetTime * TIME_SKIP_TIME_MULTIPLIER);
        }

        FunctionTimer.Create(() =>
        {
            _canTimeSkip = true;
            timeSkipOverlayImage.gameObject.SetActive(false);
        }, timeSkipResetTime);
    }
}
