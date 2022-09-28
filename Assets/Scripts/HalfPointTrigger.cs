using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfPointTrigger : MonoBehaviour
{
    public GameObject LapCompleteTrigger;
    public GameObject LapHalfTrigger;

    void OnTriggerEnter()
    {
        LapCompleteTrigger.SetActive(true);
        LapHalfTrigger.SetActive(false);
    }
}
