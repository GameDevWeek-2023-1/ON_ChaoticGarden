using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBullet : MonoBehaviour
{
    [SerializeField] private Transform hitEffect;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
        {
            healthSystem.TakeDamage();
        }
        Instantiate(hitEffect, collision.GetContact(0).point, Quaternion.identity);
        Destroy(gameObject);
    }
}
