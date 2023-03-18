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
    [SerializeField] private ParticleSystem stormyWeatherWindParticleSystem;
    [SerializeField] private List<Weather> weatherList = new List<Weather>();
    [SerializeField] private MMF_Player Feel_Feedback_Lensdistortion;
    [SerializeField] private Light sunLight;
    [SerializeField] private float minSunLight;
    [SerializeField] private float maxSunLight;

    public Weather _currentWeather;
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

        rainyWeatherParticleSystem.Stop();
        stormyWeatherParticleSystem.Stop();
        stormyWeatherWindParticleSystem.Stop();
    }
    private void TimeSkipController_OnWeatherForward(object sender, EventArgs e)
    {
        if(_currentWeatherIndex > weatherList.Count - 1) return;

        _changeWeatherTime = 0f;
        Feel_Feedback_Lensdistortion?.PlayFeedbacks();
    }
    private void Update()
    {
        if (GameStatesController.Instance.GetGamePauseState()) return;

        if (_changeWeatherTime <= 0f)
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
            //FunctionTimer.Create(() =>
            //{
            //    OnTimeBeforeWeatherChange?.Invoke(this, EventArgs.Empty);

            //    FunctionTimer.Create(() =>
            //    {
            //        SetRandomWeather();
            //        _currentWeatherIndex = 0;
            //        OnWeatherChanged?.Invoke(this, weatherList);
            //    }, resetDelayTime);
            //}, _backUpChangeWeatherTime);

            FunctionTimer.Create(() =>
            {
                if (!GameStatesController.Instance.GetGamePauseState())
                {
                    _backUpChangeWeatherTime -= Time.deltaTime;
                }

                if(_backUpChangeWeatherTime <= 0f)
                {
                    FunctionTimer.Create(() =>
                        {
                            SetRandomWeather();
                            _currentWeatherIndex = 0;
                            OnWeatherChanged?.Invoke(this, weatherList);
                        }, resetDelayTime);

                    OnTimeBeforeWeatherChange?.Invoke(this, EventArgs.Empty);
                    _backUpChangeWeatherTime = 0;
                }

                return _backUpChangeWeatherTime == 0;
            }, "", true);
        }

        switch (_currentWeather)
        {
            case Weather.Normal:
                rainyWeatherParticleSystem.Stop();
                stormyWeatherParticleSystem.Stop();
                stormyWeatherWindParticleSystem.Stop();

                FunctionTimer.Create(() =>
                {
                    sunLight.intensity -= Time.deltaTime;

                    if (sunLight.intensity <= minSunLight)
                    {
                        sunLight.intensity = minSunLight;
                    }

                    return sunLight.intensity == minSunLight;
                }, "", true);
                break;
            case Weather.Rainy:
                rainyWeatherParticleSystem.Play();
                stormyWeatherParticleSystem.Stop();
                stormyWeatherWindParticleSystem.Stop();

                FunctionTimer.Create(() =>
                {
                    sunLight.intensity -= Time.deltaTime;

                    if (sunLight.intensity <= minSunLight)
                    {
                        sunLight.intensity = minSunLight;
                    }

                    return sunLight.intensity == minSunLight;
                }, "", true);
                break;
            case Weather.Stormy:
                rainyWeatherParticleSystem.Stop();
                stormyWeatherParticleSystem.Play();
                stormyWeatherWindParticleSystem.Play();

                FunctionTimer.Create(() =>
                {
                    sunLight.intensity -= Time.deltaTime;

                    if (sunLight.intensity <= minSunLight)
                    {
                        sunLight.intensity = minSunLight;
                    }

                    return sunLight.intensity == minSunLight;
                }, "", true);
                break;
            case Weather.Sunny:
                rainyWeatherParticleSystem.Stop();
                stormyWeatherParticleSystem.Stop();
                stormyWeatherWindParticleSystem.Stop();

                FunctionTimer.Create(() =>
                {
                    sunLight.intensity += Time.deltaTime;

                    if(sunLight.intensity >= maxSunLight)
                    {
                        sunLight.intensity = maxSunLight;
                    }

                    return sunLight.intensity == maxSunLight;
                }, "", true);
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
    public Weather GetCurrentWeatherType() => _currentWeather;
}
