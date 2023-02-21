using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
	public Transform targetObject;
	public Vector3 initialOffset;
	public Vector3 cameraPosition;

	private Vector2 cameraVector;
	private Vector2 carVector;

	public float smoothTime = 0.1f;
	public float smoothSlerp = 0.15f;
	public float camRot = 10;

	public AnimationCurve camPositionCurve;
    public AnimationCurve camRotationCurve;

    void Start()
	{
		// Get the initial offset of the camera
		//initialOffset = transform.position - targetObject.position;
		initialOffset = new Vector3(0, 1.65f, 9f);

		// I need to rotate on the X and Z axes, doing so will create a circular motion
		//Get the values of vectors of camera and car
		cameraVector.x = transform.forward.x;
		cameraVector.y = transform.forward.z;

		carVector.x = targetObject.forward.x;
		carVector.y = targetObject.forward.z;
	}


	// Update is called once per frame
	void FixedUpdate()
	{
		//Update the position of camera
		carVector.x = targetObject.forward.x;
		carVector.y = targetObject.forward.z;

		cameraVector.x = transform.forward.x;
		cameraVector.y = transform.forward.z;
		Vector2 pos = Vector2.Lerp(cameraVector, carVector,  camPositionCurve.Evaluate(smoothTime));

		cameraPosition = targetObject.position + initialOffset;

		// Aproximation of parametric form of circle
		// The targetObject.position.[x,z] are the center coordinates of the circle (the car is the center)
		// initialOffset.z is the radius of the circle
		// pos.x is the interpolated position of the camera (individual steps of the circle)
		// The -1 value is there to put the camera behind the car, not in front since
		// so this flips the vector.

		cameraPosition.x = targetObject.position.x + (initialOffset.z * pos.x * -1);
		cameraPosition.z = targetObject.position.z + (initialOffset.z * pos.y * -1);

		transform.position = cameraPosition;

		// Curve for rotation when player hits the terrain so the camera looks at the center of the formula
		camRot = camRotationCurve.Evaluate(initialOffset.y);
		//Rotation of the camera itself
		Quaternion cameraRotation = Quaternion.Euler(camRot, transform.rotation.eulerAngles.y, 0);

		transform.rotation = Quaternion.Lerp(cameraRotation, targetObject.rotation, smoothSlerp);

	}
}
