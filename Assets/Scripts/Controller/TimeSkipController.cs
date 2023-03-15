using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSkipController : MonoBehaviour
{
    public static TimeSkipController Instance { get; private set; }

    public event EventHandler OnTimeSkipped;

    [SerializeField] private Image timeSkipOverlayImage;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float timeSkipResetTime;

    private bool _canTimeSkip = true;
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
        timeSkipOverlayImage.gameObject.SetActive(true);
        OnTimeSkipped?.Invoke(this, EventArgs.Empty);
        FunctionTimer.Create(() =>
        {
            _canTimeSkip = true;
            timeSkipOverlayImage.gameObject.SetActive(false);
        }, timeSkipResetTime);
    }
}
