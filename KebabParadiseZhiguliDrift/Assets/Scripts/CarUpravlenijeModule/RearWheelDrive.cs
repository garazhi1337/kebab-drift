using System;
using UnityEngine;

public class RearWheelDrive : MonoBehaviour
{

	[SerializeField] private RulAndKorobka _rulAndKorobka;
	[SerializeField] private Engine _engine;

	private WheelCollider[] wheels;

	public float maxAngle = 30;
	public GameObject wheelShape;
	public float CurrentVelocity;
	private Rigidbody _rb;

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
    
	private void FixedUpdate()
	{
		HandleMovement();
	}
	
	private void HandleMovement()
	{
		float angle = 0;
		float torque = 0;
		try
		{
			angle = maxAngle * _rulAndKorobka.steerValue;
			//torque = maxTorque * _rulAndKorobka.throttleValue * differentialRatio * gearRatios[currentGear];
			torque = Mathf.Abs(_engine.GetTorqueFromRPM() * _engine.gearRatios[_rulAndKorobka.CurrentGear] * _engine._differentialRatio * 0.9f / _engine._wheelRadius);
		}
		catch (Exception e)
		{
			//артхаус постирония хоррор
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
                //wheel.motorTorque = torque;

				if (_rulAndKorobka.CurrentGear == 6)
				{
					wheel.motorTorque = 0;
					_rb.AddForce(-_rb.transform.forward * torque);
				}
				else
				{
					wheel.motorTorque = torque;
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
}