using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public GameObject Engine;
    public GameObject Formula;
    public AnimationCurve enginePitchCurve;

    // Start is called before the first frame update
    void Start()
    {
        Engine.GetComponent<AudioSource>().pitch = 0.30f;
    }

    private void FixedUpdate()
    {    
        Engine.GetComponent<AudioSource>().pitch = enginePitchCurve.Evaluate(Formula.GetComponent<Rigidbody>().velocity.magnitude * 3.6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
