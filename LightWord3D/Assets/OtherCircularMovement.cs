using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherCircularMovement : MonoBehaviour {
    float timeCounter;
    // Use this for initialization
    void Start()
    {
        timeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += (Time.deltaTime/2);
        
        float x = 3 * Mathf.Cos(timeCounter + (-180 * Mathf.Deg2Rad));
        float y = 3 * Mathf.Sin(timeCounter);
        float z = -1;
        gameObject.transform.position = new Vector3(x, y, z);
    }
}
