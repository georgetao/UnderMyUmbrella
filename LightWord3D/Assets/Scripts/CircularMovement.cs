using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour {
    float timeCounter;
    public float scalar;
	// Use this for initialization
	void Start () {
        timeCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timeCounter += (Time.deltaTime/2);
        float x = scalar*Mathf.Cos(timeCounter);
        float y = scalar * Mathf.Sin(timeCounter);
        float z = -1;
        gameObject.transform.position = new Vector3(x, y, z);
	}
}
