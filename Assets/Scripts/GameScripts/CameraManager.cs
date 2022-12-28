using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCam;
    public Camera frontCam;

    private Vector3 closeUpCamPos = new Vector3 (0, 0.6f, 6.0f);
    private Vector3 mainCamPos = new Vector3(0, 1.65f, 9f);

    private int switch_cam = 0;

    public AnimationCurve curve;

    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        mainCam.enabled = true;
        frontCam.enabled = false;

        frontCam.GetComponent<AudioListener>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (switch_cam == 1)
            mainCam.GetComponent<SmoothCamera>().initialOffset = Vector3.Lerp(mainCamPos, closeUpCamPos, curve.Evaluate(elapsedTime));

        if (switch_cam == 2)
            mainCam.GetComponent<SmoothCamera>().initialOffset = Vector3.Lerp(closeUpCamPos, mainCamPos, curve.Evaluate(elapsedTime));


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

                
                default:
                    elapsedTime = 0;
                    switch_cam = 1;
                    break;
            }
            
        }   
    }
}
