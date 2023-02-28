using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public GameObject Engine;
	public GameObject Formula;

    public AnimationCurve reversePitchCurve;
    public AnimationCurve t1PitchCurve;
	public AnimationCurve t2PitchCurve;
	public AnimationCurve t3PitchCurve;
	public AnimationCurve t4PitchCurve;
	public AnimationCurve t5PitchCurve;

    public int transmission = 1;
    [SerializeField] private WheelCollider[] WheelsCollider = new WheelCollider[4];

    private bool changeTransmission = false;
	private float pitchstep = 0.025f; // Size of the step the pitch will change when shifting gears
    public float airpitchstep = 0.001f; // Size of the step the pitch will change when the vehicle is in the air

    private float t23LimitRaisePitch = 0.9f;
	private float t4LimitRaisePitch = 0.9f;
	private float t5LimitRaisePitch = 0.85f;

	private float t1LimitLowerPitch = 1.2f;
	private float t234LimitLowerPitch = 1.3f;

	// Start is called before the first frame update
	void Start()
	{
		// Start the engine at 0.30f and transmission 1 (TX from now on, where X is the value of transmission)
		Engine.GetComponent<AudioSource>().pitch = 0.30f;
		transmission = 1;
	}

	private void FixedUpdate()
	{
		// Take speed of the formula
		float speed = Formula.GetComponent<CarController>().formulaSpeed;

        // Change pitch when the formula is in air
        if (!WheelsCollider[0].isGrounded && !WheelsCollider[1].isGrounded && !WheelsCollider[2].isGrounded && !WheelsCollider[3].isGrounded)
        {
            if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0)
            {
                if (Engine.GetComponent<AudioSource>().pitch < 3f)
                    Engine.GetComponent<AudioSource>().pitch += airpitchstep;
				else
                    Engine.GetComponent<AudioSource>().pitch = 3f;

            }
            else
            {
				if(Engine.GetComponent<AudioSource>().pitch > 0.3f)
					Engine.GetComponent<AudioSource>().pitch -= airpitchstep;
				else
                    Engine.GetComponent<AudioSource>().pitch = 0.3f;
            }

            return;
        }

        switch (transmission) // Determine action by transmision
		{
			case 1: // Transmission T1
				if(!changeTransmission && Formula.GetComponent<CarController>().totalWheelRPM > 0) // If we don't change transmissions, change pitch of the sound accordingly to the defined function.
					Engine.GetComponent<AudioSource>().pitch = t1PitchCurve.Evaluate(speed);
				else if(Formula.GetComponent<CarController>().totalWheelRPM < 0)
                    Engine.GetComponent<AudioSource>().pitch = reversePitchCurve.Evaluate(speed);

                if (speed > 20 && Formula.GetComponent<CarController>().totalWheelRPM > 0) // If we hit 20 km/h, we change transmissions
				{
					changeTransmission = true; // Set changeTransmition to true (disables changes in pitch from the code above)
					RaiseTransmission(); // Goto RaiseTransmission();
				}
				

				break;

				// Everything is repeated for each transmission, apart from the limit speed that lowers or raises the gear number.
			case 2: // Transmission T2
				if (!changeTransmission) 
					Engine.GetComponent<AudioSource>().pitch = t2PitchCurve.Evaluate(speed);

				if (speed > 60) // at 60 km/h and above, we want to switch to T3
				{
					changeTransmission = true; 
					RaiseTransmission(); // Goto RaiseTransmission();
				}
				else if (speed < 20) // at 20 km/h and under, we want to switch to T1
				{
					changeTransmission = true; 
					LowerTransmission(); // Goto LowerTransmission();
				}
				break;

			case 3: // Transmission T3
				if (!changeTransmission)
					Engine.GetComponent<AudioSource>().pitch = t3PitchCurve.Evaluate(speed);

				if (speed > 100)
				{
					changeTransmission = true;
					RaiseTransmission();
				}
				else if (speed < 60)
				{
					changeTransmission = true;
					LowerTransmission();
				}
				break;

			case 4: // Transmission T4
				if (!changeTransmission)
					Engine.GetComponent<AudioSource>().pitch = t4PitchCurve.Evaluate(speed);


				if (speed > 140)
				{
					changeTransmission = true;
					RaiseTransmission();
				}
				else if (speed < 100)
				{
					changeTransmission = true;
					LowerTransmission();
				}
				break;

			case 5: // Transmission T5
				if (!changeTransmission)
					Engine.GetComponent<AudioSource>().pitch = t5PitchCurve.Evaluate(speed);

				if (speed < 140)
				{
					changeTransmission = true;
					LowerTransmission();
				}

				break;

		}
	}

	void LowerTransmission()
	{
		// Slowly increase the pitch of the sound and when we hit the correct value of the pitch where the next transmission function ends, change transmissions.
		Engine.GetComponent<AudioSource>().pitch += pitchstep;

		switch (transmission)
		{
			case 2: // T2
				if (Engine.GetComponent<AudioSource>().pitch > t1LimitLowerPitch) // If the pitch is higher than the maximum value of T1, change transmissions.
				{
					transmission -= 1; // Update the gear number in UI
					changeTransmission = false; // Set changeTransmission to false, since we are done changing the transmission.
				}
				break;

			case 3: // T3
				if (Engine.GetComponent<AudioSource>().pitch >= t234LimitLowerPitch) // If the pitch is higher than the maximum value of T2, change transmissions.
				{
					transmission -= 1;
					changeTransmission = false;
				}
				break;

			case 4: // T4
				if (Engine.GetComponent<AudioSource>().pitch > t234LimitLowerPitch)
				{
					transmission -= 1;
					changeTransmission = false;
				}
				break;

			case 5: // T5
				if (Engine.GetComponent<AudioSource>().pitch > t234LimitLowerPitch)
				{
					transmission -= 1;
					changeTransmission = false;
				}
				break;


		}

	}

	void RaiseTransmission()
	{
		// Slowly decrease the pitch of the sound and when we hit the correct value of the pitch where the next transmission function begins, change transmissions.
		Engine.GetComponent<AudioSource>().pitch -= pitchstep;

		switch (transmission)
		{

			case 1: // T1
				if (Engine.GetComponent<AudioSource>().pitch < t23LimitRaisePitch) // If pitch hits the lowest value of pitch for T2, change transmissions.
				{
					transmission += 1; // Change transmission
					changeTransmission = false; // Set changeTransmission to false, since we are done changing the transmission.
				}
				break;
			
			case 2: // T2
				if (Engine.GetComponent<AudioSource>().pitch < t23LimitRaisePitch) // If pitch hits the lowest value of pitch for T3, change transmissions.
				{
					transmission += 1;
					changeTransmission = false;
				}
				break;

			case 3: // T3
				if (Engine.GetComponent<AudioSource>().pitch < t4LimitRaisePitch)
				{
					transmission += 1;
					changeTransmission = false;
				}
				break;

			case 4: // T4
				if (Engine.GetComponent<AudioSource>().pitch < t5LimitRaisePitch)
				{
					transmission += 1;
					changeTransmission = false;
				}
				break;


		}
	}
}