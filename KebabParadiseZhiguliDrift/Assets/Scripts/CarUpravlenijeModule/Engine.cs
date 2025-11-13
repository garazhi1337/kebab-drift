using UnityEngine;
using LogitechG29.Sample.Input;
using Unity.VisualScripting;

public class Engine : MonoBehaviour
{
    [Header("Engine Settings")]
    public float minRPM = 500f;
    public float maxRPM = 7000f;
    public float stallRPM = 800f;
    public float currentRPM = 500f;
    private float _clutchRPM = 350f;
    public float[] gearRatios =
    {
        6.500f,  // 1-я: ~29 км/ч (компромисс)
        3.370f,  // 2-я: 50 км/ч  
        2.406f,  // 3-я: 75 км/ч
        1.925f,  // 4-я: 100 км/ч
        1.604f,  // 5-я: 125 км/ч
        1.337f,  // 6-я: 150 км/ч
        -6.500f, // Задняя
        0f       // Нейтраль
    };
    
    [Header("Torque Curve Settings")]
    [SerializeField] private AnimationCurve _torqueCurve;

    [SerializeField] private float _maxTorque;
    public float _differentialRatio = 3.0f;
    [SerializeField] private RulAndKorobka _rulAndKorobka;
    [SerializeField] private InputControllerReader _inputControllerReader;
    [SerializeField] private RearWheelDrive _rearWheelDrive;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private PribornajaPanelUI _pribornajaPanel;
    
    public float _wheelRadius; // радиус колеса в метрах

    private void Update()
    {
        UpdateRPM();
    }

    private void UpdateRPM()
    {
        //замедление оборотов от тормозов
        float brake = Mathf.Max(_inputControllerReader.Handbrake, _inputControllerReader.Brake);

        float wheelRPM = ((_rearWheelDrive.CurrentVelocity / 3.6f) / (2f * Mathf.PI * _wheelRadius)) * 60f;
        float movementRPM = wheelRPM * gearRatios[_rulAndKorobka.CurrentGear] * _differentialRatio * (1 - _inputControllerReader.Clutch);
        float clutchRPM = _clutchRPM * _inputControllerReader.Clutch;
        float engineRPM = minRPM + (maxRPM - minRPM) * _inputControllerReader.Throttle * (1 - _inputControllerReader.Clutch);
        
        // Совмещаем оба влияния
        currentRPM = Mathf.Lerp(currentRPM, movementRPM + engineRPM + clutchRPM, Time.deltaTime * 10f);

        // Проверка на заглохание
        if (_rulAndKorobka.CurrentGear != 7 && _inputControllerReader.Throttle > 0.05f && currentRPM < stallRPM)
        {
            Stall();
        }

        // Ограничение RPM
        if (currentRPM > maxRPM)
        {
            currentRPM = Mathf.Lerp(currentRPM, maxRPM, Time.deltaTime * 30f);
        }
        
        _pribornajaPanel.KmH = (int)_rearWheelDrive.CurrentVelocity;
        _pribornajaPanel.ObMin = (int)currentRPM;
    }

    public void Stall()
    {
        // Дополнительная логика заглохания
        currentRPM = Mathf.Lerp(currentRPM, minRPM, Time.deltaTime * 60f);
        //можно добавить звук
    }

    public float GetTorqueFromRPM()
    {
        if (currentRPM < stallRPM) 
            return 0f;
        
        float rpmNormalized = currentRPM / maxRPM;
        float torqueMultiplier = _torqueCurve.Evaluate(rpmNormalized);
    
        return _maxTorque * torqueMultiplier;
    }
}