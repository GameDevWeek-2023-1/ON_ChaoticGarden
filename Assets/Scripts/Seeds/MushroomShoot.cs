using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomShoot : MonoBehaviour
{
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private Transform bulletTransform;
    [SerializeField] private Transform spawnPointTransform;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float shootForce = 30f;
    [SerializeField] private MushroomSeed mushroomSeed;

    public bool _isEnemyNearBy;
    private float _shootCooldown;
    private bool _hasShot = false;
    private void Update()
    {
        if (mushroomSeed.IsDead()) return;

        if (!mushroomSeed.IsGrown()) return;

        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, checkRadius, enemyLayerMask);
        
        if (_hasShot)
        {
            _shootCooldown -= Time.deltaTime;
            if(_shootCooldown <= 0f)
            {
                _shootCooldown = shootCooldown;
                _hasShot = false;
            }
            return;
        }

        if(enemyColliders.Length != 0)
        {
            Collider enemyCollider = enemyColliders[0];

            Vector3 shootDir = (enemyCollider.gameObject.transform.position - spawnPointTransform.position).normalized;

            Transform spawnedBulletTransform = Instantiate(bulletTransform, spawnPointTransform);
            spawnedBulletTransform.GetComponent<Rigidbody>().velocity = shootDir * shootForce;

            _hasShot = true;
        }
    }
}
