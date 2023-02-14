using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    private Vector3 closeUpCamPos = new Vector3 (0, 0.9f, 6.0f);
    private Vector3 mainCamPos = new Vector3(0, 1.65f, 9f);

    private Vector3 frontCamPos = new Vector3(0, -0.2f, 0.6f);

    public int switch_cam = 0;
    public float elapsedTime;

    public AnimationCurve curve;
    public Camera mainCam;
    public Camera rearCam;
    public Camera firstPersonCam;

    private bool rearCamState;
    private bool camSwitched;

    // Start is called before the first frame update
    void Start()
    {
        mainCam.enabled = true;
        rearCamState = false;
        rearCam.enabled = false;
        firstPersonCam.enabled = false;
        camSwitched = false;

        rearCam.GetComponent<AudioListener>().enabled = false;
        firstPersonCam.GetComponent<AudioListener>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Smooth switching between camera positions
        if (switch_cam == 1)
            mainCam.GetComponent<SmoothCamera>().initialOffset = Vector3.Lerp(mainCamPos, closeUpCamPos, curve.Evaluate(elapsedTime));

        if (switch_cam == 2)
        {
            mainCam.GetComponent<SmoothCamera>().initialOffset = Vector3.Lerp(closeUpCamPos, frontCamPos, curve.Evaluate(elapsedTime));
            if (elapsedTime >= 0.8f && !camSwitched)
            {
                camSwitched = true;
                mainCam.enabled = false;
                firstPersonCam.enabled = true;
            }
        }


        if (switch_cam == 3)
        {
            mainCam.enabled = true;
            firstPersonCam.enabled = false;
            mainCam.GetComponent<SmoothCamera>().initialOffset = Vector3.Lerp(frontCamPos, mainCamPos, curve.Evaluate(elapsedTime));
            camSwitched = false;
        }
            

        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (rearCamState == true)
            {
                rearCam.enabled = false;
                rearCamState = false;
            }
            else
            {
                rearCam.enabled = true;
                rearCamState = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            switch (switch_cam)
            {
                case 0: // Switch from thirdPerson to closeUp
                    //Vector2 pos = Vector2.Lerp(cameraVector, carVector, smoothTime);

                    //mainCam.GetComponent<Transform>().position = Vector3.Lerp(mainCamPos, closeUpCamPos, 0.02f);
                    //mainCam.GetComponent<SmoothCamera>().initialOffset = closeUpCamPos;

                    // OLD CODE
                    //thirdPersonCam.enabled = false;
                    //thirdPersonCam.tag = "Untagged";
                    //thirdPersonCam.GetComponent<AudioListener>().enabled = false;

                    //closeUpCam.enabled = true;
                    //closeUpCam.tag = "MainCamera";
                    //closeUpCam.GetComponent<AudioListener>().enabled = true;
                    elapsedTime = 0;
                    switch_cam++;
                    break;

                case 1: // Switch from closeUp
                    //mainCam.GetComponent<Transform>().position = Vector3.Lerp(closeUpCamPos, mainCamPos, 0.02f);
                    //mainCam.GetComponent<SmoothCamera>().initialOffset = mainCamPos;
                    //switch_cam++;
                    elapsedTime = 0;
                    switch_cam++;
                    break;

                case 2:
                    elapsedTime = 0;
                    switch_cam++;
                    break;

                default:
                    elapsedTime = 0;
                    switch_cam = 1;
                    break;
            }
            
        }   
    }
}
