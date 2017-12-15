using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;

    public int enemyNumber = 1;

    public float spawnRate = 1;

    private float spawnTime = 0;

	private GlobalVariables globalVariables;

    public bool startSpawning = false;

	// Use this for initialization
	void Start ()
    {
		globalVariables = GameObject.Find ("GameManager").GetComponent<GlobalVariables> ();

       // enemy = GetComponent<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(enemyNumber > 0 && startSpawning/*globalVariables.closeDoor*/)
        {
            if(spawnTime >= spawnRate)
            {
                Instantiate(enemy, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                enemyNumber--;
                spawnTime = 0;
            }
            spawnTime += Time.deltaTime;
        }
	}
}
