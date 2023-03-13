using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class CameraManager : MonoBehaviour
{

	private Vector3 closeUpCamPos = new Vector3 (0, 0.9f, 6.0f);
	private Vector3 mainCamPos = new Vector3(0, 1.65f, 9f);
	private Vector3 frontCamPos = new Vector3(0, -0.2f, 0.6f);

    public GameObject Formula;
    public AnimationCurve switchCamCurve;
	public Camera mainCam;
	public Camera rearCam;
	public Camera firstPersonCam;
	public TerrainCollider terrainColl;
	public Transform targetObject;

    public AnimationCurve cameraSmoothMoveCurve;
    public AnimationCurve paramStep;

    public float parameter = 0.175f;

    private float smoothCamMove = 0.05f;
    public int switchCam = 0;
	private float elapsedTime;

    private Ray camRay;
    private Ray TPLowerRay;
    //private Ray TPCloseLowerRay;

    private bool rearCamState;
	private bool camSwitched;

	public bool printRay = true;

	// Start is called before the first frame update
	void Start()
	{
		mainCam.enabled = true;
		rearCamState = false;
		rearCam.enabled = false;
		firstPersonCam.enabled = false;
		camSwitched = false;

		rearCam.GetComponent<AudioListener>().enabled = false;
		firstPersonCam.GetComponent<AudioListener>().enabled = false;
		camRay = new Ray(targetObject.position, (mainCam.transform.position - targetObject.position));
	}

	private void FixedUpdate()
	{
		camRay = new Ray(targetObject.position, (mainCam.transform.position - (targetObject.position  + new Vector3(0, 1f, -1f))));
		TPLowerRay = new Ray(mainCam.transform.position - new Vector3(0, 0.5f, -0.5f), mainCam.transform.position - targetObject.transform.position + new Vector3(0f, -10f, 0f));
		//TPCloseLowerRay = new Ray(mainCam.transform.position - new Vector3(0, 0.5f, -0.5f), (cameraHelperPointClose.position - mainCam.transform.position));
    }

	// Update is called once per frame
	void Update()
	{
		// Full Camera switching part
		if (elapsedTime < 1)
		{
			elapsedTime += Time.deltaTime;
			// Smooth switching between camera positions
			if (switchCam == 1)
				mainCam.GetComponent<SmoothCamera>().initialOffset = Vector3.Lerp(mainCamPos, closeUpCamPos, switchCamCurve.Evaluate(elapsedTime));

			if (switchCam == 2)
			{
				mainCam.GetComponent<SmoothCamera>().initialOffset = Vector3.Lerp(closeUpCamPos, frontCamPos, switchCamCurve.Evaluate(elapsedTime));
				if (elapsedTime >= 0.8f && !camSwitched)
				{
					camSwitched = true;
					mainCam.enabled = false;
					firstPersonCam.enabled = true;
				}
			}


			if (switchCam == 3)
			{
				mainCam.enabled = true;
				firstPersonCam.enabled = false;
				mainCam.GetComponent<SmoothCamera>().initialOffset = Vector3.Lerp(frontCamPos, mainCamPos, switchCamCurve.Evaluate(elapsedTime));
				camSwitched = false;
			}

            if (Input.GetKeyDown(KeyCode.F2))
			{
				if (rearCamState == true)
				{
					rearCam.enabled = false;
					rearCamState = false;
                }
				else
				{
					rearCam.enabled = true;
					rearCamState = true;
				}
			}
		}

		// Change camera position when player pressed F1
		if (Input.GetKeyDown(KeyCode.F1))
		{
			switch (switchCam)
			{
				case 0: // Switch from thirdPerson to closeUp
					elapsedTime = 0;
					switchCam++;
					break;

				case 1: // Switch from closeUp to firstPerson
					elapsedTime = 0;
					switchCam++;
					break;

				case 2: // Swith from firstPerson back to thirdPerson
					elapsedTime = 0;
					switchCam++;
					break;

				default: // Cycle through all the modes
					elapsedTime = 0;
					switchCam = 1;
					break;
			}

		}

		// Raycast to prevent the camera from clipping into terrain.
		// It will be possible to clip through terrain during the first second when switching camera,
		// this shouldn't cause too many issues though.

		Vector3 newCameraVector = mainCam.GetComponent<SmoothCamera>().initialOffset;
		Vector3 startingPos = mainCam.GetComponent<SmoothCamera>().initialOffset;
        RaycastHit hitCamUp;
        RaycastHit hitCamDown;

        // Different rays will be used for different camera positions
        if (switchCam == 0 || switchCam == 3) { // Third person FAR position
			// Move camera slower, since we are on a larger circle
            smoothCamMove = 0.025f;

            if (terrainColl.Raycast(camRay, out hitCamUp, mainCamPos.magnitude))
			// If the ray hits the terain, we move the camera up, in hopes that fixes things.
			// NOTE: This isn't really viable in a place with low ceilings, since the camera will just colide with the ceiling and will be stuck up there forever.
			{
				// Parametric circle equation
				// limit the maximum angle(parameter) the camera can reach
				if (parameter < 1.25f) 
					parameter += paramStep.Evaluate(Formula.GetComponent<CarController>().formulaSpeed);
				else
					parameter = 1.25f;
				
				mainCam.GetComponent<SmoothCamera>().initialOffset = ComputeAngle(parameter, mainCamPos.magnitude);

            }
		else if (!terrainColl.Raycast(TPLowerRay, out hitCamDown, 2f) && parameter != 0.175)
				// Now if a helper ray stops hitting the terrain, we can safely assume that the player has moved quite far from the wall/obstacle,
				// so we may begin the lowering process.
			{
				// Lower the camera
				if (parameter > 0.175f)
					parameter -= paramStep.Evaluate(Formula.GetComponent<CarController>().formulaSpeed);
				else
					parameter = 0.175f;

				mainCam.GetComponent<SmoothCamera>().initialOffset = ComputeAngle(parameter, mainCamPos.magnitude);

            }
		}
		else if(switchCam == 1)
		{
			// Make the camera move faster, since we are moving on a smaller circle
            smoothCamMove = 0.05f;
            if (terrainColl.Raycast(camRay, out hitCamUp, closeUpCamPos.magnitude))
            // If the ray hits the terain, we move the camera up, in hopes that fixes things.
            // NOTE: This isn't really viable in a place with low ceilings, since the camera will just colide with the ceiling and will be stuck up there forever.
            {
				
                // Parametric circle equation
                // limit the maximum angle(parameter) the camera can reach
                if (parameter < 1.25f)
                    parameter += paramStep.Evaluate(Formula.GetComponent<CarController>().formulaSpeed);
                else
                    parameter = 1.25f;

                mainCam.GetComponent<SmoothCamera>().initialOffset = ComputeAngle(parameter, closeUpCamPos.magnitude);

            }
            else if (!terrainColl.Raycast(TPLowerRay, out hitCamDown, 1.5f) && parameter != 0.175)
            // Now if a helper ray stops hitting the terrain, we can safely assume that the player has moved quite far from the wall/obstacle,
            // so we may begin the lowering process.
            {
                // Lower the camera
                if (parameter > 0.175f)
                    parameter -= paramStep.Evaluate(Formula.GetComponent<CarController>().formulaSpeed);
                else
                    parameter = 0.175f;

                mainCam.GetComponent<SmoothCamera>().initialOffset = ComputeAngle(parameter, closeUpCamPos.magnitude);

            }
        }

		if (printRay)
		{
            Debug.DrawRay(targetObject.position, (mainCam.transform.position - (targetObject.position + new Vector3(0,0.5f,-0.5f))), Color.red);
			//Debug.DrawRay(mainCam.transform.position - new Vector3(0, 0.5f, -0.5f), (cameraHelperPointFar.position - mainCam.transform.position), Color.yellow);
            //Debug.DrawRay(mainCam.transform.position - new Vector3(0, 0.5f, -0.5f), (cameraHelperPointClose.position - mainCam.transform.position), Color.green);

            Debug.DrawRay(mainCam.transform.position - new Vector3(0, 0.5f, -0.5f), (mainCam.transform.position - targetObject.transform.position + new Vector3(0f, -10f, 0f)), Color.yellow);

        }

	}

	Vector3 ComputeAngle(float parameter, float circumference)
	{
		// Take the camera position and compute new camera position
		Vector3 newCameraVector = mainCam.GetComponent<SmoothCamera>().initialOffset;
		Vector3 startingPos = mainCam.GetComponent<SmoothCamera>().initialOffset;

        // Take the radius of said circle and move the camera accordingly to the position
		newCameraVector.y = circumference * Mathf.Sin(parameter);
		newCameraVector.z = circumference * Mathf.Cos(parameter);

        // Spherically interpolate between the old position and the new position computed by the function above
        // a great explanation why I used Slerp instead of lerp can be viewed here: https://www.youtube.com/watch?v=YJB1QnEmlTs&t=4m23s
        // I also use a custom curve to change the position of the camera, it resembles a logarithmic function.
        return Vector3.Slerp(startingPos, newCameraVector, cameraSmoothMoveCurve.Evaluate(smoothCamMove));
	}
}
