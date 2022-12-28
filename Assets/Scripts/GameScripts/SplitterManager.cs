using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterManager : MonoBehaviour
{
    public GameObject split1;
    public GameObject split2;
    public GameObject split3;

    public GameObject clock;

    private float time = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        time += clock.GetComponent<ClockManager>().HundredthsCount / 100;
        time += clock.GetComponent<ClockManager>().MilliCount / 10;
        time += clock.GetComponent<ClockManager>().SecCount;
        time += clock.GetComponent<ClockManager>().MinCount * 60;

        Debug.Log(time);
        Debug.Log(gameObject);

        if (gameObject.name == "Split 1")
        {
            
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        if (gameObject.name == "Split 2")
        {
            Debug.Log(time);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        time = 0;
    }
}
