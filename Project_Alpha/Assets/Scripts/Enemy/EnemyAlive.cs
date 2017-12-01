using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlive : MonoBehaviour 
{

	public float speed = .01f;
    public float life = 0;

    public GameObject deadBody;

	private Vector2 movement;
	private float direction = 1;
	private float timeToChangeDirection = 5f;
	private float ttcd;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		ttcd += Time.deltaTime;

		if(ttcd >= timeToChangeDirection)
		{
			ttcd = 0;
			direction *= -1;
		}
		movement = new Vector2(direction * speed, 0);
		gameObject.transform.Translate (movement);

        if(life <=0)
        {
            Instantiate(deadBody, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity);

        }

	}
}
