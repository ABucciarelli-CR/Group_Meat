using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public float upNdownVelocity = 10;
    public bool upDirection = true;

    public GameObject movableLiftPieces;
    public GameObject[] spawner;

    public bool active = false;

    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = movableLiftPieces.GetComponent<Rigidbody2D>();
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		if(active)
        {
            if (upDirection)
            {
                rb2d.MovePosition(rb2d.position + new Vector2(0f, -upNdownVelocity));
            }
            else
            {
                rb2d.MovePosition(rb2d.position + new Vector2(0f, upNdownVelocity));
            }
        }
	}

    private void ActiveLift(bool activation)
    {
        active = activation;
    }
}
