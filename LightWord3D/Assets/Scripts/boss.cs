using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class boss : MonoBehaviour {

    public GameObject bossLight;
    public GameObject player;
    public float bossEnergy;
    public int nextRound;
    public Slider energyBar;

    private Vector3 bossPosition;
    private delegate void differentAttacks();
    private List<differentAttacks> attacks;
    private float startingEnergy;

	// Use this for initialization
	void Start () {
        energyBar.value = 0;
        bossPosition = this.transform.position;
        attacks = new List<differentAttacks>();
        attacks.Add(Attack0);
        attacks.Add(Attack1);
        attacks.Add(Attack2);
        attacks.Add(Attack3);
        attacks.Add(Attack4);
        startingEnergy = bossEnergy;

        nextRound = 3; //if nextRound = 3 then queue next round of lights
    }
	
	// Update is called once per frame
	void Update () {
        energyBar.value = (startingEnergy - bossEnergy) / startingEnergy;
        while (bossEnergy > 1 && nextRound >= 3)
        {
            nextRound = 0;
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            int rand = Random.Range(0, attacks.Count);
            attacks[rand]();
            bossEnergy -= 1; 
        }
        if (bossEnergy == 1 && transform.childCount == 0) 
        {
            AttackFinal();
            bossEnergy -= 1;
        }
        if (bossEnergy == 0 && transform.childCount == 0)
        {
            SceneManager.LoadScene(0);
        }
	}
    
    void Shoot(float x, float y)
    {
        GameObject light = Instantiate(bossLight, bossPosition, new Quaternion(0, 0, 0, 0));
        light.transform.parent = gameObject.transform;
        bossLight lightDirection = light.GetComponent<bossLight>();
        lightDirection.direction = new Vector3(x, y, 0);
        lightDirection.direction.Normalize();
    }

    void Attack0() //Shoots balls Left Right Up and Down
    {
        Shoot(-1, 0);
        Shoot(1, 0);
        Shoot(0, 1);
        Shoot(0, -1);

    }

    void Attack1() //Shoots balls at diagonals
    {
        Shoot(1, 1);
        Shoot(-1, 1);
        Shoot(-1, -1);
        Shoot(1, -1);
    }

    void Attack2() //Shoots at players either left or right
    {
        Vector3 playerDirection = player.transform.position - bossPosition;
        if (playerDirection.x <= 0)
        {
            Shoot(-1, 1);
            Shoot(-1, 0);
            Shoot(-1, -1);
        }
        else
        {
            Shoot(1, 1);
            Shoot(1, 0);
            Shoot(1, -1);
        }
    }

    void Attack3() // Shoots at players either up or down
    {
        Vector3 playerDirection = player.transform.position - bossPosition;
        if (playerDirection.y <= 0)
        {
            Shoot(-1, -1);
            Shoot(0, -1);
            Shoot(1, -1);
        }
        else
        {
            Shoot(-1, 1);
            Shoot(0, 1);
            Shoot(1, 1);
        }
    }

    void Attack4() // Shoots 3 different shots that follow the player
    {
        StartCoroutine(Attack4Coroutine());
    }

    IEnumerator Attack4Coroutine()
    {
        Vector3 playerDirection = player.transform.position - bossPosition;
        Shoot(playerDirection.x, playerDirection.y);
        yield return new WaitForSeconds(3);

        playerDirection = player.transform.position - bossPosition;
        Shoot(playerDirection.x, playerDirection.y);
        yield return new WaitForSeconds(3);

        playerDirection = player.transform.position - bossPosition;
        Shoot(playerDirection.x, playerDirection.y);
    }

    void AttackFinal()
    {
        StartCoroutine(AttackFinalCoroutine());
    }

    IEnumerator AttackFinalCoroutine()
    {
        Attack3();
        yield return new WaitForSeconds(2);

        Attack2();
        yield return new WaitForSeconds(2);

        Attack4();
    }
}
