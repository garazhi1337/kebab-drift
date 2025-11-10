using System.Collections;
using System.Collections.Generic;
using LogitechG29.Sample.Input;
using UnityEngine;

public class RulAndKorobka : MonoBehaviour
{
    [SerializeField] private InputControllerReader _inputControllerReader;
    [SerializeField] private RearWheelDrive _upravlenije;

    public float clutchValue;
    public float brakeValue;
    public float throttleValue;
    public float steerValue;
    
    private void Start()
    {
        //педали и руль
        _inputControllerReader.ClutchCallback += OnClutch;
        _inputControllerReader.BrakeCallback += OnBrake;
        _inputControllerReader.ThrottleCallback += OnThrottle;
        _inputControllerReader.SteeringCallback += OnSteer;
        //коробка
        _inputControllerReader.Shifter1Callback += b =>
        {
            _upravlenije.currentGear = 0;
        };
        _inputControllerReader.Shifter2Callback += b =>
        {

        };
    }

    private void OnClutch(float value)
    {
        clutchValue = value;
    }
    

    private void OnBrake(float value)
    {
        brakeValue = value;
    }
    
    private void OnThrottle(float value)
    {
        throttleValue = value;
    }

    private void OnSteer(float value)
    {
        steerValue = value;
    }

    private void OnFirstShift()
    {
        
    }
}
