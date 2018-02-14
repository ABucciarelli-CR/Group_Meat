using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Title("Nemico da far spawnare.")]
    public GameObject enemy;

    [Title("Spawn Nemici.")]
    public int enemyNumber = 1;
    public int spawnAtStart = 1;
    public int spawnAtAllSpawnedDeath = 1;

    [Title("Tempi.")]
    public float spawnDelayAtStart = 1f;
    public float spawnDelayAtAllSpawnedDeath = 1f;
    public float spawnDelayBetweenEnemy = 1f;
    
    [Title("ReadOnly, modifiche disabilitate.")]
    [ReadOnly]
    public List<GameObject> enemyList;
    [ReadOnly]
    public bool startSpawning = false;

	private GlobalVariables globalVariables;

    private bool AddEnemyAtExternal = false;
    private bool canSpawn = false;
    private bool waited = false;
    private bool initialSpawn = false;
    private bool canCheck = true;
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
        if(initialSpawn)
        {
            StartCoroutine(Delay(spawnDelayAtStart));
            SpawnAtStart();
            initialSpawn = false;
        }


        if (enemyNumber > 0 && canSpawn && waited)
        {
            //Spawn();
        }

        if(CheckAllEnemyInListIfExist() && startSpawning)
        {
            canSpawn = true;
        }

	}

    private void StartToSpawn(bool doSpawn)
    {
        startSpawning = doSpawn;
        initialSpawn = true;
    }

    private void AddEnemy(List<GameObject> externalEntity)
    {
        AddEnemyAtExternal = true;
        whoExternalEntity = externalEntity;
    }

    private void SpawnAtStart()
    {
        //for(int i = 0; i < spawnAtStart; i++)
        //{
        if(waited)
        {
            Spawn(spawnAtStart, spawnDelayBetweenEnemy);
        }
        //}
    }

    private void Spawn(int numberOfEnemies, float waitTime)
    {
        //test con canspawn iniziale
        canSpawn = false;

        canCheck = false;
        for (; numberOfEnemies > 0;)
        {
            if (waited)
            {
                GameObject _enemy = Instantiate(enemy, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                enemyList.Add(_enemy);
                if (AddEnemyAtExternal)
                {
                    whoExternalEntity.Add(_enemy);
                }
                waited = false;
                StartCoroutine(Delay(waitTime));
                numberOfEnemies--;
            }
        }
        canCheck = true;
    }

    private bool CheckAllEnemyInListIfExist()
    {
        if (enemyList.Count > 0 && canCheck)
        {
            foreach (GameObject thisEnemy in enemyList)
            {
                if (thisEnemy == null)
                {
                    enemyList.Remove(thisEnemy);
                }
            }
        }

        //return true if there's enemy on list
        /*if(enemyList.Count == 0)
        {
            Debug.Log("empty");
        }*/
        if (enemyList.Count == 0 && canCheck)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator Delay(float wait)
    {
        yield return new WaitForSeconds(wait);
        waited = true;
    }
}
