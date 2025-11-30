using System;
using UnityEngine;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using CommonUsages = UnityEngine.XR.CommonUsages;

public class XRCameraSetup : MonoBehaviour
{
    [SerializeField] InputActionReference _actionRef;
    [SerializeField] private Transform _resetTransform;
    [SerializeField] private GameObject _player;
    [SerializeField] private Camera _playerHead;

    private void OnEnable()
    {
        _actionRef.action.performed += RotateCameraByAndPlaceToDesired;
    }

    private void OnDisable()
    {
        _actionRef.action.performed -= RotateCameraByAndPlaceToDesired;
    }

    private void RotateCameraByAndPlaceToDesired(InputAction.CallbackContext context)
    {
        float rotationAngleY = _resetTransform.rotation.eulerAngles.y - _playerHead.transform.rotation.eulerAngles.y;
        Vector3 diftanceDiff = _resetTransform.position - _playerHead.transform.position;
        
        _player.transform.Rotate(0, rotationAngleY, 0);
        _player.transform.position += diftanceDiff;
    }
}