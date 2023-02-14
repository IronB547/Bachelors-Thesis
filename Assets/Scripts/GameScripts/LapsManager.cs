using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LapsManager : MonoBehaviour
{
    public GameObject Clock;
    public GameObject FinishTrigger;
    public float BestTotalTime;
    public float LastTotalTime;

    public TextMeshProUGUI LastLapMinBox;
    public TextMeshProUGUI LastLapSecBox;
    public TextMeshProUGUI LastLapMilliBox;
    public TextMeshProUGUI LastLapHundredthsBox;

    public TextMeshProUGUI BestTimeMinBox;
    public TextMeshProUGUI BestTimeSecBox;
    public TextMeshProUGUI BestTimeMilliBox;
    public TextMeshProUGUI BestTimeHundredthsBox;
    
    // Update is called once per frame
    void Update()
    {

    }

    void WriteLapTime() // Function for simply overwriting time boxes
    {
        BestTimeMinBox.text = Clock.GetComponent<ClockManager>().MinBox.text;
        BestTimeSecBox.text = Clock.GetComponent<ClockManager>().SecBox.text;
        BestTimeMilliBox.text = Clock.GetComponent<ClockManager>().MilliBox.text;
        BestTimeHundredthsBox.text = Clock.GetComponent<ClockManager>().HundredthsBox.text;
    }

    void OnTriggerEnter()
    {
        FinishTrigger.SetActive(false);

        // Save last lap time into LastLap boxes
        LastLapMinBox.text = Clock.GetComponent<ClockManager>().MinBox.text;
        LastLapSecBox.text = Clock.GetComponent<ClockManager>().SecBox.text;
        LastLapMilliBox.text = Clock.GetComponent<ClockManager>().MilliBox.text;
        LastLapHundredthsBox.text = Clock.GetComponent<ClockManager>().HundredthsBox.text;

        // Reset clock time
        Clock.GetComponent<ClockManager>().MinCount = 0;
        Clock.GetComponent<ClockManager>().SecCount = 0;
        Clock.GetComponent<ClockManager>().MilliCount = 0;
        Clock.GetComponent<ClockManager>().HundredthsCount = 0;

        // Handler for BestTimeBox
        // If there is no previous time set, just insert the current time
        if (BestTimeHundredthsBox.text == "0" && BestTimeMilliBox.text == "0" &&
            int.Parse(BestTimeSecBox.text.Substring(0, BestTimeSecBox.text.Length - 1)) == 0 && 
            int.Parse(BestTimeMinBox.text.Substring(0, BestTimeMinBox.text.Length - 1)) == 0)
        {
            WriteLapTime();
        }
        else // Count total time in float, then compare, if best time is greater than LastTime, rewrite BestTime
        {

            BestTotalTime = int.Parse(BestTimeMinBox.text.Substring(0, BestTimeMinBox.text.Length - 1)) * 60 +
                            int.Parse(BestTimeSecBox.text.Substring(0, BestTimeSecBox.text.Length - 1)) +
                            float.Parse(BestTimeMilliBox.text) / 10 + float.Parse(BestTimeHundredthsBox.text) / 100;

            LastTotalTime = int.Parse(LastLapMinBox.text.Substring(0, LastLapMinBox.text.Length - 1)) * 60 +
                            int.Parse(LastLapSecBox.text.Substring(0, LastLapSecBox.text.Length - 1)) +
                            float.Parse(LastLapMilliBox.text) / 10 + float.Parse(LastLapHundredthsBox.text) / 100;

            if (BestTotalTime > LastTotalTime)
            {
                WriteLapTime();
            }
        }
    }
}
