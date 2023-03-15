using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    public enum Weather
    {
        Normal,
        Stormy,
        Rainy,
        Sunny,
    }
    [SerializeField] private float maxChangeWeatherTime;
    [SerializeField] private int weatherTypeCount;
    [SerializeField] private ParticleSystem normalWeatherParticleSystem;
    [SerializeField] private ParticleSystem rainyWeatherParticleSystem;
    [SerializeField] private ParticleSystem stormyWeatherParticleSystem;

    public List<Weather> weatherList = new List<Weather>();
    private Weather _currentWeather;
    private float _changeWeatherTime;
    private int _currentWeatherIndex = 0;
    private void Start()
    {
        SetRandomWeather();
    }
    private void Update()
    {
        if(_changeWeatherTime <= 0f)
        {
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

        _changeWeatherTime = maxChangeWeatherTime;
        _currentWeatherIndex++;

        if(_currentWeatherIndex > weatherList.Count - 1)
        {
            SetRandomWeather();
            _currentWeatherIndex = 0;
        }

        switch(_currentWeather)
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
            int randomWeather = Random.Range(0, 4);
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
}
