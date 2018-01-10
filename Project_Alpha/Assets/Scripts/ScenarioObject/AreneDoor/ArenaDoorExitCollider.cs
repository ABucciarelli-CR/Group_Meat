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
        public LayerMask playerLayerMask;
        public bool thisDoorClosed = false;

        private GameObject gameManager;
		private GlobalVariables globalVariables;
        private RaycastHit2D hitRight;
        private float maxDistance = 5;

        void Start()
		{
            hitRight = new RaycastHit2D();
            //maxDistance = Mathf.Infinity;
            checkArenaEnemyEnd = door.GetComponent<CheckArenaEnemyEnd>();
            gameManager = GameObject.Find ("GameManager");
			globalVariables = gameManager.GetComponent<GlobalVariables> ();
            playerLayerMask = (1 << LayerMask.NameToLayer("player")) | (1 << LayerMask.NameToLayer("midGhost"));
        }

        private void Update()
        {
            //Debug.Log(playerLayerMask.value);
            hitRight = Physics2D.Raycast(transform.position, -Vector2.left, maxDistance, playerLayerMask);

            if(hitRight.collider != null)
            {
                //Debug.Log("Here!1");
                if (hitRight.collider.CompareTag("Player"))
                {
                    //Debug.Log("Here!2");
                    for (int i = 0; i < checkArenaEnemyEnd.spawner.Length; i++)
                    {
                        //Debug.Log("UNLIMITATE!");
                        checkArenaEnemyEnd.spawner[i].GetComponent<EnemySpawner>().startSpawning = true;
                    }

                    globalVariables.closeDoor = true;
                    globalVariables.enemyDead = 0;

                    thisDoorClosed = true;
                }
            }
        }
        /*
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
        }*/
    }
}
