using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheArenaDoor
{
    public class ArenaDoorExitCollider : MonoBehaviour
    {
		private GameObject gameManager;
		private GlobalVariables globalVariables;

		public bool thisDoorClosed = false;

		void Start()
		{
			gameManager = GameObject.Find ("GameManager");
			globalVariables = gameManager.GetComponent<GlobalVariables> ();
		}

        private void OnTriggerExit2D(Collider2D collision)
        {
            //Debug.Log(collision);
            if (collision.CompareTag("Player"))
            {
				globalVariables.closeDoor = true;
				thisDoorClosed = true;
            }
        }
    }
}
