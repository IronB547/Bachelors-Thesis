using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private float maxTurnAngle = 45f;
    private float steerRotationDamp = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        // Wheel rotation and position
        for (int i = 0; i < 4; i++)
        {
            Vector3 position;
            Quaternion rotation;
            WheelsCollider[i].GetWorldPose(out position, out rotation);

            WheelsObject[i].transform.position = position;
            WheelsObject[i].transform.rotation = rotation;
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
                    WheelsCollider[0].motorTorque = motor * 1.3f;
                    WheelsCollider[3].motorTorque = motor * 1.3f;
                    WheelsCollider[1].motorTorque = motor * 1.3f;
                    WheelsCollider[2].motorTorque = motor * 1.3f;

                break;
                case CarDriveType.FrontWheelDrive:
                    for (int i = 0; i < 2; i++)
                    {
                        WheelsCollider[i].motorTorque = motor * 2;
                    }

                    break;
                case CarDriveType.RearWheelDrive:
                    for (int i = 2; i < 4; i++)
                    {
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

            this.transform.position = new Vector3(carPosition.x, carPosition.y + 2, carPosition.z);
            this.transform.rotation = Quaternion.Euler(carRotation);
        }

        if (Input.GetKeyDown(KeyCode.Home))
        {
            SceneManager.LoadScene("TestTrack");
        }
    }
}
