using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStable : MonoBehaviour
{
    public GameObject Car;
    public Vector3 CarPos;

    // Update is called once per frame
    void Update()
    {
        CarPos.x = Car.transform.eulerAngles.x;
        CarPos.y = Car.transform.eulerAngles.y;
        CarPos.z = Car.transform.eulerAngles.z;

        transform.eulerAngles = new Vector3(CarPos.x - CarPos.x, CarPos.y, CarPos.z - CarPos.z);
    }
}
