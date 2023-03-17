using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrasSeed : MonoBehaviour
{
    [SerializeField] private float maxSize;
    [SerializeField] private float maxTimeToGrow;
    [SerializeField] private float minTimeToGrow;
    [SerializeField] private float maxDamageDealyTime;
    [SerializeField] private float checkRadiusDamage;
    [SerializeField] private float checkRadiusHiding;
    [SerializeField] private Transform upgradeParticle;
    [SerializeField] private Animator animator;

    private float _growTime;
    private bool _hasGrown;
    private float _damageDelayTime;
    private bool _hasDoneDamage = false;
    private bool _isDangerours;
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
            _isDangerours = true;

            if (_hasDoneDamage)
            {
                _damageDelayTime -= Time.deltaTime;
                if (_damageDelayTime <= 0f)
                {
                    _damageDelayTime = maxDamageDealyTime;
                    _hasDoneDamage = false;
                }
            }

            if(!_hasDoneDamage)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, checkRadiusDamage);

                if (colliders.Length != 0)
                {
                    foreach (Collider item in colliders)
                    {
                        if (item.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
                        {
                            healthSystem.TakeDamage();
                            _hasDoneDamage = true;
                        }
                    }
                }
            }

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
            _hasDoneDamage = false;
            _isDangerours = false;
            _damageDelayTime = maxDamageDealyTime;
        }
    }

    public bool HasGrown() => _hasGrown;
    public bool IsDangerous() => _isDangerours;
}
