﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckArenaEnemyEnd : MonoBehaviour
{

    public GameObject[] spawner;
    public List<GameObject> singleEnemy;
    public GameObject physicalDoor;
    [Title("Inserire la porta corrispondente.")]
    public GameObject[] otherDoor;

    //private GlobalVariables globalVariables;
    private bool onlyOne = true;

    [Title("ReadOnly, modifiche disabilitate.")]
    [ReadOnly]
    public int enemyCountDown = 0;
    [ReadOnly]
    public bool enemyEnd = true;

	// Use this for initialization
	void Start ()
    {
        //globalVariables = GameObject.Find("GameManager").GetComponent<GlobalVariables>();

        for (int i = 0; i < spawner.Length; i++)
        {
            //Debug.Log(spawner[i].GetComponent<EnemySpawner>().name);
            //enemyCountDown += spawner[i].GetComponent<EnemySpawner>().enemyNumber;
            enemyCountDown += spawner[i].GetComponent<EnemySpawnerStateMachine>().enemyNumber;
        }

        for (int i = 0; i < singleEnemy.Count; i++)
        {
            enemyCountDown ++;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        /*if(globalVariables.enemyDead == enemyCountDown && enemyCountDown != 0)
        {
            globalVariables.closeDoor = false;
            globalVariables.enemyDead = 0;
            enemyCountDown = 0;
        }*/

        foreach (GameObject _enemy in singleEnemy)
        {
            if (_enemy == null)
            {
                singleEnemy.Remove(_enemy);
                break;
            }
        }

        enemyEnd = true;

        foreach (GameObject _spawner in spawner)
        {
            if(!_spawner.GetComponent<EnemySpawnerStateMachine>().endSpawn)
            {
                enemyEnd = false;
                break;
            }
        }
        
        if(singleEnemy.Count > 0)
        {
            enemyEnd = false;
        }

        //Debug.Log("enemy end: " + enemyEnd);

        if (enemyEnd)
        {
            if(onlyOne)
            {
                onlyOne = false;
                //globalVariables.closeDoor = false;
                physicalDoor.SetActive(false);

                foreach(GameObject door in otherDoor)
                {
                    door.SendMessage("CloseAndOpen", false);
                }
            }
            //globalVariables.enemyDead = 0;
            //enemyCountDown = 0;
        }
	}
}
