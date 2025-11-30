using System.Collections;
using UnityEngine;
using LogitechG29.Sample.Input;
using TMPro;
using Unity.VisualScripting;

public class Engine : MonoBehaviour
{
    [Header("Engine Settings")]
    public float minRPM;
    public float maxRPM;
    public float stallRPM;
    public float currentRPM;
    public float[] gearRatios;
    
    [Header("Torque Curve Settings")]
    [SerializeField] private AnimationCurve _torqueCurve;
    [SerializeField] private float _maxTorque;
    public float _differentialRatio;
    
    [Header("References")]
    [SerializeField] private RulAndKorobka _rulAndKorobka;
    [SerializeField] private InputControllerReader _inputControllerReader;
    [SerializeField] private RearWheelDrive _rearWheelDrive;
    [SerializeField] private PribornajaPanelUI _pribornajaPanel;
    [SerializeField] private SoundManager _soundManager;
    
    public float _wheelRadius;

    private bool isStalling = false;
    private int previousGear;
    private IEnumerator _stallCoroutine = null;

    private float engineRPM = 0f;
    
    private void Start()
    {
        previousGear = _rulAndKorobka.CurrentGear;
    }

    private void Update()
    {
        UpdateRPM();
        previousGear = _rulAndKorobka.CurrentGear;
    }

    private void UpdateRPM()
    {
        // ПРАВИЛЬНЫЙ расчет - БЕЗ повторного применения передаточных чисел
        float physicalWheelRPM = CalculateCorrectWheelRPM();
        float transmissionRPM = physicalWheelRPM * gearRatios[_rulAndKorobka.CurrentGear] * _differentialRatio;
    
        if (_rulAndKorobka.CurrentGear != 7)
        {
            float targetRPM = _inputControllerReader.Throttle * (maxRPM - stallRPM) + stallRPM;
            float clutch = _inputControllerReader.Clutch;
            
            if (clutch > 0.5f) 
            {
                if (currentRPM < minRPM && _inputControllerReader.Throttle > 0.1f)
                {
                    currentRPM = Mathf.Lerp(currentRPM, stallRPM, 5 * Time.deltaTime);
                }
                else
                {
                    currentRPM = Mathf.Lerp(currentRPM, targetRPM, 10 * Time.deltaTime);
                }
            }
            else 
            {
                if (currentRPM < minRPM) 
                { 
                    if (_inputControllerReader.Throttle > 0.1f)
                    {
                        currentRPM = Mathf.Lerp(currentRPM, stallRPM, 10 * Time.deltaTime) + Mathf.Lerp(transmissionRPM, transmissionRPM * clutch, 10 * Time.deltaTime);;
                    }
                    else
                    {
                        currentRPM = Mathf.Lerp(transmissionRPM, transmissionRPM * clutch, 10 * Time.deltaTime);
                    }
                }
                else
                {
                    currentRPM = Mathf.Lerp(transmissionRPM, transmissionRPM * clutch, 10 * Time.deltaTime) + Mathf.Lerp(targetRPM, targetRPM * clutch, 10 * Time.deltaTime);
                }
            }
        }
        else
        {
            float targetRPM = (_inputControllerReader.Throttle * (maxRPM - stallRPM) + stallRPM) * _inputControllerReader.Clutch;
            // ИСПРАВЛЕНО: на нейтрали НЕ используем transmissionRPM
            currentRPM = Mathf.Lerp(currentRPM, targetRPM, 10 * Time.deltaTime);
        }
        
        currentRPM = Mathf.Clamp(currentRPM, stallRPM, maxRPM);
        
        _pribornajaPanel.ObMin = (int)currentRPM;
        _pribornajaPanel.KmH = (int)_rearWheelDrive.CurrentVelocity;
    }

    private float CalculateCorrectWheelRPM()
    {
        // WheelCollider.rpm уже учитывает физику, поэтому используем более простой расчет
        // Берем скорость машины и переводим в RPM колес
        
        float speedKmH = _rearWheelDrive.CurrentVelocity;
        float speedMPS = speedKmH / 3.6f; // переводим в м/с
        
        // RPM колес = (скорость в м/с) / (длина окружности колеса) * 60
        float wheelCircumference = 2f * Mathf.PI * _wheelRadius;
        float wheelRPM = (speedMPS / wheelCircumference) * 60f;
        
        return wheelRPM;
    }

    public float GetTorqueFromRPM()
    {
        if (currentRPM < minRPM)
            return 0f;
    
        float rpmNormalized = Mathf.Clamp01((currentRPM - minRPM) / (maxRPM - minRPM));
        float torqueMultiplier = _torqueCurve.Evaluate(rpmNormalized);

        _soundManager.AdjustMotorSound(rpmNormalized, rpmNormalized * 3);
        _soundManager.PlaySound(SoundManager.Sounds.MOTOR);
        
        float clutchFactor = 1f - _inputControllerReader.Clutch;
        float throttleFactor = _inputControllerReader.Throttle;
        
        return _maxTorque * torqueMultiplier * throttleFactor * clutchFactor;
    }
}