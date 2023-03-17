using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private bool hasRandomRotationSpeed = false;

    private float _currentRotation;
    private void Start()
    {
        if (hasRandomRotationSpeed)
            rotationSpeed = Random.Range(-180, 180);
    }
    private void Update()
    {
        _currentRotation += rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, _currentRotation, 0);
    }
}
