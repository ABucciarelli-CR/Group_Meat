﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerStateMachine : MonoBehaviour
{
    [Title("se è presente un healer, immettere la porta che fa iniziare lo spawn del tutto")]
    public bool isHealerIn = false;
    [EnableIf("isHealerIn")]
    public GameObject startSpawningDoor;
    [EnableIf("isHealerIn")]
    public GameObject platformToRemove;

    public bool multiTypeSpawn = false;
    [EnableIf("multiTypeSpawn")]
    public List<GameObject> enemies = new List<GameObject>();

    [DisableIf("multiTypeSpawn")]
    [Title("Nemico da far spawnare.")]
    public GameObject enemy;

    [DisableIf("multiTypeSpawn")]
    [Title("Spawn Nemici.")]
    public int enemyNumber = 1;
    public int spawnAtStart = 1;
    //public int spawnAtAllSpawnedDeath = 1;  //TODO: spawn di più nemici

    [Title("Tempi.")]
    public float spawnDelayAtStart = 1f;
    //public float spawnDelayAtAllSpawnedDeath = 1f;    //TODO: spawn di più nemici
    public float spawnDelayBetweenEnemy = 1f;

    [Title("Inizio spawn manuale.")]
    public bool startSpawning = false;

    [Title("ReadOnly, modifiche disabilitate.")]
    [ReadOnly]
    public List<GameObject> enemyList;
    /*[HideInInspector] */public bool endSpawn = false;
    public GameObject pointToGo;

    private GlobalVariables globalVariables;

    private bool AddEnemyAtExternal = false;
    //private bool canSpawn = false;
    private bool waited = false;
    private bool initialSpawn = false;
    private bool canIRemoveThePlatform = false;
    
    private List<GameObject> whoExternalEntity;
    private int i = 0;
    [HideInInspector] public SpawnerState spawnerState;
    private bool isPlayerInside = false;

    public enum SpawnerState
    {
        check,
        spawn,
        inert
    }

    public void Awake()
    {
        spawnerState = SpawnerState.check;
    }

    void Update ()
    {
        if (startSpawning && !isPlayerInside)
        {
            switch (spawnerState)
            {
                case SpawnerState.check:
                    Check();
                    break;

                case SpawnerState.spawn:
                    Spawn();
                    break;

                case SpawnerState.inert:
                    Inert();
                    break;

                default:
                    break;
            }
        }

        //controllo se gli spawner all'interno dell'arena son tutti vuoti, nel caso, faccio scomparire la piattaforma
        canIRemoveThePlatform = true;
        if (isHealerIn)
        {
            foreach (GameObject singleSpawner in startSpawningDoor.GetComponent<CheckArenaEnemyEnd>().spawner)
            {
                if(!singleSpawner.name.ToString().Contains("Spawner_Healer"))
                {
                    if(!singleSpawner.GetComponent<EnemySpawnerStateMachine>().endSpawn)
                    {
                        canIRemoveThePlatform = false;
                    }
                }

                if (!canIRemoveThePlatform)
                {
                    break;
                }
            }

            //se non ci sono più nemici da far spawnare rimuovo la piattaforma(se ce l'ho)
            if (canIRemoveThePlatform && platformToRemove != null)
            {
                platformToRemove.SetActive(false);
            }
        }

        
    }
    //__________________________state machine things____________________________________________//

    public void Spawn()
    {
        if (waited)
        {
            GameObject thisEnemy;
            if(multiTypeSpawn)
            {
                thisEnemy = enemies[i];
                i++;
            }
            else
            {
                thisEnemy = enemy;
            }

            GameObject _enemy = Instantiate(thisEnemy, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            _enemy.GetComponent<EnemyStateMachine>().pointGoto = pointToGo;
            enemyList.Add(_enemy);
            if (AddEnemyAtExternal)
            {
                whoExternalEntity.Add(_enemy);
            }
            waited = false;
            enemyNumber--;
            spawnerState = SpawnerState.check;
            /*
            if(gameObject.name == "Spawner_Healer")
            {
                gameObject.GetComponent<HealerSpawnerArena>().myHealer.Add(_enemy);
            }*/
        }
    }

    public void Check()
    {
        if (enemyList.Count > 0)
        {
            foreach (GameObject thisEnemy in enemyList)
            {
                if (thisEnemy == null)
                {
                    //Debug.Log("remove");
                    enemyList.Remove(thisEnemy);
                    break;
                }
            }
        }

        if(multiTypeSpawn)
        {
            if (enemies.Count > i && enemyList.Count <= 0)
            {
                if (initialSpawn)
                {
                    StartCoroutine(Delay(spawnDelayAtStart));
                }
                else
                {
                    StartCoroutine(Delay(spawnDelayBetweenEnemy));
                }

                spawnerState = SpawnerState.spawn;

            }
            else if(enemyList.Count <= 0)
            {
                spawnerState = SpawnerState.inert;
                endSpawn = true;
            }
        }
        else
        {
            if (enemyNumber > 0 && enemyList.Count <= 0)
            {
                if (initialSpawn)
                {
                    StartCoroutine(Delay(spawnDelayAtStart));
                }
                else
                {
                    StartCoroutine(Delay(spawnDelayBetweenEnemy));
                }

                spawnerState = SpawnerState.spawn;

            }
            else if (enemyList.Count <= 0)
            {
                spawnerState = SpawnerState.inert;
                endSpawn = true;
            }
        }
    }

    public void Inert()
    {
        Debug.Log("Do nothing, biatch!");
    }

    //__________________________other things____________________________________________//

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isPlayerInside = true;
        }
        else
        {
            isPlayerInside = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerInside = true;
        }
        else
        {
            isPlayerInside = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerInside = false;
        }
    }

    IEnumerator Delay(float wait)
    {
        yield return new WaitForSeconds(wait);
        waited = true;
    }
}
