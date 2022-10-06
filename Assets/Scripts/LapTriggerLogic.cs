using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapTriggerLogic : MonoBehaviour
{
    public GameObject Clock;
    public GameObject StartTrigger;
    public GameObject FinishTrigger;

    void Start()
    {
        if (gameObject.name == "StartTrigger")
        {
            Clock.SetActive(false);
        }

        FinishTrigger.SetActive(false);
    }

    void OnTriggerEnter()
    {
        // If I entered StartPoint, start the Clock
        if (gameObject.name == "StartTrigger")
        {
            Clock.SetActive(true);
            StartTrigger.SetActive(false);
        }

        if (gameObject.name == "HalfPointTrigger")
        {
            FinishTrigger.SetActive(true);
        }
    }
}
