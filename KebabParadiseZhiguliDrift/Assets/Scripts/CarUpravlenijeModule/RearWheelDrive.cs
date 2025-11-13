using System;
using System.Collections;
using LogitechG29.Sample.Input;
using UnityEngine;

public class RearWheelDrive : MonoBehaviour
{

	[SerializeField] private RulAndKorobka _rulAndKorobka;
	[SerializeField] private Engine _engine;
	[SerializeField] private float _brakeForce;
	[SerializeField] private InputControllerReader _inputControllerReader;

	private WheelCollider[] wheels;

	public float maxAngle = 30;
	public GameObject wheelShape;
	public float CurrentVelocity;
	private Rigidbody _rb;
	private Coroutine _brakeCoroutine;

    public void Start()
	{
		wheels = GetComponentsInChildren<WheelCollider>();

		for (int i = 0; i < wheels.Length; ++i) 
		{
			var wheel = wheels[i];

			// create wheel shapes only when needed
			if (wheelShape != null)
			{
				var ws = GameObject.Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;
			}
		}

		_rb = GetComponent<Rigidbody>();
	}
    
	private void FixedUpdate()
	{
		HandleMovement();
	}
	
	private void HandleMovement()
	{
		float angle = 0;
		float torque = 0;
		Debug.Log(_engine.GetTorqueFromRPM());
		try
		{
			angle = maxAngle * _rulAndKorobka.steerValue;
			if (_rulAndKorobka.CurrentGear >= 0 && _rulAndKorobka.CurrentGear < _engine.gearRatios.Length)
			{
				torque = _engine.GetTorqueFromRPM() * _engine.gearRatios[_rulAndKorobka.CurrentGear] * _engine._differentialRatio * 0.9f / _engine._wheelRadius;
			}
			else
			{
				torque = 0; // Нейтральная передача или невалидная
			}
		}
		catch (Exception e)
		{
			//артхаус постирония хоррор
		}
		
		if (_engine.currentRPM > _engine.maxRPM + _engine.minRPM && _brakeCoroutine == null) //мега превышение оборотов, автоматическое экстренное торможение
		{
			_brakeCoroutine = StartCoroutine(ControlledBrakeToStop());
		}

		foreach (WheelCollider wheel in wheels)
		{
			// a simple car where front wheels steer while rear ones drive
			if (wheel.transform.localPosition.z > 0)
			{
                wheel.steerAngle = angle;
			}

			if (wheel.transform.localPosition.z < 0)
			{
				wheel.motorTorque = torque;
			}

			if (_inputControllerReader.Brake > _inputControllerReader.Handbrake)
			{
				wheel.brakeTorque = _brakeForce / 4.0f * _inputControllerReader.Brake;
			}
			else
			{
				if (wheel.transform.localPosition.z < 0)
				{
					wheel.brakeTorque = _brakeForce / 4.0f * _inputControllerReader.Handbrake;
				}
			}

			// update visual wheels if any
            if (wheelShape) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);

				// assume that the only child of the wheelcollider is the wheel shape
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
				shapeTransform.localScale = new Vector3(1, 1, 1);
			}
		}
		
		CurrentVelocity = Convert.ToSingle(_rb.velocity.magnitude * 3.6);
	}
	
	private IEnumerator ControlledBrakeToStop()
	{
		bool isBraking = true;
    
		while (isBraking && _rb.velocity.magnitude > 0.5f)
		{
			// Рассчитываем тормозной момент на основе текущей скорости
			float currentSpeed = _rb.velocity.magnitude;
			float brakeTorque = currentSpeed * 800f; // Коэффициент можно настроить
        
			// Ограничиваем максимальный тормозной момент
			brakeTorque = Mathf.Clamp(brakeTorque, 500f, 4000f);
        
			foreach (var wheel in wheels)
			{
				wheel.brakeTorque = brakeTorque;
			}
        
			// Проверяем условия прерывания
			if (currentSpeed < 0.5f)
			{
				isBraking = false;
			}
        
			yield return null;
		}
    
		// Завершение торможения
		foreach (var wheel in wheels)
		{
			wheel.brakeTorque = 0f;
		}
    
		_brakeCoroutine = null;
	}
}

//сделать звуки: машины: нажатие газа, звук колес по асфальту, звук колес по земле, звук торможения, звук врезания
//     			 игровых событий: подбирание шаурмы, появление нового желания, звук отдавания шаурмы
//механики: если всего желаний < 2 то новые не будут появляться. желания появляются раз в 30 секунд и пропадают через 3 минуты
//          отображается на экране количество желаемых продуктов, кол-во оставшегося времени, координаты желания, имя человека
//ии людей: они ходят вдоль тротуара туда-сюда пока не возжелают. тогда они идут к ближайшей из точек желания и вокруг
//          них появляется желтый круг. если их желание выполнить то появляется новое желание через 5 секунд у рандомного человека
//          людей можно сбивать, тогда они будут катиться как ежик соник в противоположную от машины сторону			 