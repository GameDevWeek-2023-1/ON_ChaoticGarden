using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
        {
            Debug.Log("Hier");
            healthSystem.TakeDamage();
        }
    }
}
