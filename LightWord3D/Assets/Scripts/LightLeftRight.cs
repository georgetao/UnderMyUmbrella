using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLeftRight : MonoBehaviour {
	public Rigidbody light;
    Vector3 left;
    Vector3 right;
    bool goingLeft;
    public int scalar;
	// Use this for initialization
	void Start () {
        light = gameObject.GetComponent<Rigidbody>();
        left = new Vector3(-1, 0, 0);
        right = new Vector3(1, 0, 0);
        goingLeft = false;
        

	}
	
	// Update is called once per frame
	void Update () {
        if (light.position.x <= -5) {
            goingLeft = false;
        }
        if (light.position.x >= 5) {
            goingLeft = true;
        }
        if (goingLeft)
        {
            light.MovePosition(light.position + left * Time.deltaTime * scalar);
        }
        else {
            light.MovePosition(light.position + right * Time.deltaTime * scalar);    
        }

	}
}
