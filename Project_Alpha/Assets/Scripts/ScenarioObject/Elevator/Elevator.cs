using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform target;
    public float speed;
    private Rigidbody2D rb2;
    // Update is called once per frame
    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!(transform.position == target.position))
            {
                rb2.AddForce(Vector2.up*speed,ForceMode2D.Force);
            }
        }
        
    }
}
