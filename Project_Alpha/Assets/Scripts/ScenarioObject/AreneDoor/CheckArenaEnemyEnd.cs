using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckArenaEnemyEnd : MonoBehaviour
{

    public GameObject[] spawner;
    public GameObject[] singleEnemy;

    private GlobalVariables globalVariables;

    [Title("ReadOnly, modifiche disabilitate.")]
    [ReadOnly]
    public int enemyCountDown = 0;

	// Use this for initialization
	void Start ()
    {
        globalVariables = GameObject.Find("GameManager").GetComponent<GlobalVariables>();

        for (int i = 0; i < spawner.Length; i++)
        {
            //Debug.Log(spawner[i].GetComponent<EnemySpawner>().name);
            //enemyCountDown += spawner[i].GetComponent<EnemySpawner>().enemyNumber;
            enemyCountDown += spawner[i].GetComponent<EnemySpawnerStateMachine>().enemyNumber;
        }

        for (int i = 0; i < singleEnemy.Length; i++)
        {
            enemyCountDown ++;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(globalVariables.enemyDead == enemyCountDown && enemyCountDown != 0)
        {
            Debug.Log("Here!");
            globalVariables.closeDoor = false;
            globalVariables.enemyDead = 0;
            enemyCountDown = 0;
        }
	}
}
