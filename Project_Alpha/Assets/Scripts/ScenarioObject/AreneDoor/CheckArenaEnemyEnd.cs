using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckArenaEnemyEnd : MonoBehaviour
{

    public GameObject[] spawner;

    private GlobalVariables globalVariables;

    public int enemyCountDown;

	// Use this for initialization
	void Start ()
    {
        globalVariables = GameObject.Find("GameManager").GetComponent<GlobalVariables>();

        for (int i = 0; i < spawner.Length; i++)
        {
            //Debug.Log(spawner[i].GetComponent<EnemySpawner>().name);
            enemyCountDown += spawner[i].GetComponent<EnemySpawner>().enemyNumber;
        }
        
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(globalVariables.enemyDead == enemyCountDown /*&& enemyCountDown != 0*/)
        {
            globalVariables.closeDoor = false;
            globalVariables.enemyDead = 0;
            enemyCountDown = 0;
        }
	}
}
