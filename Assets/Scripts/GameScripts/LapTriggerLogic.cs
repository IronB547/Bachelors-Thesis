using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class LapTriggerLogic : MonoBehaviour
{
    public GameObject Clock;
    public GameObject StartTrigger;
    public GameObject FinishTrigger;

    public int laps = 3; // Later change this number from settings
    public TextMeshProUGUI LapsLeft;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name == "StartTrigger")
        {
            Clock.SetActive(false);
            LapsLeft.text = laps.ToString("F0");
            FinishTrigger.SetActive(false);
        }

    }

    void OnTriggerEnter(Collider collided)
    {
        // If I entered StartPoint, start the Clock
        if (gameObject.name == "StartTrigger")
        {
            Clock.SetActive(true);
            StartTrigger.SetActive(false);
            
        }

        if (gameObject.name == "HalfPointTrigger")
        {
            Debug.Log("Hello I went through");
            FinishTrigger.SetActive(true);
        }

        if (gameObject.name == "FinishTrigger")
        {
            if (laps >= 1)
            {
                laps--;
                LapsLeft.text = laps.ToString("F0");
            }


            if (laps <= 0)
            {
                Clock.GetComponent<ClockManager>().enabled = false;
                //collided.transform.parent.gameObject.transform.parent.GetComponent<CarUserControl>().enabled = false; // If a car has reached the finish line, terminate user control.
            }
        }

    }
}
