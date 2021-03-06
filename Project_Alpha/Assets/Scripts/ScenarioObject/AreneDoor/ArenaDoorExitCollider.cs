﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheArenaDoor
{
    [RequireComponent(typeof(CheckArenaEnemyEnd))]
    public class ArenaDoorExitCollider : MonoBehaviour
    {
        CheckArenaEnemyEnd checkArenaEnemyEnd;

        public GameObject door;
        
        public LayerMask playerLayerMask;
        public bool thisDoorClosed = false;

        //private GameObject gameManager;
		//private GlobalVariables globalVariables;
        private RaycastHit2D hitRight;
        private float maxDistance = 5;

        [HideInInspector]public bool isPlayer = false;
        private bool onlyOne = true;

        void Start()
		{
            hitRight = new RaycastHit2D();
            //maxDistance = Mathf.Infinity;
            checkArenaEnemyEnd = door.GetComponent<CheckArenaEnemyEnd>();
            //gameManager = GameObject.Find ("GameManager");
			//globalVariables = gameManager.GetComponent<GlobalVariables> ();
            playerLayerMask = (1 << LayerMask.NameToLayer("player")) | (1 << LayerMask.NameToLayer("midGhost"));
        }

        private void Update()
        {
            if (isPlayer)
            {
                //Debug.Log("Here!2");
                for (int i = 0; i < checkArenaEnemyEnd.spawner.Length; i++)
                {
                    //Debug.Log("UNLIMITATE!");
                    //checkArenaEnemyEnd.spawner[i].GetComponent<EnemySpawner>().startSpawning = true;
                    checkArenaEnemyEnd.spawner[i].GetComponent<EnemySpawnerStateMachine>().startSpawning = true;
                }

                if(onlyOne)
                {
                    foreach (GameObject door in checkArenaEnemyEnd.otherDoor)
                    {
                        door.SendMessage("CloseAndOpen", true);
                    }
                    checkArenaEnemyEnd.physicalDoor.SetActive(true);
                    //globalVariables.closeDoor = true;
                    onlyOne = false;
                }
                //globalVariables.enemyDead = 0;

                
                thisDoorClosed = true;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                isPlayer = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isPlayer = true;
            }
        }
    }
}
