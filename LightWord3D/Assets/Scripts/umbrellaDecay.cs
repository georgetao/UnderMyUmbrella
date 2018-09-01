using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class umbrellaDecay : MonoBehaviour {

    public float timer = 5f;
	// Use this for initialization
	void Awake () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
	}
}
