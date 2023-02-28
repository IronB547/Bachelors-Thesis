using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArrowTriggerManager : MonoBehaviour
{
	public GameObject Formula;
	[SerializeField] public GameObject[] Arrows = new GameObject[4];
	[SerializeField] public GameObject[] ArrowTriggers = new GameObject[4];
	[SerializeField] public AudioSource[] audioSource = new AudioSource[4];
	public AudioClip checkpoint;

	// Start is called before the first frame update
	void Start()
	{
		for(int i = 1; i < 4; i++) { 
			Arrows[i].SetActive(false);
            ArrowTriggers[i].GetComponent<BoxCollider>().enabled = false;
        }


	}

	void OnTriggerEnter(Collider collider)
	{
		if(gameObject.name == "Arrow Trigger1")
		{            
            audioSource[0].PlayOneShot(checkpoint, 1.5f);

            Arrows[0].SetActive(false);
			Arrows[1].SetActive(true);

			ArrowTriggers[0].GetComponent<BoxCollider>().enabled = false;
            ArrowTriggers[1].GetComponent<BoxCollider>().enabled = true;

        }

		if (gameObject.name == "Arrow Trigger2")
		{
			audioSource[1].PlayOneShot(checkpoint, 1.5f);

			Arrows[1].SetActive(false);
			Arrows[2].SetActive(true);

            ArrowTriggers[1].GetComponent<BoxCollider>().enabled = false;
            ArrowTriggers[2].GetComponent<BoxCollider>().enabled = true;
        }

		if (gameObject.name == "Arrow Trigger3")
		{
			audioSource[2].PlayOneShot(checkpoint, 1.5f);

			Arrows[2].SetActive(false);
			Arrows[3].SetActive(true);

            ArrowTriggers[2].GetComponent<BoxCollider>().enabled = false;
            ArrowTriggers[3].GetComponent<BoxCollider>().enabled = true;
        }

		if (gameObject.name == "Arrow Trigger4")
		{
			audioSource[3].PlayOneShot(checkpoint, 1.5f);

			Arrows[3].SetActive(false);
			Arrows[0].SetActive(true);

            ArrowTriggers[3].GetComponent<BoxCollider>().enabled = false;
            ArrowTriggers[0].GetComponent<BoxCollider>().enabled = true;
        }

	}
}
