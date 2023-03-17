using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBag : MonoBehaviour
{
    [SerializeField] private float checkRadius;
    private void Update()
    {
        Collider[] checkCollider = Physics.OverlapSphere(transform.position, checkRadius);

        if(checkCollider.Length != 0)
        {
            foreach(Collider collider in checkCollider)
            {
                if(collider.TryGetComponent<PlayerController>(out PlayerController playerController))
                {
                    SeedController.Instance.RestockSeeds();
                    Destroy(gameObject);
                }
            }
        }
    }
}
