using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealisticGravity : MonoBehaviour
{
    [SerializeField] private float _gravitationConstant;
    [SerializeField] private Rigidbody _rb;

    private void FixedUpdate()
    {
        _rb.AddForce(new Vector3(0, -_gravitationConstant * _rb.mass, 0));
    }
}
