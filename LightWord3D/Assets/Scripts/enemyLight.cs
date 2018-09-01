using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLight : MonoBehaviour {
    public GameObject player;
    public GameObject enemy;

    private Vector3 direction;
    private float timeElapsed;

	// Use this for initialization
	void Start () {
        direction = player.transform.position - enemy.transform.position;
        timeElapsed = 0;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(direction * Time.deltaTime/3);

        timeElapsed += Time.deltaTime;
        if (timeElapsed > 15)
        {
            Destroy(gameObject);
        }
	}
}
