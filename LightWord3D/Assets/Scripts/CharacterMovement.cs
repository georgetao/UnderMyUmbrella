using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {

    //Stats
    public float movespeed;
    public float umbrellaDur = 5f;
    public float health = 100;
    private float sightRadius = 50;
    private float tpMaxCd = 0.5f;
    private float tpCd = 0;
    private float umbrellaMaxCd = 2f;
    private float umbrellaCd = 0;
    private bool stunned = false;

    //Components
    Rigidbody rb;
    Vector3 mousePos;
    public Rigidbody umbrellaPrefab;
    Rigidbody currUmbrella = null;
    Animator anim;

    //Environment
    Transform[] lights;
    Vector3 cameraDist;
    Plane playground;

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
        lights = GameObject.Find("Lights").GetComponentsInChildren<Transform>();

        cameraDist = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
        playground = new Plane(Vector3.forward, cameraDist);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (umbrellaCd > 0)
        {
            umbrellaCd -= Time.deltaTime;
        }
        if (tpCd > 0)
        {
            tpCd -= Time.deltaTime;
        }
        if(currUmbrella == null)
        {
            anim.SetBool("Umbrella", false);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            teleport();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            spawnbrella();
        }

        if (!stunned)
        {
            move();
        }

        if (!inShadow())
        {
            takeDamage(Time.deltaTime * 10);
        }
	}

    void move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(moveX, moveY, 0);
        dir.Normalize();
        rb.velocity = dir * movespeed;

        if (moveX == 0 && moveY == 0)
        {
            anim.SetBool("Movement", false);
        }
        else
        {
            anim.SetBool("Movement", true);
            anim.SetFloat("LastDirX", moveX);
            anim.SetFloat("LastDirY", moveY);
        }
        anim.SetFloat("DirX", moveX);
        anim.SetFloat("DirY", moveY);
    }

    void teleport()
    {
        if(tpCd > 0)
        {
            return;
        } 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hit = 0;
        if (playground.Raycast(ray, out hit)) {
            Vector3 mousePos = ray.GetPoint(hit);
            if (inShadow() && targetInShadow(mousePos) && targetInLOS(mousePos))
            {
                StartCoroutine(tpAnim(mousePos));
            }
        }
        tpCd = tpMaxCd;
    }

    IEnumerator tpAnim(Vector3 mousePos)
    {
        stunned = true;
        float dur = 0.2f;
        float t = 0;
        anim.SetBool("Dive", true);
        while(t < dur)
        {
            t += Time.deltaTime;
            yield return null;
        }
        rb.MovePosition(mousePos);
        anim.SetBool("Dive", false);
        stunned = false;
    }

    private bool inShadow()
    {
        foreach (Transform light in lights) {
            Vector3 castDir = light.position - transform.position;
            RaycastHit hit;
            Ray ray = new Ray(transform.position, castDir);
            if (Physics.Raycast(ray, out hit, sightRadius))
            {
               if(hit.collider != null && hit.collider.tag == "Light")
                {
                    return false;
                }
               else if(hit.collider != null && hit.collider.tag == "Obstacle")
                {
                    continue;
                }
            }
        }
        return true;
    }

    private bool targetInShadow(Vector3 mousePos)
    {
        foreach (Transform light in lights)
        {
            Vector3 castDir = light.position - mousePos;
            RaycastHit hit;
            Ray ray = new Ray(mousePos, castDir);
            if (Physics.Raycast(ray, out hit, sightRadius))
            {
                if (hit.collider != null && hit.collider.tag == "Light")
                {
                    return false;
                }
                else if (hit.collider != null && hit.collider.tag == "Obstacle")
                {
                    continue;
                }
            }
        }
        return true;
    }

    private bool targetInLOS(Vector3 mousePos) //target in Line of Sight
    {
        Vector3 castDir = this.transform.position - mousePos;
        RaycastHit hit;
        Ray ray = new Ray(mousePos, castDir);
        if (Physics.Raycast(ray, out hit, sightRadius))
        {
            if (hit.collider != null && hit.collider.tag == "Obstacle")
            {
                return false;
            }
        }
        return true;
    }

    void spawnbrella()
    {
        if (umbrellaCd > 0)
        {
            return;
        }
        if (currUmbrella != null)
        {
            Destroy(currUmbrella.gameObject);
            currUmbrella = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hit = 0;
        if (playground.Raycast(ray, out hit))
        {
            Vector3 mousePos = ray.GetPoint(hit);
            if (targetInLOS(mousePos)) {
                anim.SetBool("Throw", true);
                Rigidbody umbrella = GameObject.Instantiate(umbrellaPrefab, transform.position, transform.rotation);
                StartCoroutine(openerella(umbrella, umbrella.transform.position, mousePos));
                currUmbrella = umbrella;
                anim.SetBool("Umbrella", true);
            }
        }
        umbrellaCd = umbrellaMaxCd;
    }

    IEnumerator openerella(Rigidbody umbrella, Vector3 pos, Vector3 mousePos)
    {
        float delay = 0.6f;
        float t = 0;

        while(t < delay)
        {
            umbrella.MovePosition(Vector3.Lerp(pos, mousePos, t/delay));
            t += Time.deltaTime;
            yield return null;
        }
        open(umbrella.gameObject);
        anim.SetBool("Throw", false);
    }

    void open(GameObject umbrella)
    {
        Destroy(umbrella.GetComponent<SpriteRenderer>());
        umbrella.transform.GetChild(0).gameObject.SetActive(true);
        Debug.Log(umbrella.GetComponent<SpriteRenderer>());
    }


    void takeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            Debug.Log("YOU DIED.");
        }
    }
}
