using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {
    public Transform target;
    public Transform target2;
    public float speed;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            float step = speed * Time.deltaTime;
            if (!(transform.position == target.position))
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            }
        }
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("EHI LISTEN");
            float step = speed * Time.deltaTime;
            if (!(transform.position == target.position))
            {
                transform.position = Vector3.MoveTowards(transform.position, target2.position, step);
            }
        }
    }
}
