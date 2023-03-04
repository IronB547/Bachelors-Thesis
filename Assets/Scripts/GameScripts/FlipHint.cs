using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipHint : MonoBehaviour
{

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision + "       " + gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(gameObject.name == "Terrain")
        Debug.Log(collision.gameObject + "       " + gameObject);
    }
}
