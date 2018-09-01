using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossLight : MonoBehaviour {

    public Vector3 direction;

    private float timeElapsed;
    private int difficulty;

	// Use this for initialization
	void Start () {
        timeElapsed = 0;
        if (GameObject.Find("Lights").GetComponent<boss>().bossEnergy < 5)
        {
            difficulty = 5;
        }
        else
        {
            difficulty = 2;
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(direction * Time.deltaTime * difficulty);

        timeElapsed += Time.deltaTime;
        if (Mathf.Abs(transform.position.x) > 15 || Mathf.Abs(transform.position.y) > 10)
        {
            GameObject.Find("Lights").GetComponent<boss>().nextRound += 1;
            Destroy(gameObject);
            enabled = false;
        }

    }
}
