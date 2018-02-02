using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    public float upNdownVelocity = 10;
    public bool upDirection = true;

    public GameObject movableLiftPieces;
    public GameObject movableBackGround;
    public GameObject[] spawner;

    public bool active = false;

    private Rigidbody2D rb2dMovableLiftPieces;
    private Rigidbody2D rb2dBackground;

    private void Awake()
    {
        rb2dMovableLiftPieces = movableLiftPieces.GetComponent<Rigidbody2D>();
        rb2dBackground = movableBackGround.GetComponent<Rigidbody2D>();
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
                rb2dMovableLiftPieces.MovePosition(rb2dMovableLiftPieces.position + new Vector2(0f, -upNdownVelocity));
                rb2dBackground.MovePosition(rb2dBackground.position + new Vector2(0f, -upNdownVelocity));
            }
            else
            {
                rb2dMovableLiftPieces.MovePosition(rb2dMovableLiftPieces.position + new Vector2(0f, upNdownVelocity));
                rb2dBackground.MovePosition(rb2dBackground.position + new Vector2(0f, upNdownVelocity));
            }
        }
	}

    private void ActiveLift(bool activation)
    {
        active = activation;
    }
}
