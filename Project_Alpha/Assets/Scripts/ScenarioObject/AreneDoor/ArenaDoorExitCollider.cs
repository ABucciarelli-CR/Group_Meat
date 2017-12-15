using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TheArenaDoor
{
    [RequireComponent(typeof(CheckArenaEnemyEnd))]
    public class ArenaDoorExitCollider : MonoBehaviour
    {
        CheckArenaEnemyEnd checkArenaEnemyEnd;

        public GameObject door;

        private GameObject gameManager;
		private GlobalVariables globalVariables;

		public bool thisDoorClosed = false;

		void Start()
		{
            checkArenaEnemyEnd = door.GetComponent<CheckArenaEnemyEnd>();
            gameManager = GameObject.Find ("GameManager");
			globalVariables = gameManager.GetComponent<GlobalVariables> ();
		}

        private void OnTriggerExit2D(Collider2D collision)
        {
            //Debug.Log(collision);
            if (collision.CompareTag("Player"))
            {
				
                for(int i = 0; i < checkArenaEnemyEnd.spawner.Length; i++)
                {
                    //Debug.Log("UNLIMITATE!");
                    checkArenaEnemyEnd.spawner[i].GetComponent<EnemySpawner>().startSpawning = true;
                }

                globalVariables.closeDoor = true;

                thisDoorClosed = true;
            }
        }
    }
}
