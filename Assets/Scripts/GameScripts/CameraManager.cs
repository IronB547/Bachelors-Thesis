using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraManager : MonoBehaviour
{

	private Vector3 closeUpCamPos = new Vector3 (0, 0.9f, 6.0f);
	private Vector3 mainCamPos = new Vector3(0, 1.65f, 9f);
	private Vector3 frontCamPos = new Vector3(0, -0.2f, 0.6f);

	public AnimationCurve switchCamCurve;
    public Camera mainCam;
	public Camera rearCam;
	public Camera firstPersonCam;
	public TerrainCollider terrainColl;
	public Transform targetObject;
    public Transform cameraHelperPoint;
    public AnimationCurve cameraSmoothMoveCurve;

    private float parameter = 0.175f;
	private float smoothCamMove = 0.001f;
    private int switch_cam = 0;
    private float elapsedTime;

    private Ray camRay;
	private Ray camLowerRay;

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
		camRay = new Ray(targetObject.position, (mainCam.transform.position - targetObject.position));
		camLowerRay = new Ray(mainCam.transform.position, (cameraHelperPoint.position - mainCam.transform.position));
	}

	// Update is called once per frame
	void Update()
	{
        // Full Camera switching part
        if (elapsedTime < 1)
		{
            elapsedTime += Time.deltaTime;
            // Smooth switching between camera positions
            if (switch_cam == 1)
				mainCam.GetComponent<SmoothCamera>().initialOffset = Vector3.Lerp(mainCamPos, closeUpCamPos, switchCamCurve.Evaluate(elapsedTime));

			if (switch_cam == 2)
			{
				mainCam.GetComponent<SmoothCamera>().initialOffset = Vector3.Lerp(closeUpCamPos, frontCamPos, switchCamCurve.Evaluate(elapsedTime));
				if (elapsedTime >= 0.8f && !camSwitched)
				{
					camSwitched = true;
					mainCam.enabled = false;
					firstPersonCam.enabled = true;
				}
			}


			if (switch_cam == 3)
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

		RaycastHit hitCamUp;
        RaycastHit hitCamDown;
        // Change camera on F1
        if (Input.GetKeyDown(KeyCode.F1))
		{
			switch (switch_cam)
			{
				case 0: // Switch from thirdPerson to closeUp
					elapsedTime = 0;
					switch_cam++;
					break;

				case 1: // Switch from closeUp to firstPerson
					elapsedTime = 0;
					switch_cam++;
					break;

				case 2: // Swith from firstPerson back to thirdPerson
					elapsedTime = 0;
					switch_cam++;
					break;

				default:
					elapsedTime = 0;
					switch_cam = 1;
					break;
			}

		}

		// Raycast to prevent the camera from clipping into terrain.
		// It will be possible to clip through terrain in 1 second when switching camera,
		// this shouldn't cause too many issues though.
		Vector3 newCameraVector = mainCam.GetComponent<SmoothCamera>().initialOffset;
		Vector3 startingPos = mainCam.GetComponent<SmoothCamera>().initialOffset;

		if (switch_cam == 0 || switch_cam == 3) {
            if (terrainColl.Raycast(camRay, out hitCamUp, mainCamPos.magnitude))
            {

                //parameter += tryMoveCam;
				if(parameter < 1.25f)
					parameter += 0.004f;
				else
                    parameter = 1.25f;

                float circumference = mainCamPos.magnitude;
                newCameraVector.y = circumference * Mathf.Sin(parameter);
                newCameraVector.z = circumference * Mathf.Cos(parameter);

                mainCam.GetComponent<SmoothCamera>().initialOffset = Vector3.Slerp(startingPos, newCameraVector, cameraSmoothMoveCurve.Evaluate(smoothCamMove));

            }
            else if (!terrainColl.Raycast(camLowerRay, out hitCamDown, 2.5f) && parameter != 0.175)
			{
				// Lower the camera
				if (parameter > 0.175f)
					parameter -= 0.004f;
				else
					parameter = 0.175f;

                float circumference = mainCamPos.magnitude;
                newCameraVector.y = circumference * Mathf.Sin(parameter);
                newCameraVector.z = circumference * Mathf.Cos(parameter);

                mainCam.GetComponent<SmoothCamera>().initialOffset = Vector3.Slerp(startingPos, newCameraVector, cameraSmoothMoveCurve.Evaluate(smoothCamMove));

            }
		}
		else if(switch_cam == 1)
		{
			if (terrainColl.Raycast(camRay, out hitCamUp, closeUpCamPos.magnitude))
			{
				Debug.Log("I hit close men" + hitCamUp);
			}
		}



		if (printRay)
		{
			Debug.DrawRay(targetObject.position, (mainCam.transform.position - targetObject.position), Color.red);
			Debug.DrawRay(mainCam.transform.position, (cameraHelperPoint.position - mainCam.transform.position), Color.yellow);
            //Debug.DrawRay(targetObject.position, cameraVector, Color.green);
        }

		//Vector3 cameraRay = mainCam.transform.position;
		

		//Ray ray;
		//ray.direction = cameraRay.normalized.z;
		//RaycastHit hit;
		//Terrain.activeTerrain.collider.Raycast(ray, out)
		//Debug.Log(hit.collider.name )


	}
}
