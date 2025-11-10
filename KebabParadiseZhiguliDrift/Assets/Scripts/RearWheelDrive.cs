using System;
using UnityEngine;

public class RearWheelDrive : MonoBehaviour
{

	[SerializeField] private RulAndKorobka _rulAndKorobka;
	private WheelCollider[] wheels;

	public float maxAngle = 30;
	public float maxTorque = 85;
	public float[] gearRatios = { 3.667f, 2.100f, 1.361f, 1.000f, 0.821f, -3.530f }; //это сказал дипсик чтобы машина норм ехала на разных передачах
	public float differentialRatio = 4.1f;
	public int currentGear = 0; // Текущая передача
	public GameObject wheelShape;
	private float _currentVelocity;
	private float _gas;
	private float _tormoz;
	private float _clutch;
	private Rigidbody _rb;
	
	//макс скорости для передач
	private const int FIRST_SPEED_MAX = 25;
	private const int SECOND_SPEED_MAX = 45;
	private const int THIRD_SPEED_MAX = 70;
	private const int FOURTH_SPEED_MAX = 100;
	private const int FIFTH_SPEED_MAX = 142;

	// here we find all the WheelColliders down in the hierarchy
	public void Start()
	{
		wheels = GetComponentsInChildren<WheelCollider>();

		for (int i = 0; i < wheels.Length; ++i) 
		{
			var wheel = wheels [i];

			// create wheel shapes only when needed
			if (wheelShape != null)
			{
				var ws = GameObject.Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;
			}
		}

		_rb = GetComponent<Rigidbody>();
	}

	// this is a really simple approach to updating wheels
	// here we simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero
	// this helps us to figure our which wheels are front ones and which are rear
	private void FixedUpdate()
	{
		HandleMovement();
	}

	private void Update()
	{
		SwitchPeredach();
	}

	private void HandleMovement()
	{
		float angle;
		float torque;
		try
		{
			angle = maxAngle * _rulAndKorobka.steerValue;
			torque = maxTorque * _rulAndKorobka.throttleValue * differentialRatio * gearRatios[currentGear];
		}
		catch (Exception e)
		{
			angle = maxAngle * Input.GetAxis("Horizontal");
			torque = maxTorque * Input.GetAxis("Vertical") * differentialRatio * gearRatios[currentGear];
		}


		foreach (WheelCollider wheel in wheels)
		{
			// a simple car where front wheels steer while rear ones drive
			if (wheel.transform.localPosition.z > 0)
				wheel.steerAngle = angle;
			    //wheel.motorTorque = torque;

			switch (currentGear)
			{
			    case 0: // 1-я передача
				    if (_currentVelocity < FIRST_SPEED_MAX)
				    {
					    wheel.motorTorque = torque;
				    }
				    else
				    {
					    wheel.motorTorque = 0f; // Отключаем мотор
				    }
				    break;
        
			    case 1: // 2-я передача
				    if (_currentVelocity < SECOND_SPEED_MAX)
				    {
					    wheel.motorTorque = torque;
				    }
				    else
				    {
					    wheel.motorTorque = 0f; // Отключаем мотор
				    }
				    break;
        
			    case 2: // 3-я передача
				    if (_currentVelocity < THIRD_SPEED_MAX)
				    {
					    wheel.motorTorque = torque;
				    }
				    else
				    {
					    wheel.motorTorque = 0f; // Отключаем мотор
				    }
				    break;
        
			    case 3: // 4-я передача
				    if (_currentVelocity < FOURTH_SPEED_MAX)
				    {
					    wheel.motorTorque = torque;
				    }
				    else
				    {
					    wheel.motorTorque = 0f; // Отключаем мотор
				    }
				    break;

			    case 4: // 5-я передача
				    if (_currentVelocity < FIFTH_SPEED_MAX)
				    {
					    wheel.motorTorque = torque;
				    }
				    else
				    {
					    wheel.motorTorque = 0f; // Отключаем мотор
				    }
				    break;
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
		
		_currentVelocity = Convert.ToSingle(GetComponent<Rigidbody>().velocity.magnitude * 3.6);
		Debug.Log(_currentVelocity);
	}

	private void SwitchPeredach()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Debug.Log("1 передача");
			//1 передача - 0-25 км.ч
			/*
			 * если 0 км.ч то 1. сцепление 100 2. 1 передача 3. газ 10-20 4. сцепление 50-70 5. сцепление 0
			 */
			if (_currentVelocity >= 0 && _currentVelocity <= 25) currentGear = 0;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			//2 передача - 20-45 км.ч
			if (_currentVelocity >= 20 && _currentVelocity <= 45) currentGear = 1;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			//3 передача - 40-70 км.ч
			if (_currentVelocity >= 40 && _currentVelocity <= 70) currentGear = 2;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			//4 передача - 60-100 км.ч
			if (_currentVelocity >= 60 && _currentVelocity <= 100) currentGear = 3;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			//4 передача - 80-142 км.ч
			if (_currentVelocity >= 80 && _currentVelocity <= 142) currentGear = 4;
		}
	}
}
