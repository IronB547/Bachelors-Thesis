using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    public int MinCount;
    public int SecCount;
    public int MilliCount;
    public float HundredthsCount;

    public TextMeshProUGUI MinBox;
    public TextMeshProUGUI SecBox;
    public TextMeshProUGUI MilliBox;
    public TextMeshProUGUI HundredthsBox;

    // Update is called once per frame
    void Update()
    {
        HundredthsCount += Time.deltaTime * 100;

        // Display Hundreths in UI if less than 9.5. The reason why 9.5 is the limit is because C# rounds up the value 
        // and it wouldn't be pretty with 10 in the UI display. It also has to be above the counting functions, because the delay from ifs
        // invalidates this check
        if (HundredthsCount < 9.5)
        {
            HundredthsBox.text = "" + HundredthsCount.ToString("F0");
        }

        if (HundredthsCount >= 10)
        {
            HundredthsCount = 0;
            MilliCount += 1;
        }

        if (MilliCount >= 10)
        {
            MilliCount = 0;
            SecCount += 1;
        }

        // Also, I do not expect the player to spend an hour on the track so no MinCount overflow check for over 59 minutes
        if (SecCount > 59)
        {
            SecCount = 0;
            MinCount += 1;
        }

        // From here on out we can use 10, because we've solved the issue
        if (MilliCount < 10)
        {
            MilliBox.text = "" + MilliCount;
        }

        if (SecCount < 10) // If seconds are under 10, pad out a 0 at the beginning
        {
            SecBox.text = "0" + SecCount + ".";
        }
        else
        {
            SecBox.text = "" + SecCount + ".";
        }

        if (MinCount < 10) // If minutes are under 10, pad out a 0 at the beginning
        {
            MinBox.text = "0" + MinCount + ":";
        }
        else
        {
            MinBox.text = "" + MinCount + ":";
        }


    }
}
