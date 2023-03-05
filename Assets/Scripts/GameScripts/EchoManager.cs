using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EchoManager : MonoBehaviour
{
	public GameObject Engine;
    public bool echoEnabled;

	// Start is called before the first frame update
	void Start()
	{
        Engine.GetComponent<AudioEchoFilter>().enabled = false;

        if (gameObject.name == "Echo Trigger Enter")
        {
            echoEnabled = false;
        }

        if (gameObject.name == "Echo Trigger Exit")
            echoEnabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {
		
		if(other.name == "Hull")
		{
            if (echoEnabled)
			{
                Engine.GetComponent<AudioEchoFilter>().enabled = false;
                echoEnabled = false;
            }
			else
			{
                Engine.GetComponent<AudioEchoFilter>().enabled = true;
                echoEnabled = true;
            }
		}
    }
}
