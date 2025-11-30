using System;
using System.Collections;
using System.Collections.Generic;
using LogitechG29.Sample.Input;
using UnityEngine;

public class RulAndKorobka : MonoBehaviour
{
    [SerializeField] private InputControllerReader _inputControllerReader;
    [SerializeField] private KorobkaPeredachUI _korobkaPeredachUI;
    [Header("moveable parts")]
    [SerializeField] private Transform _rul;
    [SerializeField] private Transform _throttle;
    [SerializeField] private Transform _brake;
    [SerializeField] private Transform _clutch;
    [SerializeField] private Transform _handbrakae;
    [SerializeField] private SoundManager _soundManager;
    
    [SerializeField] private Engine _engine;

    public float clutchValue;
    public float brakeValue;
    public float throttleValue;
    public float steerValue;
    public int CurrentGear = 7;
    private bool[] gearActive = new bool[8];
    private bool[] previousGearActive = new bool[8];

    private void OnEnable()
    {
        //педали и руль
        _inputControllerReader.ClutchCallback += OnClutch;
        _inputControllerReader.BrakeCallback += OnBrake;
        _inputControllerReader.ThrottleCallback += OnThrottle;
        _inputControllerReader.SteeringCallback += OnSteer;
        //коробка
        _inputControllerReader.Shifter1Callback += Sh1;

        _inputControllerReader.Shifter2Callback += Sh2;

        _inputControllerReader.Shifter3Callback += Sh3;

        _inputControllerReader.Shifter4Callback += Sh4;

        _inputControllerReader.Shifter5Callback += Sh5;

        _inputControllerReader.Shifter6Callback += Sh6;

        _inputControllerReader.Shifter7Callback += Sh7;
    }

    private void OnDisable()
    {
        _inputControllerReader.ClutchCallback -= OnClutch;
        _inputControllerReader.BrakeCallback -= OnBrake;
        _inputControllerReader.ThrottleCallback -= OnThrottle;
        _inputControllerReader.SteeringCallback -= OnSteer;
        //коробка
        _inputControllerReader.Shifter1Callback -= Sh1;
                                                
        _inputControllerReader.Shifter2Callback -= Sh2;
                                                
        _inputControllerReader.Shifter3Callback -= Sh3;
                                                
        _inputControllerReader.Shifter4Callback -= Sh4;
                                                
        _inputControllerReader.Shifter5Callback -= Sh5;
                                                
        _inputControllerReader.Shifter6Callback -= Sh6;
                                                
        _inputControllerReader.Shifter7Callback -= Sh7;
    }

    private void Start()
    {

        
        _korobkaPeredachUI.SetPeredachActive(CurrentGear);
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
        try
        {
            _rul.localRotation = Quaternion.Euler(0, 0, -steerValue * 450.0f);
        }
        catch (Exception e)
        {
            
        }

    }
    
    private void UpdateGearState()
    {
        bool anyGearActive = false;
        bool stateChanged = false;
    
        for (int i = 0; i < gearActive.Length; i++)
        {
            if (gearActive[i] != previousGearActive[i])
            {
                stateChanged = true;
                _soundManager.PlaySound(SoundManager.Sounds.PEREDACH_SWITCH);

                if (_inputControllerReader.Clutch < 0.5f && _inputControllerReader.Throttle > 0.05f)
                {
                    _soundManager.PlaySound(SoundManager.Sounds.WRONG_MOTOR);
                }
                
                _korobkaPeredachUI.SetPeredachActive(i);
                previousGearActive[i] = gearActive[i];
            }
        
            if (gearActive[i])
            {
                CurrentGear = i;
                anyGearActive = true;
                break; // Уберите break чтобы сохранять последнюю активную передачу
            }
        }

        if (!anyGearActive)
        {
            CurrentGear = 7;
            _korobkaPeredachUI.SetPeredachActive(CurrentGear);
        }
    
        // Проверять заглохание ТОЛЬКО при изменении состояния передачи
        if (stateChanged && _inputControllerReader.Clutch < 0.5f && anyGearActive) 
        {
            //_engine.Stall();
        }
    }

    private void Sh1(bool b)
    {
        gearActive[0] = b;
        UpdateGearState();
    }
    
    private void Sh2(bool b)
    {
        gearActive[1] = b;
        UpdateGearState();
    }
    
    private void Sh3(bool b)
    {
        gearActive[2] = b;
        UpdateGearState();
    }
    
    private void Sh4(bool b)
    {
        gearActive[3] = b;
        UpdateGearState();
    }
    
    private void Sh5(bool b)
    {
        gearActive[4] = b;
        UpdateGearState();
    }
    
    private void Sh6(bool b)
    {
        gearActive[5] = b;
        UpdateGearState();
    }
    
    private void Sh7(bool b)
    {
        gearActive[6] = b;
        UpdateGearState();
    }
}