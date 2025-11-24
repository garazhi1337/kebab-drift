using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatShawarma : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _destroyTime;

    private void Start()
    {
        Destroy(gameObject, _destroyTime);
    }
    
    private void FixedUpdate()
    {
        _rb.transform.Rotate(0, Time.fixedDeltaTime * _rotationSpeed, 0);
    }
}
