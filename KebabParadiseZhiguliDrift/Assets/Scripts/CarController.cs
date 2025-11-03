using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private int _maxVelocity;
    [SerializeField] private int _velocityIncrement;
    [SerializeField] private int _maxForwardForce;
    [SerializeField] private int _breakForce;
    private Rigidbody _rb;
    private int _currTorque = 0;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (_rb.GetAccumulatedForce().z <= _maxForwardForce)
            {
                //_rb.AddForce(new Vector3(0, 0, 1000));
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            
        }
    }
}
