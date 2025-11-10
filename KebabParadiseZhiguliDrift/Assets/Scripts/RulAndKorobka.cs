using System.Collections;
using System.Collections.Generic;
using LogitechG29.Sample.Input;
using UnityEngine;

public class RulAndKorobka : MonoBehaviour
{
    [SerializeField] private InputControllerReader _inputControllerReader;
    [SerializeField] private RearWheelDrive _upravlenije;
    [SerializeField] private Transform _rul;

    public float clutchValue;
    public float brakeValue;
    public float throttleValue;
    public float steerValue;
    private bool[] gearActive = new bool[7];

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
            gearActive[0] = b;
            UpdateGearState();
        };

        _inputControllerReader.Shifter2Callback += b =>
        {
            gearActive[1] = b;
            UpdateGearState();
        };

        _inputControllerReader.Shifter3Callback += b =>
        {
            gearActive[2] = b;
            UpdateGearState();
        };

        _inputControllerReader.Shifter4Callback += b =>
        {
            gearActive[3] = b;
            UpdateGearState();
        };

        _inputControllerReader.Shifter5Callback += b =>
        {
            gearActive[5] = b;
            UpdateGearState();
        };

        _inputControllerReader.Shifter6Callback += b =>
        {
            gearActive[5] = b;
            UpdateGearState();
        };

        _inputControllerReader.Shifter7Callback += b =>
        {
            gearActive[6] = b;
            UpdateGearState();
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
        _rul.localRotation = Quaternion.Euler(0, 0, steerValue * 900.0f);
    }



    private void UpdateGearState()
    {
        bool anyGearActive = false;
        for (int i = 0; i < gearActive.Length; i++)
        {
            if (gearActive[i])
            {
                _upravlenije.UpdateGearBasedOnSpeed(i);
                anyGearActive = true;
                break;
            }
        }

        if (!anyGearActive)
        {
            _upravlenije.currentGear = 8;
        }
    }

}
