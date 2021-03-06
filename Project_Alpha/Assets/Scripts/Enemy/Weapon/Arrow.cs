﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject player;
    public bool friendlyFire = true;
    public float arrowVelocity = 200f;
	public float arrowDamage = 15f;

    public float precision = 1f;

    private Rigidbody2D rb2d;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }


	void Start ()
    {
        
        var dir = player.transform.position - this.transform.position;
        var angle = Mathf.Atan2(dir.y + Random.Range(-precision, precision), dir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        rb2d.AddRelativeForce(new Vector2(arrowVelocity, 0), ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Called");
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("Damage", arrowDamage);
            if (player.GetComponentInChildren<Life>().actualLife <= 0)
            {
                GameObject.Find("DeathBy").GetComponent<LastEnemyDamage>().KilledBy("Archer");
            }
        }
        else if(friendlyFire)
        {
            if(collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.SendMessage("Damage", arrowDamage);
            }
        }
        Destroy(this.gameObject);
    }

    public void AtkSet(float newAtk)
    {
        arrowDamage = newAtk;
    }
}
