using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherUIController : MonoBehaviour
{
    [SerializeField] private Image[] weatherHolderImages;
    [SerializeField] private Image[] currentWeatherImages;
    [SerializeField] private Sprite sunnyWeatherImage;
    [SerializeField] private Sprite rainyWeatherImage;
    [SerializeField] private Sprite stormyWeatherImage;
    [SerializeField] private Sprite normalWeatherImage;
    private void Start()
    {
        WeatherController.Instance.OnWeatherChanged += WeatherController_OnWeatherChanged;
        WeatherController.Instance.OnCurrentWeatherChanged += WeatherController_OnCurrentWeatherChanged;

        UpdateWeatherUiHolder(WeatherController.Instance.GetWeatherList());
        UpdateCurrentWeatherImage();
    }
    private void WeatherController_OnCurrentWeatherChanged(object sender, System.EventArgs e)
    {
        UpdateCurrentWeatherImage();
    }
    private void WeatherController_OnWeatherChanged(object sender, List<WeatherController.Weather> e)
    {
        UpdateWeatherUiHolder(e);
    }
    private void UpdateWeatherUiHolder(List<WeatherController.Weather> weatherList)
    {
        for (int i = 0; i < weatherList.Count; i++)
        {
            switch(weatherList[i])
            {
                case WeatherController.Weather.Normal:
                    ChangeWeatherSprite(normalWeatherImage, i);
                    break;
                case WeatherController.Weather.Stormy:
                    ChangeWeatherSprite(stormyWeatherImage, i);
                    break;
                case WeatherController.Weather.Sunny:
                    ChangeWeatherSprite(sunnyWeatherImage, i);
                    break;
                case WeatherController.Weather.Rainy:
                    ChangeWeatherSprite(rainyWeatherImage, i);
                    break;
            }
        }
    }
    private void ChangeWeatherSprite(Sprite weatherSprite, int weatherHolderImageIndex)
    {
        weatherHolderImages[weatherHolderImageIndex].sprite = weatherSprite;
    }
    private void UpdateCurrentWeatherImage()
    {
        int currentWeatherIndex = WeatherController.Instance.GetCurrentWeatherIndex() - 1;
        Debug.Log(currentWeatherIndex);

        foreach (Image currentWeatherImage in currentWeatherImages)
        {
            currentWeatherImage.gameObject.SetActive(false);
        }

        currentWeatherImages[currentWeatherIndex].gameObject.SetActive(true);
    }
}
