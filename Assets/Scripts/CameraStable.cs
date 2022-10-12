using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStable : MonoBehaviour
{
    public GameObject Car;
    public Vector3 CarPos;
    public Quaternion CarRot;
    //public Transform toRotation;

    private float timeCount = 0.0f;

    /*
    public GameObject Car;

    private Vector3 refPos;
    private Vector3 refRot;
    */

    // Update is called once per frame
    void Update()
    {
        CarPos.x = Car.transform.eulerAngles.x;
        CarPos.y = Car.transform.eulerAngles.y;
        CarPos.z = Car.transform.eulerAngles.z;

        //transform.eulerAngles = new Vector3(CarPos.x - CarPos.x, CarPos.y, CarPos.z - CarPos.z); // OLD CODE

        transform.rotation = Quaternion.Euler(0, CarPos.y, 0); // EQUIVALENT EFFECT

        //transform.rotation = Quaternion.Slerp(Car.transform.rotation, transform.rotation, timeCount * 0.1f);
        //timeCount += Time.deltaTime;

        //transform.rotation = Quaternion.Slerp(Car.transform.rotation, toRotation.rotation, timeCount);
        //timeCount += Time.deltaTime;

        //transform.rotation = Quaternion.Slerp(transform.rotation, Car.transform.rotation, rotationSpeed * Time.deltaTime);
    }
}
