using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    [SerializeField] private float maxDamageDealyTime;

    private float _damageDelayTime;
    private bool _hasDoneDamage = false;
    private void Start()
    {
        _damageDelayTime = maxDamageDealyTime;
    }
    private void Update()
    {
        if (_hasDoneDamage)
        {
            _damageDelayTime -= Time.deltaTime;
            if(_damageDelayTime <= 0f)
            {
                _damageDelayTime = maxDamageDealyTime;
                _hasDoneDamage = false;
            }
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

        if(colliders.Length != 0)
        {
            foreach (Collider item in colliders)
            {
                if(item.TryGetComponent<PlayerController>(out PlayerController playerController) 
                    && item.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
                {
                    healthSystem.TakeDamage();
                    _hasDoneDamage = true;
                }
            }
        }
    }
}
