using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera ThirdPersonCam;
    public Camera RoofCam;
    public Camera FrontCam;

    private int switch_cam = 0;

    // Start is called before the first frame update
    void Start()
    {
        ThirdPersonCam.enabled = true;
        RoofCam.enabled = false;
        FrontCam.enabled = false;

        RoofCam.GetComponent<AudioListener>().enabled = false;
        FrontCam.GetComponent<AudioListener>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            switch (switch_cam)
            {
                case 0:
                    ThirdPersonCam.enabled = false;
                    ThirdPersonCam.tag = "Untagged";
                    ThirdPersonCam.GetComponent<AudioListener>().enabled = false;

                    RoofCam.enabled = true;
                    RoofCam.tag = "MainCamera";
                    RoofCam.GetComponent<AudioListener>().enabled = true;
                    switch_cam++;
                    break;

                case 1:
                    RoofCam.enabled = false;
                    RoofCam.tag = "Untagged";
                    RoofCam.GetComponent<AudioListener>().enabled = false;

                    FrontCam.enabled = true;
                    FrontCam.tag = "MainCamera";
                    FrontCam.GetComponent<AudioListener>().enabled = true;
                    switch_cam++;
                    break;

                case 2:
                    ThirdPersonCam.enabled = true;
                    ThirdPersonCam.tag = "MainCamera";
                    ThirdPersonCam.GetComponent<AudioListener>().enabled = true;

                    FrontCam.enabled = false;
                    FrontCam.tag = "Untagged";
                    FrontCam.GetComponent<AudioListener>().enabled = false;
                    switch_cam = 0;
                    break;

                default:
                    switch_cam = 0;
                    break;
            }
            
        }   
    }
}
