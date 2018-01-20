using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform target;
    public Transform target2;
    public bool move = false;
    public float speed;
    private Rigidbody2D rb2d;
    // Use this for initialization
    void Start ()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        if (move)
        {
            rb2d.AddForce(new Vector2(0, speed * 1000));
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!(transform.position == target.position))
            {
                move = true;
                //transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            }
        }
        if (collision.CompareTag("Enemy"))
        {
            if (!(transform.position == target.position))
            {
                move = true;
                //transform.position = Vector3.MoveTowards(transform.position, target2.position, step);
            }
        }
    }
}
