using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour {
    
    public Rigidbody enemyRb;
    public GameObject player;
    public GameObject enemyLight;

    public float explosionRadius;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        enemyRb.velocity = new Vector3(-1, 0, 0);

        float distance = Vector3.Distance(player.transform.position, this.transform.position);

        if (distance < explosionRadius)
        {
            enabled = false;

            enemyRb.velocity = new Vector3(0, 0, 0);
            Vector3 spawnPosition = this.transform.position;
            Destroy(gameObject, 1);

            Instantiate(enemyLight, spawnPosition, new Quaternion(0, 0, 0, 0));
        }
	}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);   
    }
}
