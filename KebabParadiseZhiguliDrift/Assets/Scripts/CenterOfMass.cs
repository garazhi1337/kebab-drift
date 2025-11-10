using System;
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _redBall;
    [SerializeField] private Vector3 _center;

    private void Start()
    {
        UpdateCenter();
    }

    private void OnValidate()
    {
        UpdateCenter();
    }

    private void UpdateCenter()
    {
        _rb.centerOfMass = _center;
        _redBall.position = _center;
    }
}