using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerStateMachine : MonoBehaviour
{
    [Title("Nemico da far spawnare.")]
    public GameObject enemy;

    [Title("Spawn Nemici.")]
    public int enemyNumber = 1;
    public int spawnAtStart = 1;
    //public int spawnAtAllSpawnedDeath = 1;  //TODO: spawn di più nemici

    [Title("Tempi.")]
    public float spawnDelayAtStart = 1f;
    //public float spawnDelayAtAllSpawnedDeath = 1f;    //TODO: spawn di più nemici
    public float spawnDelayBetweenEnemy = 1f;

    [Title("ReadOnly, modifiche disabilitate.")]
    [ReadOnly]
    public List<GameObject> enemyList;
    [ReadOnly]
    public bool startSpawning = false;
    [HideInInspector] public bool endSpawn = false;

    private GlobalVariables globalVariables;

    private bool AddEnemyAtExternal = false;
    //private bool canSpawn = false;
    private bool waited = false;
    private bool initialSpawn = false;
    
    private List<GameObject> whoExternalEntity;
    [HideInInspector] public SpawnerState spawnerState;

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
        if (startSpawning)
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
    }

    //__________________________state machine things____________________________________________//

    public void Spawn()
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
            enemyNumber--;
            spawnerState = SpawnerState.check;

            if(gameObject.name == "Spawner_Healer")
            {
                gameObject.GetComponent<HealerSpawnerArena>().myHealer.Add(_enemy);
            }
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

        if(enemyNumber > 0 && enemyList.Count <= 0)
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

    IEnumerator Delay(float wait)
    {
        yield return new WaitForSeconds(wait);
        waited = true;
    }

}
