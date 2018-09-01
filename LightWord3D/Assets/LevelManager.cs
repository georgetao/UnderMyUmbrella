using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    public GameObject player;
    public GameObject exit;

	// Use this for initialization
	void Start () {
        Debug.Log(SceneManager.sceneCount);
	}
	
	// Update is called once per frame
	void Update () {
        float distance = Vector3.Distance(player.transform.position, exit.transform.position);
        if (player.GetComponent<CharacterMovement>().health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   
        }
		if (distance < 0.6)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
	}
}
