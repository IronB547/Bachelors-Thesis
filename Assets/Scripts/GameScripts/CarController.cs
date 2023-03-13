using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Xml.Schema;
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
	public float maxBrakeTorque = 1000;
	public float totalWheelRPM;
	public float formulaSpeed;
	private float maxTurnAngle = 45f;
	private float steerRotationDamp = 0.5f;
	private float motor;
	private bool carFlip = false;
	private bool brakes = false;
    private bool drift = false;

    // Start is called before the first frame update
    void Start()
	{
		
		carFlip = false;
		brakes = false;
		drift = false;
	}

	// Update is called once per frame
	void FixedUpdate()
	{

		// Compute wheel RPM to determine if the vehicle is going backwards.
		totalWheelRPM = WheelsCollider[0].rpm + WheelsCollider[1].rpm + WheelsCollider[2].rpm + WheelsCollider[3].rpm;

		// Compute the total speed of the vehicle
		formulaSpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3.6f;

		// Check if the car has been flipped or not
		if (WheelsCollider[0].isGrounded && WheelsCollider[1].isGrounded && WheelsCollider[2].isGrounded && WheelsCollider[3].isGrounded)
			carFlip = false;

		// A limiter for steering angle in high velocities
		if (drift)
		{
			maxSteeringAngle = 15;
		}
		else
		{
			if (formulaSpeed >= 72)
				maxSteeringAngle = 10;

			else if (formulaSpeed >= 54)
			{
				maxSteeringAngle = 20 - (2 * ((int)(formulaSpeed / 3.6f) - 15));
			}
			else if (formulaSpeed >= 36)
			{
				maxSteeringAngle = 35 - (3 * ((int)(formulaSpeed / 3.6f) - 10));
			}
			else
				maxSteeringAngle = 35;
		}

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
						WheelsCollider[i].brakeTorque = maxBrakeTorque;
					else if(!brakes)
						WheelsCollider[i].brakeTorque = 0;
				}

				// This torque aplication is meant to help eliminate massive RPM spikes in wheels so that 
				// the vehicle doesn't veer to sides, it doesn't do a great job, but it suffices
				if (totalWheelRPM < 0 && (formulaSpeed > 40))
				{
					for (int i = 0; i < 4; i++)
						WheelsCollider[i].motorTorque = 0;
				}
				else 
				{
					WheelsCollider[0].motorTorque = motor * 1.3f;
					WheelsCollider[3].motorTorque = motor * 1.3f;
					WheelsCollider[1].motorTorque = motor * 1.3f;
					WheelsCollider[2].motorTorque = motor * 1.3f;
				}

				break;
			case CarDriveType.FrontWheelDrive:
				for (int i = 0; i < 2; i++)
				{
					if (WheelsCollider[i].rpm >= 3500f || WheelsCollider[i].rpm <= -2000f)
					{
						WheelsCollider[i].brakeTorque = maxBrakeTorque;
						continue;
					}
					else
						WheelsCollider[i].brakeTorque = 0;
					
					WheelsCollider[i].motorTorque = motor * 2f;
				}

				break;
			case CarDriveType.RearWheelDrive:
				for (int i = 2; i < 4; i++)
				{
					if (WheelsCollider[i].rpm >= 3500f || WheelsCollider[i].rpm <= -2000f)
					{
						WheelsCollider[i].brakeTorque = maxBrakeTorque;
						continue;
					}
					else if(!brakes)
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
		if (Input.GetKeyDown(KeyCode.End) && !carFlip)
		{
			carFlip = true;
			Vector3 carRotation = this.transform.rotation.eulerAngles;
			Vector3 carPosition = this.transform.position;

			carRotation.x = 0;
			carRotation.z = 0;

			gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
			gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

			
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
            drift = true;

            for (int i = 0; i < 4; i++)
			{
				WheelFrictionCurve curve = WheelsCollider[i].sidewaysFriction;

				curve.extremumSlip = 4f;
				curve.extremumValue = 2f;

				WheelsCollider[i].sidewaysFriction = curve;
			}

            for (int i = 2; i < 4; i++)
			{
				WheelsCollider[i].brakeTorque = maxBrakeTorque / 3f;
			}

        }

		// And turn it off
		if (Input.GetKeyUp(KeyCode.Space))
		{
			drift = false;

			for(int i = 0; i < 2; i++)
			{
				WheelFrictionCurve curve = WheelsCollider[i].sidewaysFriction;

				curve.extremumSlip = 0.3f;
                curve.extremumValue = 1f;

                WheelsCollider[i].sidewaysFriction = curve;
			}

			for (int i = 2; i < 4; i++)
			{
				WheelFrictionCurve curve = WheelsCollider[i].sidewaysFriction;
				
				curve.extremumSlip = 0.2f;
                curve.extremumValue = 1f;

                WheelsCollider[i].sidewaysFriction = curve;
                WheelsCollider[i].brakeTorque = 0f;
            }
		}

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
            brakes = true;

            for (int i = 2; i < 4; i++)
			{
				WheelsCollider[i].brakeTorque = maxBrakeTorque;
			}

        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            brakes = false;

            for (int i = 2; i < 4; i++)
            {
                WheelsCollider[i].brakeTorque = 0f;
            }

        }

    }
}
