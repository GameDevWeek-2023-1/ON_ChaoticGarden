using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public static WeatherController Instance { get; private set; }
    public event EventHandler OnCurrentWeatherChanged;
    public event EventHandler<List<Weather>> OnWeatherChanged;
    public event EventHandler OnTimeBeforeWeatherChange;
    public enum Weather
    {
        Normal,
        Stormy,
        Rainy,
        Sunny,
    }

    [SerializeField] private float maxChangeWeatherTime;
    [SerializeField] private float minChangeWeatherTime;
    [SerializeField] private float resetDelayTime;
    [SerializeField] private int weatherTypeCount;
    [SerializeField] private ParticleSystem normalWeatherParticleSystem;
    [SerializeField] private ParticleSystem rainyWeatherParticleSystem;
    [SerializeField] private ParticleSystem stormyWeatherParticleSystem;
    [SerializeField] private List<Weather> weatherList = new List<Weather>();
    [SerializeField] private MMF_Player Feel_Feedback_Lensdistortion;

    private Weather _currentWeather;
    private float _changeWeatherTime;
    private float _backUpChangeWeatherTime;
    private int _currentWeatherIndex = 0;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SetRandomWeather();
    }
    private void Start()
    {
        TimeSkipController.Instance.OnTimeSkipped += TimeSkipController_OnWeatherForward;
    }
    private void TimeSkipController_OnWeatherForward(object sender, EventArgs e)
    {
        if(_currentWeatherIndex > weatherList.Count - 1) return;

        _changeWeatherTime = 0f;
        Feel_Feedback_Lensdistortion?.PlayFeedbacks();
    }
    private void Update()
    {
        if(_changeWeatherTime <= 0f)
        {
            if (_currentWeatherIndex > weatherList.Count - 1) return;

            OnCurrentWeatherChanged?.Invoke(this, EventArgs.Empty);
            ChangeWeather();
        }
        else
        {
            _changeWeatherTime -= Time.deltaTime;
        }
    }
    private void ChangeWeather()
    {
        _currentWeather = weatherList[_currentWeatherIndex];

        _changeWeatherTime = UnityEngine.Random.Range(minChangeWeatherTime, maxChangeWeatherTime);
        _backUpChangeWeatherTime = _changeWeatherTime;

        _currentWeatherIndex++;

        if (_currentWeatherIndex > weatherList.Count - 1)
        {
            FunctionTimer.Create(() =>
            {
                OnTimeBeforeWeatherChange?.Invoke(this, EventArgs.Empty);

                FunctionTimer.Create(() =>
                {
                    SetRandomWeather();
                    _currentWeatherIndex = 0;
                    OnWeatherChanged?.Invoke(this, weatherList);
                }, resetDelayTime);
            }, _backUpChangeWeatherTime);
        }

        switch (_currentWeather)
        {
            case Weather.Normal:
                _currentWeather = Weather.Rainy;
                normalWeatherParticleSystem.Play();
                rainyWeatherParticleSystem.Stop();
                stormyWeatherParticleSystem.Stop();
                break;
            case Weather.Rainy:
                _currentWeather = Weather.Stormy;
                normalWeatherParticleSystem.Stop();
                rainyWeatherParticleSystem.Play();
                stormyWeatherParticleSystem.Stop();
                break;
            case Weather.Stormy:
                _currentWeather = Weather.Sunny;
                normalWeatherParticleSystem.Stop();
                rainyWeatherParticleSystem.Stop();
                stormyWeatherParticleSystem.Play();
                break;
            case Weather.Sunny:
                _currentWeather = Weather.Normal;
                normalWeatherParticleSystem.Stop();
                rainyWeatherParticleSystem.Stop();
                stormyWeatherParticleSystem.Stop();
                break;
        }
    }

    private void SetRandomWeather()
    {
        for (int i = 0; i < weatherTypeCount; i++)
        {
            int randomWeather = UnityEngine.Random.Range(0, 4);
            switch (randomWeather)
            {
                case 0:
                    weatherList[i] = Weather.Normal;
                    break;
                case 1:
                    weatherList[i] = Weather.Rainy;
                    break;
                case 2:
                    weatherList[i] = Weather.Stormy;
                    break;
                case 3:
                    weatherList[i] = Weather.Sunny;
                    break;
            }
        }
    }
    public List<Weather> GetWeatherList() => weatherList;
    public int GetCurrentWeatherIndex() => _currentWeatherIndex;
}
