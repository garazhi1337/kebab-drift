using System;
using UnityEngine;

public class RearWheelDrive : MonoBehaviour
{

	[SerializeField] private RulAndKorobka _rulAndKorobka;

	private WheelCollider[] wheels;

	public float maxAngle = 30;
	public float maxTorque = 85;
	public float[] gearRatios; //это сказал дипсик чтобы машина норм ехала на разных передачах
	public float differentialRatio = 4.1f;
	public int currentGear = 7; // Текущая передача 8 по умолчанию нейтралка
	public GameObject wheelShape;
	private float _currentVelocity;
	private float _gas;
	private float _tormoz;
	private float _clutch;
	private Rigidbody _rb;

    float[,] ranges = new float[,]
    {
        {0f, 15f, 0}, {15f, 35f, 1}, {35f, 55f, 2},
        {55f, 75f, 3}, {75f, 95f, 4}, {95f, 120f, 5},
        {-0.05f, 0.05f, 6}
    };

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
		//SwitchPeredach();
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
				    if (_currentVelocity < ranges[0, 1])
				    {
					    wheel.motorTorque = torque;
				    }
				    else
				    {
					    wheel.motorTorque = 0f; // Отключаем мотор
				    }
				    break;
        
			    case 1: // 2-я передача
				    if (_currentVelocity < ranges[1, 1])
				    {
					    wheel.motorTorque = torque;
				    }
				    else
				    {
					    wheel.motorTorque = 0f; // Отключаем мотор
				    }
				    break;
        
			    case 2: // 3-я передача
				    if (_currentVelocity < ranges[2, 1])
				    {
					    wheel.motorTorque = torque;
				    }
				    else
				    {
					    wheel.motorTorque = 0f; // Отключаем мотор
				    }
				    break;
        
			    case 3: // 4-я передача
				    if (_currentVelocity < ranges[3, 1])
				    {
					    wheel.motorTorque = torque;
				    }
				    else
				    {
					    wheel.motorTorque = 0f; // Отключаем мотор
				    }
				    break;

			    case 4: // 5-я передача
				    if (_currentVelocity < ranges[4, 1])
				    {
					    wheel.motorTorque = torque;
				    }
				    else
				    {
					    wheel.motorTorque = 0f; // Отключаем мотор
				    }
				    break;

                case 5: // 6-я передача
                    if (_currentVelocity < ranges[5, 1])
                    {
                        wheel.motorTorque = torque;
                    }
                    else
                    {
                        wheel.motorTorque = 0f; // Отключаем мотор
                    }
                    break;

                case 6: // 7-я передача задняя
                    if (_currentVelocity < ranges[6, 1])
                    {
                        wheel.motorTorque = torque;
                    }
                    else
                    {
                        wheel.motorTorque = 0f; // Отключаем мотор на нейтралке (когда передача -1)
                    }
					break;
				case 7:
                    wheel.motorTorque = torque;
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

    /**
	private void SwitchPeredach()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Debug.Log("1 передача");

			if (_currentVelocity >= 0 && _currentVelocity <= FIRST_SPEED_MAX) currentGear = 0;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			//2 передача - 15-40 км.ч
			if (_currentVelocity >= 15 && _currentVelocity <= SECOND_SPEED_MAX) currentGear = 1;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			//3 передача - 40-70 км.ч
			if (_currentVelocity >= 35 && _currentVelocity <= THIRD_SPEED_MAX) currentGear = 2;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			//4 передача - 60-100 км.ч
			if (_currentVelocity >= 55 && _currentVelocity <= FOURTH_SPEED_MAX) currentGear = 3;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			//4 передача - 80-142 км.ч
			if (_currentVelocity >= 75 && _currentVelocity <= FIFTH_SPEED_MAX) currentGear = 4;
		}

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            //4 передача - 80-142 км.ч
            if (_currentVelocity >= 95 && _currentVelocity <= FIFTH_SPEED_MAX) currentGear = 5;
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            //4 передача - 80-142 км.ч
            if (_currentVelocity >= -0.05 && _currentVelocity <= 0.05) currentGear = 6;
        }
    }
	**/

    public void UpdateGearBasedOnSpeed(int i)
    {
        currentGear = -1; // по умолчанию нейтралка

        float minSpeed = ranges[i, 0];
        float maxSpeed = ranges[i, 1];
        int gear = (int)ranges[i, 2];

        if (_currentVelocity >= minSpeed && _currentVelocity <= maxSpeed)
        {
            currentGear = gear;
        }
    }
}
