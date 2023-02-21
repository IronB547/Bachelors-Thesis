using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CarController : MonoBehaviour
{
	public enum CarDriveType
	{
		FrontWheelDrive,
		RearWheelDrive,
		FourWheelDrive
	}

	public CarDriveType carDriveType = CarDriveType.FourWheelDrive;
	[SerializeField] private GameObject[] WheelsObject = new GameObject[4];
	[SerializeField] private WheelCollider[] WheelsCollider = new WheelCollider[4];

	public GameObject SteeringWheel;
	

	public float maxMotorTorque;
	public float maxSteeringAngle;
	public float maxBreakTorque;
	private float maxTurnAngle = 45f;
	private float steerRotationDamp = 0.5f;
	private float motor;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		float formulaSpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;

		// A limiter for steering angle in high velocities
		if (formulaSpeed >= 20.0f)
			maxSteeringAngle = 10;

		else if (formulaSpeed >= 15.0f)
		{
			maxSteeringAngle = 20 - (2 * ((int)formulaSpeed - 15));
		}
		else if (formulaSpeed >= 10.0f)
		{
			maxSteeringAngle = 35 - (3 * ((int)formulaSpeed - 10));
		}
		else
			maxSteeringAngle = 35;

		motor = maxMotorTorque * Input.GetAxis("Vertical");
		float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
		WheelHit wheelData;

		// Wheel rotation and position
		for (int i = 0; i < 4; i++)
		{
			Vector3 position;
			Quaternion rotation;
			WheelsCollider[i].GetWorldPose(out position, out rotation);

			WheelsObject[i].transform.position = position;
			WheelsObject[i].transform.rotation = rotation;

			WheelsCollider[i].GetGroundHit(out wheelData);
		}

		// Apply Motor torque and angle
		// Only first two wheels are steering
		for (int i = 0; i < 2; i++)
		{
			WheelsCollider[i].steerAngle = steering;
		}

		// Select drive type accordingly
		switch (carDriveType)
		{
			case CarDriveType.FourWheelDrive:

				for (int i = 0; i < 4; i++)
				{
					if (WheelsCollider[i].rpm >= 2000f || WheelsCollider[i].rpm <= -2000f)
						WheelsCollider[i].brakeTorque = maxBreakTorque;
					else
						WheelsCollider[i].brakeTorque = 0;
				}

				// This torque aplication is meant to help eliminate massive RPM spikes in wheels so that 
				// the vehicle doesn't veer to sides, it doesn't do a great job, but it suffices
				WheelsCollider[0].motorTorque = motor * 1.3f;
				WheelsCollider[3].motorTorque = motor * 1.3f;
				WheelsCollider[1].motorTorque = motor * 1.3f;
				WheelsCollider[2].motorTorque = motor * 1.3f;

				break;
			case CarDriveType.FrontWheelDrive:
				for (int i = 0; i < 2; i++)
				{
					if (WheelsCollider[i].rpm >= 2000f || WheelsCollider[i].rpm <= -2000f)
					{
						WheelsCollider[i].brakeTorque = maxBreakTorque;
						continue;
					}
					else
						WheelsCollider[i].brakeTorque = 0;

					WheelsCollider[i].motorTorque = motor * 2;
				}

				break;
			case CarDriveType.RearWheelDrive:
				for (int i = 2; i < 4; i++)
				{
					if (WheelsCollider[i].rpm >= 2000f || WheelsCollider[i].rpm <= -2000f)
					{
						WheelsCollider[i].brakeTorque = maxBreakTorque;
						continue;
					}
					else
						WheelsCollider[i].brakeTorque = 0;

					WheelsCollider[i].motorTorque = motor * 2;
				}

				break;
		}

		// CODE TAKEN FROM:
		// https://forum.unity.com/threads/steering-wheel-rotation.126270/
		// ANSWERED BY: LYNXARTS 14.5. 2021
		// Author used an incorrect vector, it is supposed to be Vector3.back, not Vector3.up.

		// The code itself is simple, you take the axis you want to rotate around, in my case Z axis,
		// then insert the value of rotation, in this case that's the player steering to the left or right,
		// times 100 for percentage of rotational value, times the dampening rate and maximum turning angle.
		SteeringWheel.transform.localEulerAngles = Vector3.back * Mathf.Clamp((Input.GetAxis("Horizontal") * 100) * steerRotationDamp, -maxTurnAngle, maxTurnAngle);

	}

	void Update()
	{
		// Flip the car on pressing END key
		if (Input.GetKeyDown(KeyCode.End))
		{
			Vector3 carRotation = this.transform.rotation.eulerAngles;
			Vector3 carPosition = this.transform.position;

			carRotation.x = 0;
			carRotation.z = 0;

			gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

			if (this.transform.position.y <= 20)
				this.transform.position = new Vector3(carPosition.x, carPosition.y + 2, carPosition.z);

			this.transform.rotation = Quaternion.Euler(carRotation);
		}

		// Reload the entire scene on Home button press
		if (Input.GetKeyDown(KeyCode.Home))
		{
			SceneManager.LoadScene("TestTrack");
		}

		// Turn on handbreak when holding down spacebar
		if (Input.GetKeyDown(KeyCode.Space))
		{
			for (int i = 2; i < 4; i++)
			{
				WheelsCollider[i].brakeTorque = maxBreakTorque;
			}
		}

		// And turn it off
		if (Input.GetKeyUp(KeyCode.Space))
		{
			for (int i = 2; i < 4; i++)
			{
				WheelsCollider[i].brakeTorque = 0;
			}
		}

	}
}
