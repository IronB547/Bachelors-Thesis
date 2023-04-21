using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DunesWindSoundManager : MonoBehaviour
{

    public AudioSource DunesWindSound;


    void OnTriggerEnter(Collider other)
    {
        if(DunesWindSound.enabled == false)
            DunesWindSound.enabled = true;
        else
            DunesWindSound.enabled = false;
    }

}
