using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;

    public int enemyNumber = 1;

    public float spawnRate = 1;

    public bool startSpawning = false;

    private float spawnTime = 0;

	private GlobalVariables globalVariables;

    private bool AddEnemyAtExternal = false;
    private List<GameObject> whoExternalEntity;

    

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
                if(AddEnemyAtExternal)
                {
                    whoExternalEntity.Add(enemy);
                }
                enemyNumber--;
                spawnTime = 0;
            }
            spawnTime += Time.deltaTime;
        }
	}

    private void StartToSpawn(bool doSpawn)
    {
        startSpawning = doSpawn;
    }

    private void AddEnemy(List<GameObject> externalEntity)
    {
        AddEnemyAtExternal = true;
        whoExternalEntity = externalEntity;
    }
}
