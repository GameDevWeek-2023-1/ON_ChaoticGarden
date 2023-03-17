using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrasSeed : MonoBehaviour
{
    [SerializeField] private float maxSize;
    [SerializeField] private float maxTimeToGrow;
    [SerializeField] private float minTimeToGrow;
    //[SerializeField] private Animator animator;
    [SerializeField] private Transform upgradeParticle;
    [SerializeField] private Animator animator;

    private float _growTime;
    private bool _hasGrown;
    private void Start()
    {
        _growTime = Random.Range(minTimeToGrow, maxTimeToGrow);
    }

    private void Update()
    {
        if (GameStatesController.Instance.GetGamePauseState()) return;

        if (WeatherController.Instance.GetCurrentWeatherType() == WeatherController.Weather.Sunny)
        {
            animator.SetBool("dangerous", true);

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
        else
        {
            animator.SetBool("dangerous", false);
        }
    }
}