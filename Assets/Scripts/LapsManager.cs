using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapsManager : MonoBehaviour
{
    public GameObject Clock;
    public GameObject StartTrigger;
    public GameObject FinishTrigger;

    public TextMeshProUGUI LastLapMinBox;
    public TextMeshProUGUI LastLapSecBox;
    public TextMeshProUGUI LastLapMilliBox;
    public TextMeshProUGUI LastLapHundredthsBox;

    public TextMeshProUGUI BestTimeMinBox;
    public TextMeshProUGUI BestTimeSecBox;
    public TextMeshProUGUI BestTimeMilliBox;
    public TextMeshProUGUI BestTimeHundredthsBox;
    
    // Start is called before the first frame update
    void Start()
    {
        Clock.SetActive(false);
        FinishTrigger.SetActive(false);
    }

    // Update is called once per frame
    void OnTriggerEnter()
    {
        // If I entered StartPoint, start the Clock
        if (gameObject.name == "StartTrigger")
        {
            Clock.SetActive(true);
            //Debug.Log(Clock.GetComponent<TextMeshProUGUI>().text);
            StartTrigger.SetActive(false);
            
        }

        if (gameObject.name == "HalfPointTrigger")
        {
            FinishTrigger.SetActive(true);
        }

        // If I reached the finish line, stop the Clock -- temporary for now, will to a three lap check after this.
        if (gameObject.name == "FinishTrigger")
        {
            Clock.GetComponent<TextMeshProUGUI>().text = LastLapMinBox.text;
        }
        /*
        if (gameObject.name == "FinishTrigger" && Laps == 3)
        {
            Clock.SetActive(false);
            // And remove ability to move
        }
        */
    }
}
