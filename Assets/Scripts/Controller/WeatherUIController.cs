using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherUIController : MonoBehaviour
{
    [SerializeField] private RectTransform weatherUIHolder;
    [SerializeField] private RectTransform sunnyWeatherImage;
    [SerializeField] private RectTransform rainyWeatherImage;
    [SerializeField] private RectTransform stormyWeatherImage;
    [SerializeField] private RectTransform normalWeatherImage;
    private void Start()
    {
        WeatherController.Instance.OnWeatherChanged += WeatherController_OnWeatherChanged;
        ResetWeatherUiHolder(WeatherController.Instance.GetWeatherList());
    }
    private void WeatherController_OnWeatherChanged(object sender, List<WeatherController.Weather> e)
    {
        ResetWeatherUiHolder(e);
    }
    private void ResetWeatherUiHolder(List<WeatherController.Weather> weatherList)
    {
        foreach (RectTransform weatherTransform in weatherUIHolder)
        {
            Destroy(weatherTransform.gameObject);
        }

        foreach(WeatherController.Weather weather in weatherList)
        {
            switch(weather)
            {
                case WeatherController.Weather.Normal:
                    SpawnUIWeather(normalWeatherImage);
                    break;
                case WeatherController.Weather.Sunny:
                    SpawnUIWeather(sunnyWeatherImage);
                    break;
                case WeatherController.Weather.Rainy:
                    SpawnUIWeather(rainyWeatherImage);
                    break;
                case WeatherController.Weather.Stormy:
                    SpawnUIWeather(stormyWeatherImage);
                    break;
            }
        }
    }
    private void SpawnUIWeather(RectTransform spawnedTransform)
    {
        RectTransform uiObject = Instantiate(spawnedTransform, weatherUIHolder);
        uiObject.SetParent(weatherUIHolder);
    }
}
