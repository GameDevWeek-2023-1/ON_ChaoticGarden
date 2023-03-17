using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSeed : MonoBehaviour
{
    [SerializeField] private float maxSize;
    [SerializeField] private float maxTimeToGrow;
    [SerializeField] private float minTimeToGrow;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform upgradeParticle;

    private float _growTime;
    private bool _hasGrown;
    private bool _isDead = false;
    private void Start()
    {
        _growTime = Random.Range(minTimeToGrow, maxTimeToGrow);
    }

    private void Update()
    {
        if (GameStatesController.Instance.GetGamePauseState()) return;

        if (_isDead)
        {
            return;
        }

        if(WeatherController.Instance.GetCurrentWeatherType() == WeatherController.Weather.Rainy)
        {
            if (_hasGrown) return;

            if (_growTime <= 0f)
            {
                Instantiate(upgradeParticle, transform.position, Quaternion.identity);

                float delayTime = .15f;
                FunctionTimer.Create(() =>
                {
                    transform.localScale = new Vector3(maxSize, maxSize, maxSize);
                    _hasGrown = true;
                }, delayTime);
            }
            else
            {
                _growTime -= Time.deltaTime;
            }
        }
        else if(WeatherController.Instance.GetCurrentWeatherType() == WeatherController.Weather.Sunny)
        {
            animator.SetBool("Dead", true);
            _isDead = true;
        }
    }
    public bool IsDead() => _isDead;
}
