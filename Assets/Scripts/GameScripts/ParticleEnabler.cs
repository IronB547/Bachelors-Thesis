using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleEnabler : MonoBehaviour
{
	public GameObject LeftWheelSandParticles;
	public GameObject RightWheelSandParticles;

	public WheelCollider LeftWheel;
	public WheelCollider RightWheel;

	public GameObject formula;

	public BoxCollider Start;
	public BoxCollider End;

	private bool particleState = false;
	public bool enteredDune = false;

    void OnTriggerEnter(Collider other)
	{

        if (gameObject.name == "ParticleTriggerEnter")
        {
            enteredDune = true;
            End.enabled = true;
            Start.enabled = false;

            if (LeftWheel.isGrounded)
                LeftWheelSandParticles.SetActive(true);
            if (RightWheel.isGrounded)
                RightWheelSandParticles.SetActive(true);
        }

        if (gameObject.name == "ParticleTriggerExit")
		{
			Start.GetComponent<ParticleEnabler>().enteredDune = false;
            enteredDune = false;
            Start.enabled = true;
			End.enabled = false;

            LeftWheelSandParticles.SetActive(false);
            RightWheelSandParticles.SetActive(false);
        }
	}


	private void FixedUpdate()
	{

		if (enteredDune)
		{
			
			if (formula.GetComponent<CarController>().formulaSpeed < 15f)
				particleState = false;
			else
				particleState = true;

			if (LeftWheel.isGrounded == false)
				particleState = false;

			if (RightWheel.isGrounded == false)
				particleState = false;

			if (particleState == false)
			{
				LeftWheelSandParticles.SetActive(false);
				RightWheelSandParticles.SetActive(false);
			}
			else if (particleState == true)
			{
				if (LeftWheel.isGrounded)
					LeftWheelSandParticles.SetActive(true);
				if (RightWheel.isGrounded)
					RightWheelSandParticles.SetActive(true);
			}
		}
				
	}

}
